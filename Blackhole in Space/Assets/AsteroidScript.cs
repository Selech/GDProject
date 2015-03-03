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

	}
}
