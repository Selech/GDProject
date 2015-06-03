using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlackHoleScript : MonoBehaviour {

	public GameObject playerLeft;
	public GameObject playerRight;
	public GameObject VictoryScreen;

	public ParticleSystem pfxSuckingLeft;
	public ParticleSystem pfxSuckingRight;
	public GameObject deathExplosion;
	public GameObject instanceOfDeathExplosion;

	public int greenScoreNum = 0;
	public int redScoreNum = 0;
	public Text greenScore;
	public Text redScore;
	
	private float slowMoDist = 1.3f;
	private AudioSource audioSrc;

	bool isUltraSlowMotion = false;
	bool isDeathZooming = false;
	GameObject loserToZoomTo = null;
	Vector3 loserPosition;
	int zoomTime = 90;

	float originalOrthographicZoom = 5;
	float limitOrthoGraphicZoom = 2;
	float zoomSpeed = 1.2f;
	float currentDifIn;
	float currentDifOut;

	// Use this for initialization
	void Start () 
	{
		// Set zoom difference
		currentDifIn = currentDifOut = 5;

		// Save reference to Flight Sound Audio Source
		audioSrc = GetComponent<AudioSource>();
		audioSrc.volume = 0.2f;

		// Dont suck at start..
		pfxSuckingLeft.enableEmission = false;
		pfxSuckingRight.enableEmission = false;

		greenScore.text = "0";
		redScore.text = "0";
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckForSlowmotion();
		CheckForDeathZooming();
		CheckForDeathExplosion();
	}

	void CheckForDeathExplosion ()
	{
		if(instanceOfDeathExplosion !=null)
		{
			if(zoomTime>0)
			{
				if(instanceOfDeathExplosion.GetComponent<ExplosionMat>()._alpha < 1) instanceOfDeathExplosion.GetComponent<ExplosionMat>()._alpha += 0.05f;
				Vector3.Lerp(instanceOfDeathExplosion.transform.localScale, new Vector3(6,6,6),  0.5f * Time.timeScale);
			}
			else
			{
				if(instanceOfDeathExplosion.GetComponent<ExplosionMat>()._alpha > 0) instanceOfDeathExplosion.GetComponent<ExplosionMat>()._alpha -= 0.1f;
				Vector3.Lerp(instanceOfDeathExplosion.transform.localScale, new Vector3(0,0,0),  0.5f * Time.timeScale);
			}
		}
	}

	void CheckForDeathZooming ()
	{
		// Focus Camera on Ship
		if (isDeathZooming == true && loserPosition != null)
		{
			// Focus Camera on Ship
			if(Camera.main.orthographicSize > limitOrthoGraphicZoom)
			{
				currentDifIn = currentDifIn / zoomSpeed; 
				Camera.main.orthographicSize -= currentDifIn * Time.timeScale;
			}
			else
			{
				Camera.main.orthographicSize = limitOrthoGraphicZoom;
			}

			// Zoom to correct X and Y
			Vector3 playerPos = new Vector3 (loserPosition.x, loserPosition.y, -20);
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, playerPos, 0.5f * Time.timeScale);

			if(zoomTime > 0){zoomTime--;}
			else
			{
				isDeathZooming = false;
				showWinScreen();
			}
		}
		else
		{
			// Focus Camera on Ship
			if(Camera.main.orthographicSize < originalOrthographicZoom)
			{
				currentDifOut = currentDifOut / zoomSpeed; 
				Camera.main.orthographicSize += currentDifOut * Time.timeScale;
			}
			else
			{
				Camera.main.orthographicSize = originalOrthographicZoom;
			}

			// Zoom to correct X and Y
			Vector3 pos = new Vector3 (0, 0, -20);
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, pos, 0.2f * Time.timeScale);
		}
	}

	void showWinScreen ()
	{
		VictoryScreen.SetActive(true);

		if(loserToZoomToName == "Ship")
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

	/**
	 * When a player is near this blackhole everything slows down.
	 **/
	void CheckForSlowmotion()
	{
		// Ulta slowmotion (Set if player is dead)
		if(isUltraSlowMotion)
		{
			Time.timeScale = 0.1F;
		}

		// Slowmotion if close to black hole
		if(playerLeft != null && playerRight != null)
		{
			if(playerLeft.activeSelf && playerRight.activeSelf)
			{
				ZTween.use(transform.root.gameObject).scaleTo(new Vector3(1f, 10f, 0.05f), 0.01f, Easing.elasticIn);

				bool isSuckedOnLeft = (playerLeft.transform.position.x - this.transform.position.x > -slowMoDist);
				bool isSuckedOnRight = (playerRight.transform.position.x - this.transform.position.x < slowMoDist);
				
				if (isSuckedOnLeft || isSuckedOnRight) 
				{
					// Slow down time
					Time.timeScale = 0.3F;

					// Show pfx if 
					if(isSuckedOnLeft)
					{
						Vector3 pos = pfxSuckingLeft.transform.position;
						pos.y = playerLeft.transform.position.y;
						pfxSuckingLeft.transform.position = pos;
						pfxSuckingLeft.enableEmission = true;
					}
					if(isSuckedOnRight) 
					{
						Vector3 pos = pfxSuckingRight.transform.position;
						pos.y = playerRight.transform.position.y;
						pfxSuckingRight.transform.position = pos;
						pfxSuckingRight.enableEmission = true;
					}
					// Play Vacuum Cleaner sound
					VacuumCleanerSound(true);

					// Scales an object
					ZTween.use(transform.root.gameObject).scaleTo(new Vector3(1f, 10f, 0.25f), 0.2f, Easing.elasticOut);
				} 
				else 
				{
					if(isSuckedOnLeft == false) pfxSuckingLeft.enableEmission = false;
					if(isSuckedOnRight == false) pfxSuckingRight.enableEmission = false;
					
					// Stop Vacuum Cleaner sound
					VacuumCleanerSound(false);

					Time.timeScale = 1.0F;
				}
				Time.fixedDeltaTime = 0.02F * Time.timeScale;
			}
		}
	}

	public void VacuumCleanerSound(bool start)
	{
		if (start == false) 
		{
			audioSrc.time = 0;
			audioSrc.Stop();
		}
		else if (audioSrc.time == 0)
		{
			audioSrc.Play();
		}
	}

	void OnCollisionEnter(Collision other)
	{
		// Play Shoot sound
		Script_SlowMotionSound_triggered scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered>();
		scr.playSound1(false, 0.3f);

		// Name of collider
		string namz = other.gameObject.name;

		// Set Ship or Astoroid to dying
		if (namz == "Ship") 
		{ 
			other.gameObject.GetComponent<PlayerControl>().isDying = true;
			doTheEnd(other.gameObject, other.gameObject.transform.position);
		}
		else if (namz == "Ship2")
		{
			other.gameObject.GetComponent<PlayerControl>().isDying = true;
			doTheEnd(other.gameObject, other.gameObject.transform.position);
		}
		else if(namz == "Asteroid(Clone)" || namz == "Asteroid(Clone)(Clone)") 
		{
			other.gameObject.GetComponent<AsteroidScript>().isDying = true; 
		}
		else if (namz == "Satellite(Clone)")
		{
			other.gameObject.GetComponent<SatelliteScript>().isDying = true; 
		}
		else if (namz == "PowerUpRandom(Clone)" || namz == "PowerUpYellow(Clone)" || namz == "PowerUpBlue(Clone)" || namz == "PowerUpPurple(Clone)")
		{
			other.gameObject.GetComponent<PowerUpRandomScript>().isDying = true; 
		}
		else if (namz == "Toilet(Clone)")
		{
			other.gameObject.GetComponent<ToiletScript>().isDying = true; 
		}
		else
		{
			Destroy(other.gameObject);
		}
	}

	string loserToZoomToName;

	public void doTheEnd(GameObject loser, Vector3 loserPos)
	{
		isUltraSlowMotion = true;
		isDeathZooming = true;
		loserToZoomTo = loser;
		loserToZoomToName = loser.name;
		loserPosition = loserPos;
		Destroy((loser.name=="Ship") ? playerRight : playerLeft);
	}

	public void deathByLasor(GameObject loser)
	{
		Vector3 vec = new Vector3 (loser.transform.position.x, loser.transform.position.y, loser.transform.position.z + 2);
		instanceOfDeathExplosion = Instantiate(deathExplosion, vec, new Quaternion()) as GameObject;
		instanceOfDeathExplosion.transform.localScale = new Vector3(4f, 4f, 4f);
	}
}
