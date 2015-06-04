using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour 
{
	public int PlayerNumber;
	public GameObject opponent;

	public KeyCode up;
	public KeyCode down;
	public KeyCode left;
	public KeyCode right;
	public float speed;
	public float shootBulletSpeed;
	public KeyCode shootBullet;
	public KeyCode shootSecondaryBullet;
	public KeyCode shootDoubleBullets;
	private float secondaryShootCD = 0;
	private float primaryShootCD = 0;
	private float primaryShootCDReset = 25;

	private bool showReloadingReady;

	public GameObject NuzzleFireGun;
	public GameObject NuzzleFirePowerUpYellow;
	public GameObject NuzzleFirePowerUpBlue;

	public GameObject ship;
	public GameObject spawnPointFront;
	public float spawnPointFrontX;

	public int currentShotLevel = 1;
	public string currentShotType;
	public GameObject bullet;
	public GameObject doubleBullet;
	public GameObject VictoryScreen; 

	private bool isMoving;
	private bool isSpeedingUp;
	private AudioSource audioSrc;
	public bool isDying = false;
	public bool isBeingSuckedIntoBlackHole = false;

	public int numCollectedPowerups	= 0;
	public int numCollectedBluePowerups = 0;
	public int numCollectedYellowPowerups = 0;
	public int numCollectedPurplePowerups = 0;
	public GameObject gObjPowerUpLevelText;

//	public Text txtCollectedPowerups;
//	public Text txtCollectedBluePowerups;	
//	public Text txtCollectedYellowPowerups;
//	public Text txtCollectedPurplePowerups;
	public Text txtOpponentScore;

	public GameObject assJetWhite;
	public GameObject assJetWhiteDots;
	public GameObject assJetWhiteLineA;
	public GameObject assJetWhiteLineB;
	public GameObject assJetBlue;
	public GameObject assJetYellow;
	public GameObject assJetPurple;

	public GameObject bulletPowerUpYellow;
	public GameObject bulletPowerUpBlue;
	public GameObject bulletPowerUpPurple;
	public AudioClip bulletHit_mp3;
	
	public GameObject absorbEffectBlue;
	public GameObject absorbEffectYellow;
	public GameObject absorbEffectPurple;
	public AudioClip absorbEffectBlue_mp3;
	public AudioClip absorbEffectYellow_mp3;
	public AudioClip absorbEffectPurple_mp3;

	private float rotationSpeed = 2.0f;
	public float paralyzeTime = 0;
	public ParticleSystem PfxParalysed;

	bool atRight;
	
	// Use this for initialization
	void Start () 
	{
//		transform.FindChild ("Ship").gameObject.GetComponent<ClickOrTapToExplode>().StartExplosion();
//		Destroy(transform.root.gameObject);

		// At right?
		atRight = Camera.main.WorldToScreenPoint (transform.root.gameObject.transform.position).x > Screen.width / 2;

		// Save reference to Flight Sound Audio Source
		audioSrc = GetComponent<AudioSource>();
		audioSrc.volume = 0.4f;
	}

	void UpdateScores()
	{
		if (this.name.Replace("(Clone)", "") == "Ship")
		{
			txtOpponentScore.text = LocalDB.Player1_Score.ToString();
		}
		else
		{
			txtOpponentScore.text = LocalDB.Player2_Score.ToString();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Update Scores
		UpdateScores();

		// Drag towards Black Hole
		transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0, 0, 0), 0.005f); 

		// Physics when not being sucked into blackhole
		if(isDying == false)
		{
			physicsControl();
			flightSound();
			checkShooting();
			lowerShootCooldown();
		}
		else
		{
			AnimateDeath();
		}
	}

	void showReloadEffect ()
	{
		if(currentShotType == "PowerUpYellow") 
		{
			transform.FindChild ("ReloadYellow").gameObject.SetActive(true);
			transform.FindChild ("ReloadYellow").gameObject.GetComponent<Reloading> ().reset(); 
		}
		else if(currentShotType == "PowerUpBlue") 
		{
			transform.FindChild ("ReloadBlue").gameObject.SetActive(true);
			transform.FindChild ("ReloadBlue").gameObject.GetComponent<Reloading> ().reset();
		}
		else if(currentShotType == "PowerUpPurple")
		{
			transform.FindChild ("ReloadPurple").gameObject.SetActive(true);
			transform.FindChild ("ReloadPurple").gameObject.GetComponent<Reloading> ().reset();
		}
	}

	void lowerShootCooldown ()
	{
		//if(atRight == false) print (secondaryShootCD);
		if(secondaryShootCD > 0 ) {
			secondaryShootCD -= Time.timeScale;
		}
		if(secondaryShootCD < 20 && showReloadingReady == true) 
		{
			showReloadEffect();
			showReloadingReady = false;
		}

		if (primaryShootCD > 0 ) {
			primaryShootCD -= Time.timeScale;
		}
	}

	void AnimateDeath()
	{
		// Shrink
		float scaleSpeed = 0.02f;
		ship.transform.localScale -= new Vector3((scaleSpeed*1.2f) * Time.timeScale, (scaleSpeed*0.8f) * Time.timeScale, (scaleSpeed) * Time.timeScale);

		// Move a little
		float moveSpeed = 0.005f;
		ship.transform.position = new Vector3(ship.transform.position.x + ((atRight) ? -moveSpeed : moveSpeed), ship.transform.position.y, ship.transform.position.z);

		if(ship.transform.localScale.x < 0.0f)
		{
			Destroy(GameObject.Find(ship.transform.parent.gameObject.name));
		}
	}

	public void flightSound()
	{
		if(audioSrc != null)
		{
			if(isMoving)
			{
				if(isSpeedingUp == false)
				{
					audioSrc.time = 0;
					isSpeedingUp = true;
				}
				
				if (audioSrc.time == 0)
				{
					audioSrc.Play();
				}
				else if (audioSrc.time > 1.405f)
				{
					audioSrc.time = 0.85f;
				}
			}
			else if (audioSrc.time != 0 && audioSrc.time < 1.4f)
			{
				isSpeedingUp = false;
				audioSrc.time = 1.4f;
				audioSrc.Play();
			}
			
			if (audioSrc.time == 2.548f)
			{
				audioSrc.time = 0;
			}
		}
	}

	public void physicsControl()
	{
		//print (paralyzeTime);
		if (paralyzeTime > 0) 
		{
			paralyzeTime -= Time.timeScale;
			// Show paralysed effect
			PfxParalysed.Play();
			PfxParalysed.enableEmission = true;
		} 
		else 
		{
			// Don't show paralysed	
			PfxParalysed.enableEmission = false;

			// Check rotation
			float maxRotation = 0.4f;
			if (Input.GetKey (right) && transform.localRotation.x < maxRotation) transform.Rotate (new Vector3 (rotationSpeed, 0f));
			else if (Input.GetKey (left) && transform.localRotation.x > -maxRotation) transform.Rotate (new Vector3 (-rotationSpeed, 0f));
			else
			{
				// Rotate Back
				if (transform.localRotation.x > 0) transform.Rotate (new Vector3 (-rotationSpeed, 0f));
				else if (transform.localRotation.x < 0) transform.Rotate (new Vector3 (rotationSpeed, 0f));
			}

			// Calculate movement
			if (Input.GetKey (right)) {
				GetComponent<Rigidbody>().AddForce(new Vector3(0,speed,0),ForceMode.Force);
			}
			
			if (Input.GetKey (left)) {
				GetComponent<Rigidbody>().AddForce(new Vector3(0,-speed,0),ForceMode.Force);
			}
			
			if (Input.GetKey (up)) {
				if(PlayerNumber == 1){
					GetComponent<Rigidbody>().AddForce(new Vector3(-speed,0,0),ForceMode.Force);
				}
				if(PlayerNumber == 2){
					GetComponent<Rigidbody>().AddForce(new Vector3(speed,0,0),ForceMode.Force);
				} 
			}
			
			if (Input.GetKey (down)) {
				if(PlayerNumber == 1){
					GetComponent<Rigidbody>().AddForce(new Vector3(speed*0.5f,0,0),ForceMode.Force);
				}
				if(PlayerNumber == 2){
					GetComponent<Rigidbody>().AddForce(new Vector3(-speed*0.5f,0,0),ForceMode.Force);
				} 		
			}
			
			// For Sound
			if(Input.GetKey (right) || Input.GetKey (left) || Input.GetKey (up) || Input.GetKey (down)) 
				isMoving = true;
			else isMoving = false;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		string name = other.collider.name.Replace("(Clone)", "");

		CollisionWithPowerUp(other, name);
		CollisionWithBlackHole(other, name);
		CollisionWithBullet(other, name);
	}

	void CollisionWithBullet (Collision other, string nameOfCollider)
	{
		if (nameOfCollider == "BulletGreen" || nameOfCollider == "BulletRed")
		{
			Script_SlowMotionSound_triggered scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered> ();
			scr.playSound2();
		}
	}

	private void CollisionWithBlackHole(Collision other, string nameOfCollider)
	{
		if(nameOfCollider == "TheBlackhole")
		{
			if(isBeingSuckedIntoBlackHole == false)
			{
				isBeingSuckedIntoBlackHole = true;

				IncreaseScore();
				UpdateScores();
			}

			// Stop Jet
			this.hideJetasses();
			this.assJetWhiteDots.SetActive(false);
			this.assJetWhiteLineA.SetActive(false);
			this.assJetWhiteLineB.SetActive(false);
		}
	}

	public void IncreaseScore ()
	{
		if (this.name.Replace("(Clone)", "") == "Ship")
		{
			LocalDB.Player2_Score = LocalDB.Player2_Score + 1;
		}
		else
		{
			LocalDB.Player1_Score = LocalDB.Player1_Score + 1;
		}
	}

	private void ImprovePowerUpShot(string shotType)
	{
		// Change Powerup Type if not the current + reset level
		if (currentShotType != shotType)
		{
			currentShotType = shotType;
			currentShotLevel = 0;
		}

		// Improve level of Powerup
		if (currentShotType == shotType)
		{
			if(currentShotLevel < 3)
			{
				currentShotLevel++;
			}
		}

		// Show Pfx and text about the level of the powerup
		GameObject gObj = Instantiate(gObjPowerUpLevelText, transform.position, new Quaternion()) as GameObject;
		if(shotType == "PowerUpYellow")
		   	(gObj.transform.FindChild("yellow") as Transform).gameObject.GetComponent<ParticleSystem>().Play();
		else if (shotType == "PowerUpBlue")
			(gObj.transform.FindChild("blue") as Transform).gameObject.GetComponent<ParticleSystem>().Play();
		else if (shotType == "PowerUpPurple")
			(gObj.transform.FindChild("purple") as Transform).gameObject.GetComponent<ParticleSystem>().Play();
		(gObj.transform.FindChild("text") as Transform).gameObject.GetComponent<TextMesh>().text = "lvl "+currentShotLevel;
	}

	void calculateAmazingnessOfReadySignal (ParticleSystem particleSystem)
	{
		particleSystem.loop = true;
		particleSystem.Play();

		if (currentShotLevel == 1){
			particleSystem.startSpeed = 0.7f;
			particleSystem.startLifetime = .4f;
		} else if (currentShotLevel == 2){
			particleSystem.startSpeed = 1.0f;
			particleSystem.startLifetime = .65f;
		} else if (currentShotLevel == 3){
			particleSystem.startSpeed = 1.3f;
			particleSystem.startLifetime = .9f;
		}
	}

	private void CollisionWithPowerUp(Collision other, string name)
	{
		int ranNum = (name == "PowerUpRandom") ? Random.Range(1, 4) : 0;
		float volumeOfAbsorb = 0.4f;

		if (name == "PowerUpBlue" || ranNum == 1)
		{
			hideJetasses();
			hideSecondaryShotReadySignals();
			assJetBlue.SetActive(true);

			// Show Ready Signal of Blue
			calculateAmazingnessOfReadySignal(transform.FindChild("ReloadBlue").FindChild("pfxReady").gameObject.GetComponent<ParticleSystem>());
			calculateAmazingnessOfReadySignal(transform.FindChild("ReloadBlue").FindChild("pfxReady2").gameObject.GetComponent<ParticleSystem>());

			// Pfx Ring
			Instantiate(absorbEffectBlue, transform.position, new Quaternion());
			// Sound
			Script_SlowMotionSound_triggered scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered> ();
			scr.playSound3(false, volumeOfAbsorb);
			//AudioSource.PlayClipAtPoint (absorbEffectBlue_mp3, GameObject.Find("Main Camera").GetComponent<Transform>().position);

			// Improve Power Up
			ImprovePowerUpShot("PowerUpBlue");
		}
		else if (name == "PowerUpYellow" || ranNum == 2)
		{
			hideJetasses();
			hideSecondaryShotReadySignals();
			assJetYellow.SetActive(true);

			// Show Ready Signal of Blue
			calculateAmazingnessOfReadySignal(transform.FindChild("ReloadYellow").FindChild("pfxReady").gameObject.GetComponent<ParticleSystem>());
			calculateAmazingnessOfReadySignal(transform.FindChild("ReloadYellow").FindChild("pfxReady2").gameObject.GetComponent<ParticleSystem>());

			// Pfx Ring
			Instantiate(absorbEffectYellow, transform.position, new Quaternion());
			Script_SlowMotionSound_triggered scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered> ();
			scr.playSound5(false, volumeOfAbsorb);
			//AudioSource.PlayClipAtPoint (absorbEffectYellow_mp3, GameObject.Find("Main Camera").GetComponent<Transform>().position);

			// Improve PowerUp
			ImprovePowerUpShot("PowerUpYellow");
		}
		else if (name == "PowerUpPurple" || ranNum == 3)
		{
			hideJetasses();
			hideSecondaryShotReadySignals();
			assJetPurple.SetActive(true);
			
			// Show Ready Signal of Blue
			calculateAmazingnessOfReadySignal(transform.FindChild("ReloadPurple").FindChild("pfxReady").gameObject.GetComponent<ParticleSystem>());
			calculateAmazingnessOfReadySignal(transform.FindChild("ReloadPurple").FindChild("pfxReady2").gameObject.GetComponent<ParticleSystem>());

			// Pfx Ring
			Instantiate(absorbEffectPurple, transform.position, new Quaternion());
			Script_SlowMotionSound_triggered scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered> ();
			scr.playSound4(false, volumeOfAbsorb);
			//AudioSource.PlayClipAtPoint (absorbEffectPurple_mp3, GameObject.Find("Main Camera").GetComponent<Transform>().position);

			// Improve PowerUp
			ImprovePowerUpShot("PowerUpPurple");
		}
	}

	private void hideJetasses()
	{
		assJetBlue.SetActive(false);
		assJetWhite.SetActive(false);
		assJetYellow.SetActive(false);
		assJetPurple.SetActive(false);
	}

	private void hideSecondaryShotReadySignals()
	{
		// Hide ready signal
		transform.FindChild("ReloadBlue").FindChild("pfxReady").gameObject.GetComponent<ParticleSystem>().loop = false;
		transform.FindChild("ReloadBlue").FindChild("pfxReady2").gameObject.GetComponent<ParticleSystem>().loop = false;
		transform.FindChild("ReloadYellow").FindChild("pfxReady").gameObject.GetComponent<ParticleSystem>().loop = false;
		transform.FindChild("ReloadYellow").FindChild("pfxReady2").gameObject.GetComponent<ParticleSystem>().loop = false;
		transform.FindChild("ReloadPurple").FindChild("pfxReady").gameObject.GetComponent<ParticleSystem>().loop = false;
		transform.FindChild("ReloadPurple").FindChild("pfxReady2").gameObject.GetComponent<ParticleSystem>().loop = false;
	}

	public void asteroidsControl()
	{
		if (Input.GetKey (up)) 
		{
			Vector3 direction = ship.transform.position - spawnPointFront.transform.position;
			GetComponent<Rigidbody>().AddForce(-direction*1.5f,ForceMode.Force);
		}
		
		if (Input.GetKey (down)) 
		{
			Vector3 direction = ship.transform.position - spawnPointFront.transform.position;
			GetComponent<Rigidbody>().AddForce(direction, ForceMode.Force);
		}
		
		if (Input.GetKey (left)) 
		{
			if (PlayerNumber == 1 && (spawnPointFront.transform.position.y - ship.transform.position.y > -1)){
				spawnPointFront.transform.Translate (new Vector3 (-0.12f, 0, 0));
				ship.transform.Rotate (Vector3.back * 5);
				ship.transform.Rotate (0,3f,0);
			}
			
			if(PlayerNumber == 2 && (spawnPointFront.transform.position.y - ship.transform.position.y < 1) ){
				spawnPointFront.transform.Translate(new Vector3(0.12f,0,0));
				ship.transform.Rotate (Vector3.back * 5);
				ship.transform.Rotate (0,-3f,0);
			}
		}
		
		if (Input.GetKey (right) ) {
			if(PlayerNumber == 1 && (spawnPointFront.transform.position.y - ship.transform.position.y < 1)){
				spawnPointFront.transform.Translate(new Vector3(0.12f,0,0));
				ship.transform.Rotate(-Vector3.back * 5);
				ship.transform.Rotate (0,-3f,0);
				
			}
			
			if(PlayerNumber == 2 && (spawnPointFront.transform.position.y - ship.transform.position.y > -1)){
				spawnPointFront.transform.Translate(new Vector3(-0.12f,0,0));
				ship.transform.Rotate(-Vector3.back * 5);
				ship.transform.Rotate (0,3f,0);
			}
		}
	}

	public void checkShooting()
	{
		//Single bullet
		if(Input.GetKeyDown(shootBullet))
		{
			if (primaryShootCD <= 0)
			{
				primaryShootCD = primaryShootCDReset;
				// Shot Sound
				//AudioSource.PlayClipAtPoint (shot, GameObject.Find("Main Camera").GetComponent<Transform>().position);
	//			Script_SlowMotionSound_triggered scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered> ();
	//			scr.playSound1();
				
				// NuzzleFire
				Vector3 pos = ship.transform.position - (new Vector3(0.5f + spawnPointFrontX, (ship.transform.position.y - spawnPointFront.transform.position.y) * 0.5f, 0f));
				GameObject bul = Instantiate(bullet, pos, new Quaternion()) as GameObject;
				BulletMovement bMov = bul.GetComponent<BulletMovement>();
				if(bMov != null) bMov.force = shootBulletSpeed;
				ShowNuzzleFireParticles(NuzzleFireGun);
				
				// Push back
				float xForce = ((name=="Ship")?100:-100) * (1 / (1-(1-Time.timeScale)));
				GetComponent<Rigidbody>().AddForce (xForce, 0, 0);
			}
		}
		
		// Secondary Bullet
		if(Input.GetKeyDown(shootSecondaryBullet))
		{
			if(secondaryShootCD <= 0)
			{
				showReloadingReady = true;

				// Hide ready signal
				hideSecondaryShotReadySignals();

				if (currentShotType == "PowerUpBlue")
				{
					// Bullet on the right side of the ship
					GameObject gObj = Instantiate(bulletPowerUpBlue, transform.position, new Quaternion()) as GameObject;
					gObj.GetComponent<BulletPowerUpBlue>().SpawnBullets(currentShotLevel, this);
					secondaryShootCD = gObj.GetComponent<BulletPowerUpBlue>().getShootCooldown(currentShotLevel);
					
					// NuzzleFire
					ShowNuzzleFireParticles(NuzzleFirePowerUpBlue);
				}
				else if (currentShotType == "PowerUpYellow")
				{
					// Bullet on the right side of the ship
					Vector3 position = ship.transform.position - (new Vector3((float)ship.transform.rotation.y, (ship.transform.position.y - spawnPointFront.transform.position.y) * 0.3f, 0f));
					Instantiate(bulletPowerUpYellow, position, new Quaternion());
					bulletPowerUpYellow.GetComponent<BulletPowerupYellowMovement>().playerScript = this;
					secondaryShootCD = bulletPowerUpYellow.GetComponent<BulletPowerupYellowMovement>().getShootCooldown(currentShotLevel);

					// NuzzleFire
					ShowNuzzleFireParticles(NuzzleFirePowerUpYellow);
				}
				else if (currentShotType == "PowerUpPurple")
				{
					// NuzzleFire
					GameObject gObj = Instantiate(bulletPowerUpPurple, transform.root.gameObject.transform.position, new Quaternion()) as GameObject;
					gObj.GetComponent<BulletPowerUpPurple>().playerScript = this;
					secondaryShootCD = gObj.GetComponent<BulletPowerUpPurple>().getShootCooldown(currentShotLevel);
				}
			}
		}
		
		//Double bullet
		if(Input.GetKeyDown(shootDoubleBullets))
		{
			// Bullet on the right side of the ship
			Instantiate(doubleBullet, ship.transform.position - (new Vector3((float)ship.transform.rotation.y + 0.5f, 
			                                                                 (ship.transform.position.y - spawnPointFront.transform.position.y) * 0.3f, 0f)), new Quaternion());
			
			// Bullet on the left side of the ship
			Instantiate(doubleBullet, ship.transform.position - (new Vector3((float)ship.transform.rotation.y - 0.5f, 
			                                                                 (ship.transform.position.y - spawnPointFront.transform.position.y) * 0.3f, 0f)), new Quaternion());
		}
	}

	public void showBlueNuzzleFire()
	{
		// NuzzleFire
		ShowNuzzleFireParticles(NuzzleFirePowerUpBlue);
	}

	public void ShowNuzzleFireParticles(GameObject gObj)
	{
		// Left Nuzzle Fire
		ParticleSystem psLeft = gObj.transform.FindChild("left").GetComponent<ParticleSystem>();
		if(psLeft != null)
		{
			psLeft.Clear();
			psLeft.Play();
		}

		// Right Nuzzle Fire
		ParticleSystem psRight = gObj.transform.FindChild("right").GetComponent<ParticleSystem>();
		psRight.Clear();
		psRight.Play();
	}
}
