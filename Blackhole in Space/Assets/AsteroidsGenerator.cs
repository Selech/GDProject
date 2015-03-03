using UnityEngine;
using System.Collections;

public class AsteroidsGenerator : MonoBehaviour {

	int leftCount;
	int rightCount;

	public GameObject asteroid;

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
			Instantiate(asteroid,new Vector3(-10.0f,Random.Range(-4.5f,4.5f),0),new Quaternion(0,0,90,0));
			leftCount = Random.Range (200,300);

		}

		if (rightCount == 0) {
			Instantiate(asteroid,new Vector3(10.0f,Random.Range(-4.5f,4.5f),0),new Quaternion(0,0,90,0));
			rightCount = Random.Range (200,300);

		}

	}

	void SpawnAsteroid(){



	}


}
