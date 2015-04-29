using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlackHoleScript : MonoBehaviour {

	public GameObject playerLeft;
	public GameObject playerRight;

	public int greenScoreNum = 0;
	public int redScoreNum = 0;
	public Text greenScore;
	public Text redScore;

	public AudioClip slurp;

	// Use this for initialization
	void Start () 
	{
		greenScore.text = "0";
		redScore.text = "0";
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckForSlowmotion();
	}

	/**
	 * When a player is near this blackhole everything slows down.
	 **/
	void CheckForSlowmotion()
	{
		if(playerLeft != null && playerRight != null)
		{
			if(playerLeft.activeSelf && playerRight.activeSelf)
			{
				if ((playerLeft.transform.position.x - this.transform.position.x > -1.0f) || (playerRight.transform.position.x - this.transform.position.x < 1.0f)) {
					Time.timeScale = 0.3F;
				} else {
					Time.timeScale = 1.0F;
					
				}
				Time.fixedDeltaTime = 0.02F * Time.timeScale;
			}
		}
	}

	void OnCollisionEnter(Collision other)
	{
		AudioSource.PlayClipAtPoint (slurp, GameObject.Find("Main Camera").GetComponent<Transform>().position);
		string namz = other.gameObject.name;

		// Set Ship or Astoroid to dying
		if (namz == "Ship") 
		{ 
			other.gameObject.GetComponent<PlayerControl>().isDying = true;
			Destroy(playerRight);
		}
		else if (namz == "Ship2")
		{
			other.gameObject.GetComponent<PlayerControl>().isDying = true;
			Destroy(playerLeft);
		}
		else if(namz == "Asteroid(Clone)" || namz == "Asteroid(Clone)(Clone)") 
		{
			other.gameObject.GetComponent<AsteroidScript>().isDying = true; 
		}
		else if (namz == "Satellite(Clone)")
		{
			other.gameObject.GetComponent<SatelliteScript>().isDying = true; 
		}
		else if (namz == "PowerUpRandom(Clone)")
		{
			other.gameObject.GetComponent<PowerUpRandomScript>().isDying = true; 
		}
		else
		{
			Destroy(other.gameObject);
		}
	}
}
