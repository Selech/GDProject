using UnityEngine;
using System.Collections;

public class AsteroidScript : MonoBehaviour {

	Vector3 target; 
	public Vector3 scale;
	public AudioClip explosion;
	public bool isDying = false;
	public GameObject blastEffect;
	public GameObject blastEffectSmall;

	// Use this for initialization
	void Start () 
	{
		this.target = new Vector3(0.0f,Random.Range(-5.0f, 5.0f),0.0f);
		this.transform.localScale = scale * Random.Range(1.1f, 1.5f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.transform.position = Vector3.MoveTowards (this.transform.position, target, 0.05f); 
		this.transform.Rotate (4f,4f,0);

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


			// Play Sound
			AudioSource.PlayClipAtPoint (explosion, GameObject.Find("Main Camera").GetComponent<Transform>().position);

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
