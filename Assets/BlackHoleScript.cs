using UnityEngine;
using System.Collections;

public class BlackHoleScript : MonoBehaviour {

	public GameObject playerLeft;
	public GameObject playerRight;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(playerLeft.activeSelf && playerRight.activeSelf){
			if (Vector3.Distance (playerLeft.transform.position,this.transform.position) < 2f || Vector3.Distance (playerRight.transform.position,this.transform.position) < 2f) {
				Time.timeScale = 0.5F;
			} else {
				Time.timeScale = 1.0F;
				
			}
			Time.fixedDeltaTime = 0.02F * Time.timeScale;
		}
	}

	void OnCollisionEnter(Collision other){
		Destroy(other.gameObject);
	}
}
