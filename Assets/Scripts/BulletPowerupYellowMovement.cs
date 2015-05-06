using UnityEngine;
using System.Collections;

public class BulletPowerupYellowMovement: MonoBehaviour 
{
	public float force;
	public float speed = 5; 
	public PlayerControl playerScript;	
	public GameObject bulletCollissionExplosion;
	bool isCountingDown = false;
	int countDownNum = 0;
	public GameObject electroMineSmall;
	public GameObject electroMineMedium;

	// Use this for initialization
	void Start () 
	{
		bool atRight = Camera.main.WorldToScreenPoint(transform.root.gameObject.transform.position).x > Screen.width / 2;
		this.transform.GetComponent<Rigidbody>().AddForce (new Vector3(((atRight) ? 1 : -1) * (force * speed),0,0));
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
			int mineLevel = playerScript.currentShotLevel;
			GameObject elec = Instantiate((mineLevel == 1) ? electroMineSmall : electroMineMedium, position, new Quaternion()) as GameObject;
			elec.GetComponent<ElectroMineScript>().mineLevel = mineLevel;
			Destroy(transform.root.gameObject);
		}
	}

	void checkCoolDown()
	{
		if(isCountingDown)
		{
			if(countDownNum == 0)
			{
				SpawnElectroMine();
			}

			if(countDownNum >= 0)
			{
				countDownNum--;
			}
		}
	}

	void initializeCountDown ()
	{
		isCountingDown = true;
		countDownNum = Mathf.RoundToInt (Random.Range (20.0f, 80.0f));
	}

	void OnCollisionEnter(Collision target)
	{
		if(swappedSide == false)
		{
			// Going out the screen at LEFT
			if (target.gameObject.name == "Left") {
				swappedSide = true;
				this.transform.position = new Vector3 (9f, this.transform.position.y, 0);
				initializeCountDown();
			} 
			// Going out the screen at RIGHT
			else if (target.gameObject.name == "Right") {
				swappedSide = true;
				this.transform.position = new Vector3 (-9f, this.transform.position.y, 0);
				initializeCountDown();
			} 
		}
		else if (target.gameObject.name != "Right" && target.gameObject.name != "Left")
		{
			// Remove Bullet colliding with bullet
			if(target.collider.gameObject.tag == "Bullet")
			{
				Instantiate(bulletCollissionExplosion,this.transform.position,new Quaternion());
			}
			
			// Push PLAYER back
			if (target.gameObject.tag == "Player")
			{
				target.gameObject.GetComponent<Rigidbody>().AddForce (((target.gameObject.name=="Ship")?600:-600),0,0);
				Instantiate(bulletCollissionExplosion, transform.position, new Quaternion());
			}

			// Remove this yellow powerup bullet
			Destroy(this.gameObject);
		}

		// Re-apply force (of some reason)
		bool atRight = Camera.main.WorldToScreenPoint(transform.root.gameObject.transform.position).x > Screen.width / 2;
		this.transform.GetComponent<Rigidbody>().AddForce(	new Vector3(	(	((atRight) ? -1 : 1) * (force * speed)), 0, 0)	);
	}

	bool swappedSide;
}
