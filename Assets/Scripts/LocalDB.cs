using UnityEngine;
using System.Collections;

public class LocalDB : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{

	}

	public static void initialize()
	{
		print (SessionExists);
		if (SessionExists == "continue")
		{
			SessionExists = "notToContinue";
		}
		else if (SessionExists == "notToContinue")
		{
			Player1_Score = 0;
			Player2_Score = 0;
		}
	}

	/**
	 * Return 1 if a session exists and otherwise 0.
	 **/
	public static string SessionExists
	{
		get { return PlayerPrefs.GetString("SessionExists"); }
		set { PlayerPrefs.SetString("SessionExists", value); }
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
