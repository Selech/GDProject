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
	public AudioClip shot;
	public bool isDying = false;
	public bool isBeingSuckedIntoBlackHole = false;

	public int numCollectedPowerups	= 0;
	public int numCollectedBluePowerups = 0;
	public int numCollectedYellowPowerups = 0;
	public int numCollectedPurplePowerups = 0;

	public Text txtCollectedPowerups;
	public Text txtCollectedBluePowerups;	
	public Text txtCollectedYellowPowerups;
	public Text txtCollectedPurplePowerups;
	public Text txtOpponentScore;

	public GameObject assJetWhite;
	public GameObject assJetBlue;
	public GameObject assJetYellow;
	public GameObject assJetPurple;

	public GameObject bulletPowerUpYellow;
	public GameObject bulletPowerUpBlue;
	public GameObject bulletPowerUpPurple;
	
	public GameObject absorbEffectBlue;
	public GameObject absorbEffectYellow;
	public GameObject absorbEffectPurple;
	public AudioClip absorbEffectBlue_mp3;
	public AudioClip absorbEffectYellow_mp3;
	public AudioClip absorbEffectPurple_mp3;

	private float rotationSpeed = 2.0f;
	public float paralyzeTime = 0;
	public ParticleSystem PfxParalysed;

	// Use this for initialization
	void Start () 
	{
		// Save reference to Flight Sound Audio Source
		audioSrc = GetComponent<AudioSource>();
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

		}
		else
		{
			AnimateDeath();
		}
	}

	void AnimateDeath()
	{
		float scaleSpeed = 0.02f;
		ship.transform.localScale -= new Vector3((scaleSpeed*1.2f) * Time.timeScale, (scaleSpeed*2) * Time.timeScale, (scaleSpeed) * Time.timeScale);

		if(ship.transform.localScale.x < 0.0f)
		{
			Destroy(GameObject.Find(ship.transform.parent.gameObject.name));
			VictoryScreen.SetActive(true);

			if(ship.transform.parent.gameObject.name == "Ship")
			{
				VictoryScreen.transform.Find("pfxRed").gameObject.SetActive(true);
				VictoryScreen.transform.Find("Image (RED in MIddle)").gameObject.SetActive(true);
				VictoryScreen.transform.Find("Text (Red is Victorious)").gameObject.SetActive(true);
			}
			else
			{
				VictoryScreen.transform.Find("pfxGreen").gameObject.SetActive(true);
				VictoryScreen.transform.Find("Image (Green in MIddle)").gameObject.SetActive(true);
				VictoryScreen.transform.Find("Text (Green is Victorious)").gameObject.SetActive(true);
			}
		}
	}

	public void flightSound()
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
	}

	private void CollisionWithBlackHole(Collision other, string nameOfCollider)
	{
		if(nameOfCollider == "TheBlackhole")
		{
			if(isBeingSuckedIntoBlackHole == false)
			{
				isBeingSuckedIntoBlackHole = true;

				if (this.name.Replace("(Clone)", "") == "Ship")
				{
					LocalDB.Player2_Score = LocalDB.Player2_Score + 1;
				}
				else
				{
					LocalDB.Player1_Score = LocalDB.Player1_Score + 1;
				}

				UpdateScores();
			}
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
	}

	private void CollisionWithPowerUp(Collision other, string name)
	{
		int ranNum = (name == "PowerUpRandom") ? Random.Range(1, 4) : 0;

		if (name == "PowerUpBlue" || ranNum == 1)
		{
			hideJetasses();
			assJetBlue.SetActive(true);
			Instantiate(absorbEffectBlue, transform.position, new Quaternion());
			AudioSource.PlayClipAtPoint (absorbEffectBlue_mp3, GameObject.Find("Main Camera").GetComponent<Transform>().position);
			ImprovePowerUpShot("PowerUpBlue");
		}
		else if (name == "PowerUpYellow" || ranNum == 2)
		{
			hideJetasses();
			assJetYellow.SetActive(true);
			Instantiate(absorbEffectYellow, transform.position, new Quaternion());
			AudioSource.PlayClipAtPoint (absorbEffectYellow_mp3, GameObject.Find("Main Camera").GetComponent<Transform>().position);
			ImprovePowerUpShot("PowerUpYellow");
		}
		else if (name == "PowerUpPurple" || ranNum == 3)
		{
			hideJetasses();
			assJetPurple.SetActive(true);
			Instantiate(absorbEffectPurple, transform.position, new Quaternion());
			AudioSource.PlayClipAtPoint (absorbEffectPurple_mp3, GameObject.Find("Main Camera").GetComponent<Transform>().position);
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
			// Shot Sound
			AudioSource.PlayClipAtPoint (shot, GameObject.Find("Main Camera").GetComponent<Transform>().position);
			
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
		
		// Secondary Bullet
		if(Input.GetKeyDown(shootSecondaryBullet))
		{
			if (currentShotType == "PowerUpBlue")
			{
				// Bullet on the right side of the ship
				GameObject gObj = Instantiate(bulletPowerUpBlue, transform.position, new Quaternion()) as GameObject;
				gObj.GetComponent<BulletPowerUpBlue>().SpawnBullets(currentShotLevel, this);
			}
			else if (currentShotType == "PowerUpYellow")
			{
				// Bullet on the right side of the ship
				Vector3 position = ship.transform.position - (new Vector3((float)ship.transform.rotation.y, (ship.transform.position.y - spawnPointFront.transform.position.y) * 0.3f, 0f));
				Instantiate(bulletPowerUpYellow, position, new Quaternion());
				bulletPowerUpYellow.GetComponent<BulletPowerupYellowMovement>().playerScript = this;
				
				// NuzzleFire
				ShowNuzzleFireParticles(NuzzleFirePowerUpYellow);
			}
			else if (currentShotType == "PowerUpPurple")
			{
				// NuzzleFire
				GameObject gObj = Instantiate(bulletPowerUpPurple, transform.root.gameObject.transform.position, new Quaternion()) as GameObject;
				gObj.GetComponent<BulletPowerUpPurple>().playerScript = this;
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
		psLeft.Clear();
		psLeft.Play();
		
		// Right Nuzzle Fire
		ParticleSystem psRight = gObj.transform.FindChild("right").GetComponent<ParticleSystem>();
		psRight.Clear();
		psRight.Play();
	}
}
