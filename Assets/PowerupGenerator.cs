using UnityEngine;
using System.Collections;

public class PowerupGenerator : MonoBehaviour {

	int leftCount;
	int rightCount;

	public GameObject random;

	// Use this for initialization
	void Start () {
		leftCount = Random.Range (200,300);
		rightCount = Random.Range (200,300);
	}
	
	// Update is called once per frame
	void Update () {
	
		leftCount--;
		rightCount--;
		
		if (leftCount == 0) {
			Instantiate(random,new Vector3(-18.0f,Random.Range(-4.5f,4.5f),0),new Quaternion(0,0.7f,-0.7f,0));
			leftCount = Random.Range (200,300);
			
		}
		
		if (rightCount == 0) {
			Instantiate(random,new Vector3(18.0f,Random.Range(-4.5f,4.5f),0),new Quaternion(0,0.7f,-0.7f,0));
			rightCount = Random.Range (200,300);
			
		}

	}
}
