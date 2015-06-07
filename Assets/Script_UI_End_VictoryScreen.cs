using UnityEngine;
using System.Collections;

public class Script_UI_End_VictoryScreen : MonoBehaviour {

	public KeyCode startButton;
	public GameObject gzLasse;
	public GameObject gzGunver;
	
	// Use this for initialization
	void Start () 
	{
		// Reset flow of time
		Time.timeScale = 1f;

		if(LocalDB.PlayerDead == 2)
		{
			gzLasse.SetActive(true);
		}
		else
		{
			gzGunver.SetActive(true);
		}

		// Reset scores
		PlayerControl.Player1Score = 0; 
		PlayerControl.Player2Score = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(startButton != null)
		{
			if(Input.GetKey (startButton)) 
			{
				Application.LoadLevel ("Test");
			}
		}
	}
}
