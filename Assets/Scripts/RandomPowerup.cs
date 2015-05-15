using UnityEngine;
using System.Collections;

public class RandomPowerup : MonoBehaviour {

	private bool picked = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!picked){
			transform.position = Vector3.MoveTowards (this.transform.position, new Vector3(0,0,0), 0.1f);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		picked = true;
	}
}
