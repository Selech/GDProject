using UnityEngine;
using System.Collections;

public class ElectroMineScript : MonoBehaviour {

	public GameObject gObj_Rotator;
	private float gObj_RotateSpeed = 20.0f;
	public int mineLevel = 1;
	public ParticleSystem PfxParalysedBlast;
	public ParticleSystem PfxBubbles;
	public ParticleSystem PfxRayStripes;
	public ParticleSystem PfxCircleEdgeStripes;
	public ParticleSystem PfxDissapearEffect;

	float paralyzeTimez;
	int emmitAmount;
	float explosionStrength;
	float lifeTime;
	bool isMovingDown;

	Script_SlowMotionSound_triggered scr;
	bool settingsSet;

	// Use this for initialization
	void Start () 
	{
		// Random Direction
		isMovingDown = Random.Range(0, 2) == 0;

		// Sound
		scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered> ();
		scr.playSound2();

		if (settingsSet == false)
		{
			settingsSet = true;

			// Behaviour at level 1
			if (mineLevel == 1){
				explosionStrength = 150.0f;
				paralyzeTimez = 20;
				emmitAmount = 75;
				lifeTime = Random.Range(75, 150);
			}
			
			// Behaviour at level 2
			if (mineLevel == 2) {
				explosionStrength = 250.0f;
				paralyzeTimez = 40;
				emmitAmount = 125;
				lifeTime = Random.Range(100, 250);
			}
			
			// Behaviour at level 3
			if (mineLevel == 3) {
				explosionStrength = 350.0f;
				paralyzeTimez = 60;
				emmitAmount = 200;
				lifeTime = Random.Range(125, 350);
				traverseVeritcally();
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Rotation of Part icle Effect
		gObj_Rotator.transform.Rotate (Vector3.back, gObj_RotateSpeed * Time.deltaTime);

		// Destroy when time runs out
		if(lifeTime < 0)
		{
			(Instantiate(PfxDissapearEffect, transform.root.gameObject.transform.position, new Quaternion()) as ParticleSystem).Play();
			Destroy(transform.root.gameObject);
		}
		else if(lifeTime >= 0)
		{
			lifeTime -= Time.timeScale;
		}
	}

	void traverseVeritcally()
	{
		float speed = 1.0f;
		transform.Translate(((isMovingDown) ? Vector3.down : Vector3.up) * speed * Time.deltaTime);

		float dist = (transform.position - Camera.main.transform.position).z; 
		float topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0,0,dist)).y; 
		//float bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(1,0,dist)).y;

		float borderDist = 0.6f;
		if(transform.position.y > Mathf.Abs(topBorder) + borderDist)
		{
			isMovingDown = true;
		}
		else if(transform.position.y < topBorder + borderDist)
		{
			isMovingDown = false;
		}
//		print ("----");
//		print ("down: "+isMovingDown );
//		print ("top: "+topBorder);
////		print ("bot: "+bottomBorder);
//		print ("abs: "+ (Mathf.Abs(topBorder)-0.5f));
//
//		print ("y: "+transform.position.y);
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Player")
		{
			// Ship gameobjects root
			GameObject rootObj = other.collider.gameObject.transform.root.gameObject;
			
			// Sound
			scr.playSound1();
			
			// Blast particles towards ship
			PfxParalysedBlast.Emit(emmitAmount);
			PfxParalysedBlast.transform.LookAt(rootObj.transform.position, Vector3.back);
			
			// Push back
			rootObj.GetComponent<Rigidbody>().AddForce(PfxParalysedBlast.transform.forward * explosionStrength * (1 / (1-(1-Time.timeScale)))); //, ForceMode.Acceleration);
			
			// Dissable Ship for a period
			rootObj.GetComponent<PlayerControl>().paralyzeTime = paralyzeTimez;
		}
	}
}
