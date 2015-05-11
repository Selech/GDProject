using UnityEngine;
using System.Collections;

public class AsteroidScript : MonoBehaviour {

	Vector3 target; 
	public Vector3 scale;
	public bool isDying = false;
	public GameObject blastEffect;
	public GameObject blastEffectSmall;
	bool atRight;
	public float speed;
	float xRotationSpeed;
	float yRotationSpeed;
	float zRotationSpeed;

	// Use this for initialization
	void Start () 
	{
		this.target = new Vector3(0.0f,Random.Range(-5.0f, 5.0f),0.0f);
		this.transform.localScale = scale * Random.Range(1.1f, 1.5f);

		// Save if at right
		atRight = Camera.main.WorldToScreenPoint (transform.root.gameObject.transform.position).x > Screen.width / 2;
		
		// Wuut... virkede kun hvis den var her to gange i træk :S
		transform.root.gameObject.GetComponent<Rigidbody>().AddForce (new Vector3(((atRight) ? -1 : 1) * (speed * (1 / (1-(1-Time.timeScale)))), 0, 0));

		// Rotation Speed
		xRotationSpeed = Random.Range(0, 2.0f);
		yRotationSpeed = Random.Range(0, 2.0f);
		zRotationSpeed = Random.Range(0, 2.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Rotate Asteroid
		this.transform.Rotate (xRotationSpeed * Time.timeScale, yRotationSpeed * Time.timeScale, zRotationSpeed * Time.timeScale);

		// Animate Death
		if(isDying) AnimateDeath();
	}
	
	void AnimateDeath()
	{
		float scaleSpeed = 0.02f * Time.timeScale;
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
			if(this.gameObject.GetComponent<AsteroidScript>().scale.x > 0.25)
			{
				// Explosion Effect
				Instantiate(blastEffect, transform.position, new Quaternion());

				// Smaller Asteroid
				GameObject left = (GameObject) Instantiate(this.gameObject,this.transform.position,new Quaternion(0,0,0.7f,0.7f));
				left.GetComponent<AsteroidScript>().scale = scale /2 ;
				left.GetComponent<ParticleSystem>().startSize = scale.x / 2;
				left.GetComponent<ParticleSystem>().Play();
				left.transform.position = Vector3.MoveTowards (this.transform.position, target, -1.5f); 
			}
			else
			{
				// Explosion Effect
				Instantiate(blastEffectSmall, transform.position, new Quaternion());
			}

			// Destroy Colliders
			Destroy(this.gameObject);

			// DESTROY BULLET
			if(nameOfHit == "Bullet")
			{
				Destroy(other.collider.gameObject);
			}
		}
	}
}
