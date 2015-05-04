using UnityEngine;
using System.Collections;

public class ElectroMine : MonoBehaviour {

	public GameObject gObj_Rotator;
	private float gObj_RotateSpeed = 20.0f;
	public int mineLevel = 1;
	public AudioClip paralyzeSound;
	public ParticleSystem PfxParalysedBlast;
	public ParticleSystem PfxBubbles;
	public ParticleSystem PfxRayStripes;
	public ParticleSystem PfxCircleEdgeStripes;
	
	int paralyzeTimez;
	float explosionStrength;

	// Use this for initialization
	void Start () 
	{
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
		}

		// Behaviour at level 2
		if (mineLevel == 2) {
			explosionStrength = 250.0f;
			paralyzeTimez = 40;
		}

		// Behaviour at level 3
		if (mineLevel == 3) {
			explosionStrength = 350.0f;
			paralyzeTimez = 60;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		// Behaviour at level 1
		if (mineLevel == 1){
			if(other.gameObject.name == "Ship")
			{
				// Ship gameobjects root
				GameObject rootObj = other.collider.gameObject.transform.root.gameObject;

				// Sound
				AudioSource.PlayClipAtPoint (paralyzeSound, GameObject.Find("Main Camera").GetComponent<Transform>().position);
				
				// Blast particles towards ship
				PfxParalysedBlast.Emit(100);
				PfxParalysedBlast.transform.LookAt(rootObj.transform.position, Vector3.back);

				// Push back
				Vector3 forceVec  = -rootObj.GetComponent<Rigidbody>().velocity.normalized * explosionStrength;
				rootObj.GetComponent<Rigidbody>().AddForce(PfxParalysedBlast.transform.forward * explosionStrength); //, ForceMode.Acceleration);

				// Dissable Ship for a period
				rootObj.GetComponent<PlayerControl>().paralyzeTime = paralyzeTimez;
			}
		}
		
		// Behaviour at level 2
		if (mineLevel == 2) {
		}
		
		// Behaviour at level 3
		if (mineLevel == 3) {
		}
	}
}
