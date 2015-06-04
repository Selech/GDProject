using UnityEngine;
using System.Collections;

public class Script_Victory_Screen : MonoBehaviour {

	public KeyCode qButton;
	public KeyCode wButton;
	public KeyCode jButton;
	public KeyCode kButton;
	public KeyCode startButton;

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
	}
}
