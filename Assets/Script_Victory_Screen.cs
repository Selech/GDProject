using UnityEngine;
using System.Collections;

public class Script_Victory_Screen : MonoBehaviour {

	public KeyCode qButton;
	public KeyCode wButton;
	public KeyCode jButton;
	public KeyCode kButton;
	public KeyCode startButton;
	public GameObject blackholeLine;

	void Start()
	{

	}

	void OnEnable()
	{
		blackholeLine.GetComponent<MeshRenderer>().enabled = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey (qButton) || Input.GetKey (wButton) || Input.GetKey (jButton) || Input.GetKey (kButton) || Input.GetKey (startButton)) 
		{
			Application.LoadLevel ("Test");
		}

		#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		if (Input.anyKey)
		{
			Application.LoadLevel ("Test");
		}
		#endif


		if(Time.timeScale < 1)
		{
			Time.timeScale += 0.02f;
		}
	}
}
