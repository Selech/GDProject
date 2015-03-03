using UnityEngine;
using System.Collections;

public class PushWeapon : MonoBehaviour, IWeapon {

	public SphereCollider collider;
	public ParticleSystem particle;
	private bool grow = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (grow && !(collider.transform.localScale == new Vector3 (4, 1, 1))) {
			collider.transform.localScale += new Vector3 (0.1f, 0, 0);
		} else {
			grow = false;
			collider.transform.localScale = new Vector3 (1f, 1, 1);
		}
	}

	public void fire(){
		particle.Play ();
		grow = true;
	}
}
