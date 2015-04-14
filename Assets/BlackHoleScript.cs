﻿using UnityEngine;
using System.Collections;

public class BlackHoleScript : MonoBehaviour {

	public GameObject playerLeft;
	public GameObject playerRight;

	public AudioClip slurp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(playerLeft.activeSelf && playerRight.activeSelf){

			if ((playerLeft.transform.position.x - this.transform.position.x > -2f) || (playerRight.transform.position.x - this.transform.position.x < 2f)) {
				Time.timeScale = 0.5F;
			} else {
				Time.timeScale = 1.0F;
				
			}
			Time.fixedDeltaTime = 0.02F * Time.timeScale;
		}
	}

	void OnCollisionEnter(Collision other){
		AudioSource.PlayClipAtPoint (slurp, GameObject.Find("Main Camera").GetComponent<Transform>().position);

		Destroy(other.gameObject);
	}
}