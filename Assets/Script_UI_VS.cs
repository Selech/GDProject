using UnityEngine;
using System.Collections;

public class Script_UI_VS : MonoBehaviour 
{

	public int countDown;
	public int countDownPositionShips;
	private bool shipsPositioned;

	public GameObject ship;
	public GameObject ship2;
	public GameObject uiScore;

	public GameObject round1;
	public GameObject round2;
	public GameObject round3;
	public GameObject round4;
	public GameObject round5;
	public GameObject round6;
	public GameObject round7;
	public GameObject round8;
	public GameObject round9;
	public GameObject roundFinal;

	public GameObject pfxPointsGivenRed;
	public GameObject pfxPointsGivenGreen;

	// Use this for initialization
	void Start () 
	{
		int player1score = PlayerControl.Player1Score; 
		int player2score = PlayerControl.Player2Score;
		int curRound = player1score + player2score;

		// Final Round
		if(player1score == 4 || player2score == 4)
		{
			roundFinal.SetActive(true);
		}
		// Not Final Round
		else if (curRound == 0) {round1.SetActive(true);}
		else if (curRound == 1)	{round2.SetActive(true);}
		else if (curRound == 2)	{round3.SetActive(true);}
		else if (curRound == 3)	{round4.SetActive(true);}
		else if (curRound == 4)	{round5.SetActive(true);}
		else if (curRound == 5)	{round6.SetActive(true);}
		else if (curRound == 6)	{round7.SetActive(true);}
		else if (curRound == 7)	{round8.SetActive(true);}
		else if (curRound == 8)	{round9.SetActive(true);}
	}
	
	// Update is called once per frame
	void Update () 
	{
		countDown--;
		countDownPositionShips--;

		if (countDownPositionShips < 0 && shipsPositioned == false)
		{
			shipsPositioned = true;
			ship.transform.position = new Vector3(-4, ship.transform.position.y, ship.transform.position.z);
			ship2.transform.position = new Vector3(4, ship2.transform.position.y, ship.transform.position.z);
		}

		if (countDown < 0)
		{
			ship.GetComponent<PlayerControl>().enabled = true;
			ship2.GetComponent<PlayerControl>().enabled = true;
			uiScore.SetActive(true);
			Destroy(transform.root.gameObject);
		}
	}
}
