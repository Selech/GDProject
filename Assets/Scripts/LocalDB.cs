using UnityEngine;
using System.Collections;

public class LocalDB : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		// Reset Score when Game is opened anew.
		CheckScoreReset();
	}

	public static void CheckScoreReset()
	{
		if (SessionStarted == false)
		{
			Player1_Score = 0;
			Player2_Score = 0;

			SessionStarted = true;
		}
	}

	private static bool sessionStarted = false;
	private static bool SessionStarted
	{
		get { return sessionStarted; }
		set { sessionStarted = value; }
	}

	public static int Player1_Score
	{
		get { return PlayerPrefs.GetInt("Player1_Score"); }
		set { PlayerPrefs.SetInt("Player1_Score", value); }
	}
	
	public static int Player2_Score
	{
		get { return PlayerPrefs.GetInt("Player2_Score"); }
		set { PlayerPrefs.SetInt("Player2_Score", value); }
	}
}
