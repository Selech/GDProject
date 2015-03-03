using UnityEngine;
using System.Collections;

public class AsteroidsGenerator : MonoBehaviour {

	int leftCount;

	public GameObject asteroid;

	// Use this for initialization
	void Start () {
		leftCount = Random.Range (200,300);

	}
	
	// Update is called once per frame
	void Update () {
		leftCount--;

		if (leftCount == 0) {
			Instantiate(asteroid,new Vector3(-10.0f,Random.Range(-4.5f,4.5f),0),new Quaternion());
			leftCount = Random.Range (200,300);

		}


	}

	void SpawnAsteroid(){



	}


}
