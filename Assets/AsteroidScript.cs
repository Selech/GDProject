using UnityEngine;
using System.Collections;

public class AsteroidScript : MonoBehaviour {

	Vector3 target = new Vector3(0.0f,0.0f,0.0f);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//print (this.transform.position);
		//print ();
		this.transform.position = Vector3.MoveTowards (this.transform.position, target, 0.1f); 

		this.transform.Rotate (4f,4f,0);
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Player") {
			//print ("SHIIIIT");
			Camera.main.GetComponent<Animation>().Play("AsteroidsShake");
		}

	}
}
