using UnityEngine;
using System.Collections;

public class AsteroidScript : MonoBehaviour {

	Vector3 target;

	// Use this for initialization
	void Start () {
		target = GameObject.Find("TheBlackhole").transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//print (this.transform.position);
		//print ();
		this.transform.position = Vector3.MoveTowards (this.transform.position, target, 0.1f); 

	}
}
