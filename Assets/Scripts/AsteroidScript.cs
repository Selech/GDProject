using UnityEngine;
using System.Collections;

public class AsteroidScript : MonoBehaviour {

	Vector3 target = new Vector3(0.0f,0.0f,0.0f);
	public Vector3 scale;
	public AudioClip explosion;
	public bool isDying = false;

	// Use this for initialization
	void Start () {
		this.transform.localScale = scale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isDying == false)
		{
			this.transform.position = Vector3.MoveTowards (this.transform.position, target, 0.05f); 
			this.transform.Rotate (4f,4f,0);
		}
		else
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
		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Bullet(Clone)") 
		{
			other.collider.gameObject.transform.position = Vector3.MoveTowards (other.collider.gameObject.transform.position, target, 0.5f);
			Camera.main.GetComponent<Animation>().Play("AsteroidsShake");

			if(this.gameObject.GetComponent<AsteroidScript>().scale.x > 0.25)
			{
				GameObject left = (GameObject) Instantiate(this.gameObject,this.transform.position,new Quaternion(0,0,0.7f,0.7f));
				left.GetComponent<AsteroidScript>().scale = scale /2 ;
				left.GetComponent<ParticleSystem>().startSize = scale.x / 2;
				left.GetComponent<ParticleSystem>().Play();
				left.transform.position = Vector3.MoveTowards (this.transform.position, target, -1.5f); 
			}

			AudioSource.PlayClipAtPoint (explosion, GameObject.Find("Main Camera").GetComponent<Transform>().position);
			Destroy(this.gameObject);
		}
	}
}
