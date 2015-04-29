using UnityEngine;
using System.Collections;

public class ObstacleGenerator : MonoBehaviour {
	
	int leftCount;
	int rightCount;
	
	int lowerRandomNum = 40;
	int upperRandomNum = 200;
	
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
			SpawnObstacle(10.0f);
			leftCount = Random.Range (lowerRandomNum, upperRandomNum);
		}

		if (rightCount == 0) 
		{
			SpawnObstacle(-10.0f);
			rightCount = Random.Range (lowerRandomNum, upperRandomNum);
		}
	}
	
	void SpawnObstacle(float xPos)
	{
		int spawnNum = Random.Range(1,4);
		GameObject obstacle = null;

		if (spawnNum == 1)
		{
			obstacle = (GameObject) Instantiate(asteroid,new Vector3(xPos,Random.Range(-4.5f,4.5f),0),new Quaternion(0,0,0.7f,0.7f));
			obstacle.GetComponentInChildren<AsteroidScript>().scale = new Vector3(0.5f,0.5f,0.5f);
		}
		else if (spawnNum == 2)
		{
			obstacle = (GameObject) Instantiate(toilet,new Vector3(xPos,Random.Range(-4.5f,4.5f),0),new Quaternion(0,0,0.7f,0.7f));
		}
		else if (spawnNum == 3)
		{
			obstacle = (GameObject) Instantiate(satellite,new Vector3(xPos,Random.Range(-4.5f,4.5f),0), new Quaternion());
		}
	}
}
