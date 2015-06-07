using UnityEngine;
using System.Collections;

public class Script_Delayed_Sound : MonoBehaviour {

	public int delay;
	public float volume;
	public AudioClip audioClip;
	private AudioSource audioSource;
	public bool dontDestroyAfterwards;
	bool playing;

	// Use this for initialization
	void Start () 
	{
		audioSource = transform.root.gameObject.AddComponent<AudioSource>();
		audioSource.clip = audioClip;
		audioSource.volume = volume;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(delay >= 0) delay--;
		if(delay < 0) 
		{
			if (playing == false)
			{
				audioSource.Play();
			}
			playing = true;
		}

		if(dontDestroyAfterwards == false)
		{
			if(playing == true && audioSource.isPlaying == false)
			{
				Destroy(transform.gameObject);
			}
		}
	}
}
