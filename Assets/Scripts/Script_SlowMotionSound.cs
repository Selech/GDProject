﻿using UnityEngine;
using System.Collections;

public class Script_SlowMotionSound : MonoBehaviour 
{
	private AudioSource audioSource = null;
	public AudioClip audioClip;

	// Use this for initialization
	void Start () 
	{
		audioSource = transform.root.gameObject.AddComponent<AudioSource>();
		audioSource.clip = audioClip;
		audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () 
	{
		audioSource.pitch = Time.timeScale;
	}
}