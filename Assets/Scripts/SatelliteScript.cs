using UnityEngine;
using System.Collections;

public class SatelliteScript : MonoBehaviour {

	Vector3 target = new Vector3(0.0f,Random.Range(-5.0f, 5.0f),0.0f);
	public Vector3 scale;
	public AudioClip explosion;
	public bool isDying = false;
	public GameObject blastEffect;

	public GameObject powerUpRandom;
	public GameObject powerUpBlue;
	public GameObject powerUpPurple;
	public GameObject powerUpYellow;


	// Use this for initialization
	void Start () {
		this.transform.localScale = scale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.transform.position = Vector3.MoveTowards (this.transform.position, target, 0.05f); 
		this.transform.Rotate (1f,1f,1f);

		if(isDying)
		{
			AnimateDeath();
		}
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
			int ranNum = Random.Range(1, 4);
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
