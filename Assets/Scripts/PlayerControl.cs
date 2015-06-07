using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour 
{
	public static int Player1Score { get; set; }
	public static int Player2Score { get; set; }

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
	public KeyCode useSpecialAbility;
	public KeyCode shootDoubleBullets;
	private float secondaryShootCD = 0;
	private float primaryShootCD = 0;
	private float primaryShootCDReset = 15;
	private float specialAbilityCD = 0;
	private float specialAbility1_CDReset = 300;
	private float specialAbility1_Duration = 0;
	private float specialAbility1_DurationReset = 100;
	public int currentAbility;

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
			txtOpponentScore.text = Player1Score.ToString();
		}
		else
		{
			txtOpponentScore.text = Player2Score.ToString();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Update Scores
		UpdateScores();

		// Drag towards Black Hole
		transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0, 0, 0), 0.005f * Time.timeScale); 

		// Physics when not being sucked into blackhole
		if(isDying == false)
		{
			physicsControl();
			flightSound();
			checkShooting();
			lowerCooldowns();
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

	void lowerCooldowns ()
	{
		// Normal Shot
		if (primaryShootCD > 0 ) {
			primaryShootCD -= Time.timeScale;
		}

		// Secondary Shot
		if(secondaryShootCD > 0 ) {
			secondaryShootCD -= Time.timeScale;
		}
		if(secondaryShootCD < 20 && showReloadingReady == true) 
		{
			showReloadEffect();
			showReloadingReady = false;
		}

		// Special Ability
		if (specialAbilityCD > 0 ) {
			specialAbilityCD -= Time.timeScale;
		}
	}

	void AnimateDeath()
	{
		// Turn off nitro signal
		transform.root.gameObject.transform.Find("AbilityNitro").gameObject.SetActive(false);

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
			LocalDB.PlayerDead = 2;
			Player2Score = Player2Score + 1;
		}
		else
		{
			LocalDB.Player1_Score = LocalDB.Player1_Score + 1;
			LocalDB.PlayerDead = 1;
			Player1Score = Player1Score + 1;
		}
	}

	private void ImprovePowerUpShot(string shotType, bool pfx = true)
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
		print ("pfx: "+pfx);
		if(pfx == true)
		{
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
		bool isRandom = (name == "PowerUpRandom") ? true : false;
		int ranNum = (isRandom) ? Random.Range(1, 4) : 0;
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
			if(isRandom){
				if (Random.value >= 0.5){
					ImprovePowerUpShot("PowerUpBlue", false);
					ImprovePowerUpShot("PowerUpBlue", false);
				} else {
					ImprovePowerUpShot("PowerUpBlue", false);
				}
			}
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
			if(isRandom){
				if (Random.value >= 0.5){
					ImprovePowerUpShot("PowerUpYellow", false);
					ImprovePowerUpShot("PowerUpYellow", false);
				} else {
					ImprovePowerUpShot("PowerUpYellow", false);
				}
			}
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
			if(isRandom){
				if (Random.value >= 0.5){
					ImprovePowerUpShot("PowerUpPurple", false);
					ImprovePowerUpShot("PowerUpPurple", false);
				} else {
					ImprovePowerUpShot("PowerUpPurple", false);
				}
			}
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

	public void checkShooting()
	{
		//Single bullet
		if(Input.GetKeyDown(shootBullet))
		{
			if (primaryShootCD <= 0)
			{
				primaryShootCD = primaryShootCDReset;
				
				// Spil lyd
				Script_SlowMotionSound_triggered scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered>();
				scr.playSound1();
				
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

		// Use Special Ability
		if (Input.GetKeyDown(useSpecialAbility))
		{
			if(currentAbility == 1)
			{
				GameObject rdySignal = transform.root.gameObject.transform.Find("AbilityNitro").FindChild("PowerUpReady").gameObject;
				GameObject nitro = transform.root.gameObject.transform.Find("AbilityNitro").FindChild("PfxBoostNitro").gameObject;
				if(specialAbilityCD <= 0)
				{
					specialAbilityCD = specialAbility1_CDReset;
					specialAbility1_Duration = specialAbility1_DurationReset;
					
					if(rdySignal != null) rdySignal.SetActive(false);
					if(nitro != null) nitro.SetActive(true);
					
					Script_SlowMotionSound_triggered scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered> ();
					scr.playSound6(true, 0.4f);
				}
			}
		}

		if(currentAbility == 1)
		{
			if (specialAbility1_Duration > 0) {
				if (transform.root.gameObject != null)
				{
					transform.root.gameObject.GetComponent<Rigidbody>().velocity = new Vector3((atRight) ? 1	 : -1,0,0);
				}
				specialAbility1_Duration -= Time.timeScale;
			}
			if(specialAbility1_Duration <= 0)
			{
				GameObject rdySignal = transform.root.gameObject.transform.Find("AbilityNitro").FindChild("PowerUpReady").gameObject;
				GameObject nitro = transform.root.gameObject.transform.Find("AbilityNitro").FindChild("PfxBoostNitro").gameObject;
				if(nitro != null) nitro.SetActive(false);
				if(specialAbilityCD <= 0)
				{
					if(rdySignal != null) rdySignal.SetActive(true);
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
