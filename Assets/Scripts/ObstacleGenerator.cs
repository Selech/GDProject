using UnityEngine;
using System.Collections;

public class ObstacleGenerator : MonoBehaviour {
	
	int leftCount;
	int rightCount;
	
	int lowerRandomNum = 60;
	int upperRandomNum = 90;
	
	float ySpawnDistFromMid = 4.5f;
	float xSpawnDistFromMid = 9.0f;
	
	public GameObject asteroid;
	public GameObject toilet;
	public GameObject satellite;
	
	// Use this for initialization
	void Start () 
	{
		leftCount = Random.Range (lowerRandomNum, upperRandomNum);
		rightCount = Random.Range (lowerRandomNum, upperRandomNum);
	}
	
	// Update is called once per frame
	void Update () 
	{
		leftCount--;
		rightCount--;
		
		if (leftCount == 0)
		{
			SpawnObstacle(xSpawnDistFromMid);
			leftCount = Random.Range (lowerRandomNum, upperRandomNum);
		}

		if (rightCount == 0) 
		{
			SpawnObstacle(-xSpawnDistFromMid);
			rightCount = Random.Range (lowerRandomNum, upperRandomNum);
		}
	}
	
	void SpawnObstacle(float xPos)
	{
		GameObject obstacle = null;

		if (Random.value > 0.5) 
		{
			obstacle = (GameObject)Instantiate (satellite, new Vector3 (xPos, Random.Range (-ySpawnDistFromMid, ySpawnDistFromMid), 0), new Quaternion ());
		}  
		else if (Random.value > 0.5) 
		{
			obstacle = (GameObject)Instantiate (asteroid, new Vector3 (xPos, Random.Range (-ySpawnDistFromMid, ySpawnDistFromMid), 0), new Quaternion (0, 0, 0.7f, 0.7f));
			obstacle.GetComponentInChildren<AsteroidScript> ().scale = new Vector3 (0.5f, 0.5f, 0.5f);
		}
		else
		{
			obstacle = (GameObject)Instantiate (toilet, new Vector3 (xPos, Random.Range (-ySpawnDistFromMid, ySpawnDistFromMid), 0), new Quaternion ());
		}
	}
}
