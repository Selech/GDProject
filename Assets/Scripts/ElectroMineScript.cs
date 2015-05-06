using UnityEngine;
using System.Collections;

public class ElectroMineScript : MonoBehaviour {

	public GameObject gObj_Rotator;
	private float gObj_RotateSpeed = 20.0f;
	public int mineLevel = 1;
	public AudioClip paralyzeSound;
	public AudioClip explosionSound;
	public ParticleSystem PfxParalysedBlast;
	public ParticleSystem PfxBubbles;
	public ParticleSystem PfxRayStripes;
	public ParticleSystem PfxCircleEdgeStripes;
	public ParticleSystem PfxDissapearEffect;

	int paralyzeTimez;
	int emmitAmount;
	float explosionStrength;
	int lifeTime;

	// Use this for initialization
	void Start () 
	{
		// Sound
		AudioSource.PlayClipAtPoint (explosionSound, GameObject.Find("Main Camera").GetComponent<Transform>().position);

		// Set LifeTime
		lifeTime = Random.Range(75, 250);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Rotation of Particle Effect
		gObj_Rotator.transform.Rotate (Vector3.back, gObj_RotateSpeed * Time.deltaTime);

		// Behaviour at level 1
		if (mineLevel == 1){
			explosionStrength = 150.0f;
			paralyzeTimez = 20;
			emmitAmount = 75;
		}

		// Behaviour at level 2
		if (mineLevel == 2) {
			explosionStrength = 250.0f;
			paralyzeTimez = 40;
			emmitAmount = 125;
		}

		// Behaviour at level 3
		if (mineLevel == 3) {
			explosionStrength = 350.0f;
			paralyzeTimez = 60;
			emmitAmount = 200;
		}

		// Destroy when time runs out
		if(lifeTime == 0)
		{
			(Instantiate(PfxDissapearEffect, transform.root.gameObject.transform.position, new Quaternion()) as ParticleSystem).Play();
			Destroy(transform.root.gameObject);
		}
		else if(lifeTime >= 0)
		{
			lifeTime--;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Player")
		{
			// Ship gameobjects root
			GameObject rootObj = other.collider.gameObject.transform.root.gameObject;
			
			// Sound
			AudioSource.PlayClipAtPoint (paralyzeSound, GameObject.Find("Main Camera").GetComponent<Transform>().position);
			
			// Blast particles towards ship
			PfxParalysedBlast.Emit(emmitAmount);
			PfxParalysedBlast.transform.LookAt(rootObj.transform.position, Vector3.back);
			
			// Push back
//			Vector3 forceVec  = -rootObj.GetComponent<Rigidbody>().velocity.normalized * explosionStrength;
			rootObj.GetComponent<Rigidbody>().AddForce(PfxParalysedBlast.transform.forward * explosionStrength); //, ForceMode.Acceleration);
			
			// Dissable Ship for a period
			rootObj.GetComponent<PlayerControl>().paralyzeTime = paralyzeTimez;
		}
	}
}
