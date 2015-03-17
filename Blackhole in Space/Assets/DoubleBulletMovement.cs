using UnityEngine;
using System.Collections;

public class DoubleBulletMovement : MonoBehaviour {
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
		
		
		this.transform.rigidbody.AddForce (-direction * force);
		
		
		if (this.transform.position.x < -9) 
		{
			float yPos = rigidbody.transform.position.y;
			this.transform.position = new Vector3 (8, yPos, 0);
		}
	}
	
	void OnCollisionEnter(Collision target){
		if(target.gameObject.tag != "Player") Destroy (this.gameObject);
	}
}
