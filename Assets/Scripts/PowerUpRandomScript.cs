using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerUpRandomScript : MonoBehaviour {

	//Vector3 target = new Vector3(0.0f,0.0f,0.0f);
	public Vector3 scale;
	public AudioClip explosion;
	public bool isDying = false;
	public GameObject blastEffect;

	public Text greenScore;
	public Text redScore;

	public GameObject powerUp;

	// Use this for initialization
	void Start () 
	{
		this.transform.localScale = scale;
	}

	public void pushAway(float force)
	{
		// (speed * (1 / (1-(1-Time.timeScale))))
		GetComponent<Rigidbody>().AddForce (force, 0, 0);
	}

	// Update is called once per frame
	void Update () 
	{
		bool atRight = Camera.main.WorldToScreenPoint(transform.position).x > Screen.width / 2;
		GetComponent<Rigidbody>().AddForce ( (atRight)?-2.0f:2.0f,0,0);

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
			detachParticleSystem();
			Destroy ( transform.root.gameObject );
		}
	}

	void detachParticleSystem ()
	{
		Transform trsf = transform.FindChild("Particle System");
		if(trsf != null)
		{
			ParticleSystem pfx = trsf.gameObject.GetComponent<ParticleSystem>();
			pfx.loop = false;
			pfx.startLifetime = 0.1f;
			pfx.transform.parent = null;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		// Push Player backwards
		string nameOfHit = other.collider.gameObject.tag;

		// Count number of powerups
//		if (other.gameObject.tag == "Player") 
//		{
//			if(other.gameObject.name == "Ship")
//			{
//				BlackHoleScript.greenScoreNum++;
//				BlackHoleScript.greenScore.text = BlackHoleScript.greenScoreNum + "";
//			}
//			else
//			{
//				BlackHoleScript.redScoreNum++;
//				BlackHoleScript.redScore.text = BlackHoleScript.redScoreNum + "";
//			}
//		}

		// Explode Asteroide
		if (nameOfHit == "Bullet" || other.gameObject.tag == "Player")
		{
			// Explosion Effect
			//Instantiate(blastEffect, transform.position, new Quaternion());
				
			// Play Sound
			//AudioSource.PlayClipAtPoint (explosion, GameObject.Find("Main Camera").GetComponent<Transform>().position);

			// Destroy Colliders
			detachParticleSystem();
			Destroy(this.gameObject);

			// DESTROY BULLET
			if(nameOfHit == "Bullet")
			{
				Destroy(other.collider.gameObject);
			}
		}
	}
}
