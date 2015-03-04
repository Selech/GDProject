using UnityEngine;
using System.Collections;

public class PowerupScript : MonoBehaviour {

	public GameObject slot1;
	public GameObject slot2;
	public GameObject slot3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PickupPowerup(GameObject powerup){

		if (slot1 == null) {
			slot1 = powerup;
			powerup.transform.position = new Vector3(-5.45f,4,0);
			powerup.rigidbody.isKinematic = true;	
		}

		else if (slot1 == null) {
			slot2 = powerup;
		}

		else if (slot1 == null) {
			slot3 = powerup;
		}
	}
}
