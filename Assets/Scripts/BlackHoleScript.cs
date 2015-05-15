using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlackHoleScript : MonoBehaviour {

	public GameObject playerLeft;
	public GameObject playerRight;

	public ParticleSystem pfxSuckingLeft;
	public ParticleSystem pfxSuckingRight;
	

	public int greenScoreNum = 0;
	public int redScoreNum = 0;
	public Text greenScore;
	public Text redScore;
	
	private float slowMoDist = 1.3f;
	public AudioClip slurp;
	private AudioSource audioSrc;

	bool isUltraSlowMotion = false;
	bool isDeathZooming = false;
	GameObject playerDying = null;

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
	}

	void CheckForDeathZooming ()
	{
		// Focus Camera on Ship
		if (isDeathZooming == true && playerDying != null)
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
			Vector3 playerPos = new Vector3 (playerDying.transform.position.x, playerDying.transform.position.y, -20);
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, playerPos, 0.2f * Time.timeScale);

			//ZTween.use(Camera.main.transform.gameObject).moveTo(new Vector3(playerDying.transform.position.x, playerDying.transform.position.y, -20), 0.2f, Easing.expoOut);
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
		scr.playSound1();

		// Name of collider
		string namz = other.gameObject.name;

		// Set Ship or Astoroid to dying
		if (namz == "Ship") 
		{ 
			other.gameObject.GetComponent<PlayerControl>().isDying = true;
			isUltraSlowMotion = true;
			isDeathZooming = true;
			playerDying = other.gameObject;
			Destroy(playerRight);
		}
		else if (namz == "Ship2")
		{
			other.gameObject.GetComponent<PlayerControl>().isDying = true;
			isUltraSlowMotion = true;
			isDeathZooming = true;
			playerDying = other.gameObject;
			Destroy(playerLeft);
		}
		else if(namz == "Asteroid(Clone)" || namz == "Asteroid(Clone)(Clone)") 
		{
			other.gameObject.GetComponent<AsteroidScript>().isDying = true; 
		}
		else if (namz == "Satellite(Clone)")
		{
			other.gameObject.GetComponent<SatelliteScript>().isDying = true; 
		}
		else if (namz == "PowerUpRandom(Clone)")
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
}
