using UnityEngine;
using System.Collections;

public class Script_UI_Intro : MonoBehaviour {

	bool showIntro = false;
	public KeyCode startButton;
	public GameObject UI_VS;

	// Use this for initialization
	void Start () 
	{
		if(ObstacleGenerator.isOpenened == false)
		{
			ObstacleGenerator.isOpenened = true;
			showIntro = true;
		}
		else
		{
			gotoVsScreen();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Pause Game	
		Time.timeScale = 0f;

		if (showIntro == true)
		{
			if(startButton != null && UI_VS != null)
			{
				if(Input.GetKey (startButton)) 
				{
					gotoVsScreen();
				}
			}
		}
	}

	void gotoVsScreen ()
	{
		UI_VS.SetActive(true);
		Destroy(transform.root.gameObject);
	}
}
