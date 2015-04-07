using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {
	public float force;
	private PlayerControl player;	

	// Use this for initialization
	void Start () {
		//this.transform.rigidbody.AddForce (new Vector3 (-force, 0f, 0f));
	}
	
	// Update is called once per frame
	void Update () {
			player = GameObject.Find("Ship").GetComponent<PlayerControl>();

			Vector3 direction = player.transform.position - player.target.transform.position;

			this.transform.GetComponent<Rigidbody>().AddForce (-direction * force);
	}

	void OnCollisionEnter(Collision target){
		if (target.gameObject.name == "Left") {
			print ("hit");
			this.transform.position = new Vector3 (18, this.transform.position.y, 0);
		} 
		if (target.gameObject.name == "Right") {
			this.transform.position = new Vector3 (-18, this.transform.position.y, 0);
		} 
	}
}
