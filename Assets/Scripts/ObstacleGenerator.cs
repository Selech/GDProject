using UnityEngine;
using System.Collections;

public class ObstacleGenerator : MonoBehaviour {

	public static bool isOpenened;
	public static bool isRoundWon;

	float leftCount;
	float rightCount;
	
	float lowerRandomNum = 60;
	float upperRandomNum = 90;
	
	float ySpawnDistFromMid = 4.5f;
	float xSpawnDistFromMid = 11.0f;
	
	public GameObject playerLeft;
	public GameObject playerRight;

	public GameObject asteroid;
	public GameObject toilet;
	public GameObject satellite;
	public GameObject powerUpBlue;
	public GameObject powerUpPurple;
	public GameObject powerUpYellow;
	
	public GameObject pfxPointsGivenGreen;
	public GameObject pfxPointsGivenRed;
	
	// Use this for initialization
	void Start () 
	{
		isRoundWon = false;
		leftCount = Random.Range (lowerRandomNum, upperRandomNum);
		rightCount = Random.Range (lowerRandomNum, upperRandomNum);

		// Show Points increasing PFX 
		if(LocalDB.PlayerDead != 0)
		{
			// Show it
			GameObject pfx = (LocalDB.PlayerDead == 1) ? pfxPointsGivenGreen : pfxPointsGivenRed;
			pfx.SetActive(true);
			
			// Position it
			float xPosition = 8.55f;
			float yPosition = 4.45f;
			if(LocalDB.PlayerDead == 1)
			{
				pfx.transform.position = new Vector3(-xPosition, yPosition, pfxPointsGivenRed.transform.position.z);
			}
			else
			{
				pfx.transform.position = new Vector3(xPosition, yPosition, pfxPointsGivenRed.transform.position.z);
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(playerLeft != null && playerRight != null)
		{
			leftCount -= Time.timeScale;
			rightCount -= Time.timeScale;
			
			if (leftCount < 0)
			{
				SpawnObstacle(xSpawnDistFromMid);
				leftCount = Random.Range (lowerRandomNum, upperRandomNum);
			}
			
			if (rightCount < 0) 
			{
				SpawnObstacle(-xSpawnDistFromMid);
				rightCount = Random.Range (lowerRandomNum, upperRandomNum);
			}
		}
		else
		{
			isRoundWon = true;
		}
	}
	
	void SpawnObstacle(float xPos)
	{
		GameObject obstacle = null;

		if (Random.value > 0.5f) 
		{
			int randomNum = Random.Range(1,4);
			GameObject powerUp = (randomNum == 1) ? powerUpYellow : (randomNum == 2) ? powerUpBlue : powerUpPurple; //powerUpPurple; // (randomNum == 1) ? powerUpYellow : (randomNum == 2) ? powerUpBlue : powerUpPurple; 
			obstacle = (GameObject)Instantiate (powerUp, new Vector3 (xPos, Random.Range (-ySpawnDistFromMid, ySpawnDistFromMid), 0), new Quaternion ());
		}

		if (Random.value > 0.87f) 
		{
			obstacle = (GameObject)Instantiate (satellite, new Vector3 (xPos, Random.Range (-ySpawnDistFromMid, ySpawnDistFromMid), 0), new Quaternion ());
		} 


		if (Random.value > 0.5f)
		{
			obstacle = (GameObject)Instantiate (toilet, new Vector3 (xPos, Random.Range (-ySpawnDistFromMid, ySpawnDistFromMid), 0), new Quaternion ());
			//obstacle.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
		}
		else if (Random.value > 0.5f)
		{
			obstacle = (GameObject)Instantiate (asteroid, new Vector3 (xPos, Random.Range (-ySpawnDistFromMid, ySpawnDistFromMid), 0), new Quaternion (0, 0, 0.7f, 0.7f));
			obstacle.GetComponentInChildren<AsteroidScript> ().scale = new Vector3 (0.5f, 0.5f, 0.5f);
		}
	}
}
