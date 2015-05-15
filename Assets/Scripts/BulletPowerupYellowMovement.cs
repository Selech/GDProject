using UnityEngine;
using System.Collections;

public class BulletPowerupYellowMovement: MonoBehaviour 
{
	public float speed; 
	public PlayerControl playerScript;	
	public GameObject bulletCollissionExplosion;
	float countDownNum = 0;
	public GameObject electroMineSmall;
	public GameObject electroMineMedium;
	bool swappedSide;
	int mineLevel;

	// Use this for initialization
	void Start () 
	{
		// Get mine level
		mineLevel = (playerScript == null) ? 1 : playerScript.currentShotLevel;

		// Force
		bool atRight = Camera.main.WorldToScreenPoint(transform.root.gameObject.transform.position).x > Screen.width / 2;
		float sloMoMultiplier = ((atRight) ? 1 : -1) * (speed * (1 / (1-(1-Time.timeScale))));
		transform.root.gameObject.transform.GetComponent<Rigidbody>().AddForce (new Vector3(sloMoMultiplier, 0, 0));

		// Play Shoot sound
		Script_SlowMotionSound_triggered scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered>();
		if (mineLevel == 1) 		scr.playSound1();
		else if (mineLevel == 2) 	scr.playSound2();
		else if (mineLevel == 3) 	scr.playSound3();
	}
	
	// Update is called once per frame
	void Update () 
	{
		checkCoolDown ();
	}

	void SpawnElectroMine ()
	{
		if(playerScript != null)
		{
			Vector3 position = transform.root.gameObject.transform.position;
			GameObject elec = Instantiate((mineLevel == 1) ? electroMineSmall : electroMineMedium, position, new Quaternion()) as GameObject;
			elec.GetComponent<ElectroMineScript>().mineLevel = mineLevel;
			Destroy(transform.root.gameObject);
		}
	}

	void checkCoolDown()
	{
		if(swappedSide == true)
		{
			if(countDownNum < 0)
			{
				SpawnElectroMine();
			}

			if(countDownNum >= 0)
			{
				countDownNum -= Time.timeScale;
			}
		}
	}

	void initializeCountDown ()
	{
		countDownNum = Mathf.RoundToInt (Random.Range (20.0f, 80.0f));
	}

	void OnCollisionEnter(Collision target)
	{
		// Name of collided target	
		string targetName = target.gameObject.name;
		string targetTag = target.gameObject.tag;
		
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
				
				// Re-apply force (of some reason)
				bool atRight = Camera.main.WorldToScreenPoint(transform.root.gameObject.transform.position).x > Screen.width / 2;
				float sloMoMultiplier = ((atRight) ? -1 : 1) * (speed * (1 / (1-(1-Time.timeScale))));
				transform.root.gameObject.transform.GetComponent<Rigidbody>().AddForce(	new Vector3(sloMoMultiplier, 0, 0)	);
			}
		}

		// Remove Bullet colliding with bullet
		if(targetTag == "Bullet" || targetTag == "Player")
		{
			SpawnElectroMine();
			Destroy(transform.root.gameObject);
		}
	}
}
