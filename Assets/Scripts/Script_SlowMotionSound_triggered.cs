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

	public void playSound1() { playSound(audioClip1); }
	public void playSound2() { playSound(audioClip2); }
	public void playSound3() { playSound(audioClip3); }
	public void playSound4() { playSound(audioClip4); }
	public void playSound5() { playSound(audioClip5); }

	private void playSound(AudioClip audioClip)
	{
		audioSource = transform.gameObject.AddComponent<AudioSource>();
		audioSource.clip = audioClip;
		audioSource.Play();
	}

	void Update () 
	{
		if (audioSource != null)
		{
			audioSource.pitch = Time.timeScale;
		}
	}
}
