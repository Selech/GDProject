using UnityEngine;
using System.Collections;

public class BulletPowerupYellowMovement: MonoBehaviour {
	public float force;
	public float speed = 5; 
	private PlayerControl player;	
	public GameObject bulletCollissionExplosion;
	bool isCountingDown = false;
	int countDownNum = 0;
	public GameObject electroMineSmall;
	public GameObject electroMineMedium;

	// Use this for initialization
	void Start () {
		this.transform.GetComponent<Rigidbody>().AddForce (new Vector3(-(force * speed),0,0));
	}
	
	// Update is called once per frame
	void Update () 
	{
		checkCoolDown ();
	}

	void checkCoolDown()
	{
		if(countDownNum == 0)
		{

		}
		else if(countDownNum > 0)
		{
			countDownNum--;
		}
	}

	void initializeCountDown ()
	{
		isCountingDown = true;
		countDownNum = Mathf.RoundToInt (Random.Range (40.0f, 40.0f));
	}

	void OnCollisionEnter(Collision target)
	{
		// Going out the screen at LEFT
		if (target.gameObject.name == "Left") {
			this.transform.position = new Vector3 (9f, this.transform.position.y, 0);
			initializeCountDown();
		} 
		// Going out the screen at RIGHT
		else if (target.gameObject.name == "Right") {
			this.transform.position = new Vector3 (-9f, this.transform.position.y, 0);
			initializeCountDown();
		} 

		// Re-apply force (of some reason)
		this.transform.GetComponent<Rigidbody>().AddForce (new Vector3(-(force * speed),0,0));

		// Remove Bullet colliding with bullet
		if(target.collider.gameObject.tag == "Bullet")
		{
			Instantiate(bulletCollissionExplosion,this.transform.position,new Quaternion());
			Destroy(this.gameObject);
		}

		// Push PLAYER back
		if (target.gameObject.tag == "Player")
		{
			target.gameObject.GetComponent<Rigidbody>().AddForce (((target.gameObject.name=="Ship")?600:-600),0,0);
			Instantiate(bulletCollissionExplosion, transform.position, new Quaternion());
			Destroy(this.gameObject); // Remove bullet
		}
	}
}
