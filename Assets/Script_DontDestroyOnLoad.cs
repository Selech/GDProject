using UnityEngine;
using System.Collections;

public class Script_DontDestroyOnLoad : MonoBehaviour {

	private static Script_DontDestroyOnLoad instance = null;
	public static Script_DontDestroyOnLoad Instance {
		get { return instance; }
	}
	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	private AudioSource audioSource = null;
	public AudioClip audioClip;
	public float volume;
	private bool stopMusic;

	// Use this for initialization
	void Start () 
	{
		audioSource = transform.root.gameObject.AddComponent<AudioSource>();
		audioSource.clip = audioClip;
		audioSource.volume = volume;
		audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () 
	{
		audioSource.pitch = 1 - (1 - Time.timeScale) / 2;

		if(audioSource.isPlaying == false)
		{
			audioSource.Play();
		}

		if(stopMusic == true)
		{
			if(audioSource.volume > 0)
			{
				audioSource.volume -= 0.02f;
			}
		}
		else
		{
			if(audioSource.volume < volume)
			{
				audioSource.volume += 0.02f;
			}
		}
	}

	public void stopTheMusic()
	{
		stopMusic = true;
	}
	
	public void startTheMusic()
	{
		stopMusic = false;
	}
}
