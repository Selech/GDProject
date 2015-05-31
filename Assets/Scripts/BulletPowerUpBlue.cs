using UnityEngine;
using System.Collections;

public class BulletPowerUpBlue: MonoBehaviour 
{
	public float speed; 
	public float seekSpeed;
	public float seekInterval;
	float seekIntervalCD = -1;
	bool swappedSide;
	int pushOpponent;
	int pushRecoil;
	int shotLevel;
	PlayerControl pControl;

	public GameObject bulletCollissionExplosion;
	bool isCountingDown = false;
	float countDownNum = 0;
	public GameObject blast_impact_hit;
	public GameObject blast_no_hit;
	public GameObject bulletPowerUpBlue;
	public ParticleSystem trailA;
	public ParticleSystem trailB;
	bool atRight;
	bool showDefaultBlast = true;
	public bool replica = false;
	float shootAngle;

	GameObject ship;
	GameObject ship2;

	void Start()
	{
		// Player references
		ship = GameObject.Find ("Ship");
		ship2 = GameObject.Find ("Ship2");
	}

	public int getShootCooldown (int currentShotLevel)
	{
		if (currentShotLevel == 2) return 45;
		else if (currentShotLevel == 3) return 30;
		return 60;
	}

	// Use this for initialization
	public void SpawnBullets (int shotLevel, PlayerControl pControl) 
	{
		// Save shot level
		this.shotLevel = shotLevel;
		this.pControl = pControl;
		float interval = 0.10f;

		// Shot level 1
		shootAngle = 5;
		pushRecoil = 200;
		pushOpponent = 250;
		if(replica == false)
		{
			Invoke("DoAttack", interval * 1); 
			Invoke("DoAttack", interval * 2); 
		}

		// Shot level 2
		if (shotLevel == 2)
		{
			if(replica == false)
			{
				Invoke("DoAttack", interval * 3);
				Invoke("DoAttack", interval * 4);
			}
			shootAngle = 10;
			pushRecoil = 75;
			pushOpponent = 200;
		}
		// Shot level 3
		else if (shotLevel == 3)
		{
			if(replica == false)
			{
				Invoke("DoAttack", interval * 3);
				Invoke("DoAttack", interval * 4);
				Invoke("DoAttack", interval * 5); 
				Invoke("DoAttack", interval * 6); 
			}
			shootAngle = 20;
			pushRecoil = 50;
			pushOpponent = 150;
		}
		
		// Position
		atRight = Camera.main.WorldToScreenPoint (pControl.transform.root.gameObject.transform.position).x > Screen.width / 2;
		Vector3 shipPos = pControl.transform.root.gameObject.transform.position;
		Vector3 pos = new Vector3(((atRight) ? shipPos.x + 0.5f : shipPos.x - 0.5f), shipPos.y, shipPos.z);
		transform.position = pos;

		// Force + Angle
		float sloMoMultiplier = ((atRight) ? 1 : -1) * (speed * (1 / (1-(1-Time.timeScale))));
		float yRange = Random.Range(-shootAngle, shootAngle);
		transform.root.gameObject.transform.GetComponent<Rigidbody>().AddForce (new Vector3(sloMoMultiplier, yRange, 0));
	}

	public void DoAttack()
	{
		if(pControl != null)
		{
			// Recoil
			pControl.transform.root.gameObject.GetComponent<Rigidbody>().AddForce (new Vector3(((atRight) ? -1 : 1) * (pushRecoil * (1 / (1-(1-Time.timeScale)))),0,0));
			
			// New Blue Bullet
			GameObject gObj = Instantiate(bulletPowerUpBlue, transform.position, new Quaternion()) as GameObject;
			gObj.GetComponent<BulletPowerUpBlue>().replica = true;
			gObj.GetComponent<BulletPowerUpBlue>().SpawnBullets(shotLevel, pControl);
		}
	}


	// Update is called once per frame
	void Update () 
	{
		// Move Behavior
		if(swappedSide == true)
		{
//			print ("no");
			// Seek
			if(ship != null && ship2 != null)
			{
				// Face Opponent each time the seekinterval is zero again
				seekIntervalCD -= Time.timeScale;
				if (seekIntervalCD < 0)
				{
					// Reset Cooldown
					seekIntervalCD = seekInterval;

					// Seek that piece of opponent shit
					Vector3 opoPos = ((atRight) ? ship : ship2).transform.position;
					transform.LookAt(opoPos);
				}
				transform.Translate(Vector3.forward*speed/5f*Time.deltaTime);
			}
		}

		checkCoolDown ();
	}

	void Explode()
	{
		if (showDefaultBlast)
		{
			trailA.loop = false;
			trailA.transform.parent=null; // detach particle system
			
			trailB.loop = false; 
			trailB.transform.parent=null; // detach particle system
			
			GameObject gObj = Instantiate(blast_no_hit, transform.position, new Quaternion()) as GameObject;
			gObj.transform.parent = null;

			Destroy(this.transform.root.gameObject);
		}
	}
	
	void checkCoolDown()
	{
		if(isCountingDown == true)
		{
			countDownNum -= Time.timeScale;

			if(countDownNum < 0)
			{
				isCountingDown = false;
				Explode();
			}
		}
	}
	
	void initializeCountDown ()
	{
		isCountingDown = true;
		countDownNum = Mathf.RoundToInt (Random.Range (100.0f, 250.0f));
	}
	
	void OnCollisionEnter(Collision target)
	{
		// Name of collided target	
		string targetName = target.gameObject.name;

		if(swappedSide == false)
		{
			// Going out the screen at LEFT
			if(targetName == "Left" || targetName == "Right")
			{
				// Position at left edge or right depending on where it left the screen
				if (targetName == "Left") 
				{
					swappedSide = true;
					this.transform.position = new Vector3 (9f, this.transform.position.y, 0);
					initializeCountDown();
				} 
				// Going out the screen at RIGHT
				else if (targetName == "Right")
				{
					swappedSide = true;
					this.transform.position = new Vector3 (-9f, this.transform.position.y, 0);
					initializeCountDown();
				} 
			}
		}
		else if (target.gameObject.tag == "Player")
		{
			// Blast1 Pfx
			showDefaultBlast = false;
			GameObject gObj = Instantiate(blast_impact_hit, transform.position, new Quaternion(0,0,1,0)) as GameObject;
			gObj.transform.LookAt(target.gameObject.transform.position);
			gObj.transform.Find("pfxImpactBlastB").transform.Translate(Vector3.forward * 0.7f);

			// Push back
			float pushBack = pushOpponent * (1 / (1-(1-Time.timeScale)));
			target.gameObject.GetComponent<Rigidbody>().AddForce (((targetName=="Ship")?pushBack:-pushBack),0,0);

			// Remove
			Destroy(transform.root.gameObject);
		}
	}
}
