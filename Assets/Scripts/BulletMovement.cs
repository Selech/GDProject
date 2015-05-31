using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {
	public float force;
	public float speed; 
	private PlayerControl player;	
	public GameObject bulletCollissionExplosion;
	public GameObject sfxHit;


	// Use this for initialization
	void Start () 
	{
		// Wuut... virkede kun hvis den var her to gange i træk :S
		transform.root.gameObject.GetComponent<Rigidbody>().AddForce(	-(force * (speed * (1 / (1-(1-Time.timeScale))))),0 , 0);

		// Spil lyd
		Script_SlowMotionSound_triggered scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered>();
		scr.playSound1();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnCollisionEnter(Collision target)
	{
		// Going out the screen at LEFT
		if (target.gameObject.name == "Left") {
			this.transform.position = new Vector3 (9f, this.transform.position.y, 0);
		} 
		// Going out the screen at RIGHT
		else if (target.gameObject.name == "Right") {
			this.transform.position = new Vector3 (-9f, this.transform.position.y, 0);
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
			Script_SlowMotionSound_triggered scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered>();
			scr.playSound2(true);

			target.gameObject.GetComponent<Rigidbody>().AddForce (((target.gameObject.name=="Ship")?600:-600),0,0);
			Instantiate(bulletCollissionExplosion, transform.position, new Quaternion());
			Destroy(this.gameObject); // Remove bullet
		}
	}
}
