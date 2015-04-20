using UnityEngine;
using System.Collections;

public class BlackHoleScript : MonoBehaviour {

	public GameObject playerLeft;
	public GameObject playerRight;

	public AudioClip slurp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckForSlowmotion();
	}

	/**
	 * When a player is near this blackhole everything slows down.
	 **/
	void CheckForSlowmotion()
	{
		if(playerLeft != null && playerRight != null)
		{
			if(playerLeft.activeSelf && playerRight.activeSelf)
			{
				if ((playerLeft.transform.position.x - this.transform.position.x > -2f) || (playerRight.transform.position.x - this.transform.position.x < 2f)) {
					Time.timeScale = 0.3F;
				} else {
					Time.timeScale = 1.0F;
					
				}
				Time.fixedDeltaTime = 0.02F * Time.timeScale;
			}
		}
	}

	void OnCollisionEnter(Collision other){
		AudioSource.PlayClipAtPoint (slurp, GameObject.Find("Main Camera").GetComponent<Transform>().position);

		// Set Ship to dying
		if (other.collider.name == "Ship")
		{
			other.gameObject.GetComponent<PlayerControl>().isDying = true;
		}
		else
		{
			Destroy(other.gameObject);
		}
	}
}
