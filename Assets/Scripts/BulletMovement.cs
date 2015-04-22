using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {
	public float force;
	public float speed = 5; 
	private PlayerControl player;	
	public GameObject bulletCollissionExplosion;

	// Use this for initialization
	void Start () {
		this.transform.GetComponent<Rigidbody>().AddForce (new Vector3(-(force * speed),0,0));
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnCollisionEnter(Collision target)
	{
		if (target.gameObject.name == "Left") {
			this.transform.position = new Vector3 (9f, this.transform.position.y, 0);
		} 
		if (target.gameObject.name == "Right") {
			this.transform.position = new Vector3 (-9f, this.transform.position.y, 0);
		} 
		this.transform.GetComponent<Rigidbody>().AddForce (new Vector3(-(force * speed * 2),0,0));

		if(target.collider.gameObject.tag == "Bullet")
		{
			Instantiate(bulletCollissionExplosion,this.transform.position,new Quaternion());
			
			Destroy(this.gameObject);
		}
	}
}
