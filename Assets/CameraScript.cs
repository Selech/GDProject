using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public GameObject playerLeft;
	public GameObject playerRight;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3 (playerLeft.transform.position.x + playerRight.transform.position.x,0,-10);

		this.GetComponent<Camera>().orthographicSize = 4 + (Vector3.Distance (playerLeft.transform.position, playerRight.transform.position) / 12f);
	}
}
