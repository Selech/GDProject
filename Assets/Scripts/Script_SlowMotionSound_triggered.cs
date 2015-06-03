using UnityEngine;
using System.Collections;

public class Script_SlowMotionSound_triggered : MonoBehaviour 
{
	private AudioSource audioSource = null;
	public AudioClip audioClip1;
	public AudioClip audioClip2;
	public AudioClip audioClip3;
	public AudioClip audioClip4;
	public AudioClip audioClip5;

	public void playSound1(bool detach = false, float volume = 1f) { playSound(audioClip1, detach, volume); }
	public void playSound2(bool detach = false, float volume = 1f) { playSound(audioClip2, detach, volume); }
	public void playSound3(bool detach = false, float volume = 1f) { playSound(audioClip3, detach, volume); }
	public void playSound4(bool detach = false, float volume = 1f) { playSound(audioClip4, detach, volume); }
	public void playSound5(bool detach = false, float volume = 1f) { playSound(audioClip5, detach, volume); }

	bool detached = false;

	private void playSound(AudioClip audioClip, bool detach, float volume = 1f)
	{	
		if (detach)
		{
			transform.parent = null;
		}

		audioSource = transform.gameObject.AddComponent<AudioSource>();
		audioSource.clip = audioClip;
		audioSource.volume = volume;
		audioSource.Play ();
	}

	void Update () 
	{
		if (audioSource != null)
		{
			audioSource.pitch = Time.timeScale;
		}

		if (detached == true)
		{

		}
	}
}
