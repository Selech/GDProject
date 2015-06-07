using UnityEngine;
using System.Collections;

public class ToiletScript : MonoBehaviour 
{
	public Vector3 scale;
	public bool isDying = false;
	public GameObject blastEffect;
	public GameObject blastEffectSmall;
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

		// Set rotation speed
		xRotationSpeed = Random.Range(0, 2.0f);
		yRotationSpeed = Random.Range(0, 2.0f);
		zRotationSpeed = Random.Range(0, 2.0f);

		// Scale
		this.transform.localScale = scale * Random.Range(1.1f, 1.5f);
		
		// Speed
		transform.root.gameObject.GetComponent<Rigidbody>().AddForce (new Vector3(((atRight) ? -1 : 1) * (speed * (1 / (1-(1-Time.timeScale)))), 0, 0));
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Remove is game is over
		if(ObstacleGenerator.isRoundWon)
		{
			Destroy(transform.root.gameObject);
		}

		// Rotate Asteroid
		this.transform.Rotate (xRotationSpeed * Time.timeScale, yRotationSpeed * Time.timeScale, zRotationSpeed * Time.timeScale);

		// Dying Animation
		if(isDying)
		{
			AnimateDeath();
		}
	}

	void AnimateDeath()
	{
		// Move to mid
		bool atRight = Camera.main.WorldToScreenPoint (transform.root.gameObject.transform.position).x > Screen.width / 2;
		transform.transform.LookAt(new Vector3(0, transform.position.y, 0));
		transform.Translate( 	(((atRight) ? -0.1f: -0.1f)*Time.timeScale), 0 ,0);

		// Scale Down
		float scaleSpeed = 0.2f  * Time.timeScale;
		float scaleX;
		float scaleY;
		float scaleZ;
		scaleX = (transform.localScale.x > 0) ? transform.localScale.x * scaleSpeed : transform.localScale.x;
		scaleY = (transform.localScale.y > 0) ? transform.localScale.y * scaleSpeed : transform.localScale.y;
		scaleZ = (transform.localScale.z > 0) ? transform.localScale.z * scaleSpeed : transform.localScale.z;
		
		transform.localScale -= new Vector3(scaleX, scaleY, scaleZ);
	
		if (transform.localScale.x < 0.1f)
		{
			Destroy ( transform.root.gameObject );
		}
	}

	void OnCollisionEnter(Collision other)
	{
		// Push Player backwards
		string nameOfHit = other.collider.gameObject.tag;
		if (other.gameObject.tag == "Player") 
		{
			other.gameObject.GetComponent<Rigidbody>().AddForce (((other.gameObject.name=="Ship")?400:-400),0,0);
			Camera.main.GetComponent<Animation>().Play("AsteroidsShake");
		}

		// Explode Asteroide
		if (nameOfHit == "Bullet" || other.gameObject.tag == "Player")
		{
			// Smaller Asteroid
			GetComponent<ParticleSystem>().Play();
			
			// Explosion Effect
			Instantiate(blastEffectSmall, transform.position, new Quaternion());

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
