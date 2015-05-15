using UnityEngine;
using System.Collections;

public class SatelliteScript : MonoBehaviour {

	public Vector3 scale;
	public AudioClip explosion;
	public bool isDying = false;
	public GameObject blastEffect;

	public GameObject powerUpRandom;
	public GameObject powerUpBlue;
	public GameObject powerUpPurple;
	public GameObject powerUpYellow;

	public float speed;
	float xRotationSpeed;
	float yRotationSpeed;
	float zRotationSpeed;
	bool atRight;

	// Use this for initialization
	void Start () 
	{
		// Save if at right
		atRight = Camera.main.WorldToScreenPoint (transform.root.gameObject.transform.position).x > Screen.width / 2;

		this.transform.localScale = scale;

		// Set rotation speed
		xRotationSpeed = Random.Range(0, 1.5f);
		yRotationSpeed = Random.Range(0, 1.5f);
		zRotationSpeed = Random.Range(0, 1.5f);
		
		// Speed
		transform.root.gameObject.GetComponent<Rigidbody>().AddForce (new Vector3(((atRight) ? -1 : 1) * (speed * (1 / (1-(1-Time.timeScale)))), 0, 0));
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Rotate Asteroid
		this.transform.Rotate (xRotationSpeed * Time.timeScale, yRotationSpeed * Time.timeScale, zRotationSpeed * Time.timeScale);

		// Death Animation
		if(isDying) AnimateDeath();
	}
	
	void AnimateDeath()
	{
		float scaleSpeed = 0.02f;
		this.transform.localScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
	
		if(this.transform.localScale.x < 0.0f)
		{
			Destroy ( transform.root.gameObject );
		}
	}

	void OnCollisionEnter(Collision other)
	{
		// Push Player backwards
		string nameOfHit = other.collider.gameObject.tag;

		// Move player backwards
		if (other.gameObject.tag == "Player") 
		{
			other.gameObject.GetComponent<Rigidbody>().AddForce (((other.gameObject.name=="Ship")?400:-400),0,0);
			Camera.main.GetComponent<Animation>().Play("AsteroidsShake");
		}

		// Explode Asteroide
		if (nameOfHit == "Bullet" || other.gameObject.tag == "Player")
		{
			// Explosion Effect
			Instantiate(blastEffect, transform.position, new Quaternion());
				
			// Play Sound
			AudioSource.PlayClipAtPoint (explosion, GameObject.Find("Main Camera").GetComponent<Transform>().position);

			// Destroy Colliders
			Destroy(this.gameObject);

			// DESTROY BULLET
			if(nameOfHit == "Bullet")
			{
				Destroy(other.collider.gameObject);
			}

			bool leftOfMiddle = Camera.main.WorldToScreenPoint(transform.position).x > Screen.width / 2;

			// Spawn Random PowerUp
			int ranNum = Random.Range(1, 5);
			GameObject gObj = null;

			if (ranNum == 1)
			{
				gObj = Instantiate(powerUpRandom, transform.position, new Quaternion()) as GameObject;
				gObj.GetComponent<PowerUpRandomScript>().pushAway(	(leftOfMiddle)?400:-400 );
			}
			else if (ranNum == 2)
			{
				gObj = Instantiate(powerUpBlue, transform.position, new Quaternion()) as GameObject;
				gObj.GetComponent<PowerUpRandomScript>().pushAway(	(leftOfMiddle)?400:-400 );
			}
			else if (ranNum == 3)
			{
				gObj = Instantiate(powerUpYellow, transform.position, new Quaternion()) as GameObject;
				gObj.GetComponent<PowerUpRandomScript>().pushAway(	(leftOfMiddle)?400:-400 );
			}
			else if (ranNum == 4)
			{
				gObj = Instantiate(powerUpPurple, transform.position, new Quaternion()) as GameObject;
				gObj.GetComponent<PowerUpRandomScript>().pushAway(	(leftOfMiddle)?400:-400 );
			}
		}
	}
}
