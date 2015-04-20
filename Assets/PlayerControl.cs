using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {
	public int PlayerNumber;

	public KeyCode up;
	public KeyCode down;
	public KeyCode left;
	public KeyCode right;
	public float speed;
	public KeyCode shootBullet;
	public KeyCode shootDoubleBullets;

	public GameObject ship;
	public GameObject target;

	public GameObject bullet;
	public GameObject doubleBullet;
	public GameObject VictoryScreen; 

	private bool isMoving;
	private bool isSpeedingUp;
	private AudioSource audioSrc;
	public AudioClip shot;
	public bool isDying = false;

	// Use this for initialization
	void Start () 
	{
		audioSrc = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isDying == false)
		{
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0, 0, 0), 0.005f); 
			
			//asteroidsControl ();
			physicsControl ();
			//translateControl ();
			
			flightSound();
		}
		else
		{
			AnimateDeath();
		}
	}

	void AnimateDeath()
	{
		float scaleSpeed = 0.02f;
		ship.transform.localScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
		if(ship.transform.localScale.x < 0.0f)
		{
			Destroy(GameObject.Find(ship.transform.parent.gameObject.name));

			VictoryScreen.SetActive(true);

			if(ship.transform.parent.gameObject.name == "Ship")
			{
				VictoryScreen.transform.Find("pfxRed").gameObject.SetActive(true);
				VictoryScreen.transform.Find("Image (RED in MIddle)").gameObject.SetActive(true);
				VictoryScreen.transform.Find("Text (Red is Victorious)").gameObject.SetActive(true);
			}
			else
			{
				VictoryScreen.transform.Find("pfxRed").gameObject.SetActive(true);
				VictoryScreen.transform.Find("Image (Green in MIddle)").gameObject.SetActive(true);
				VictoryScreen.transform.Find("Text (Green is Victorious)").gameObject.SetActive(true);
			}
		}
	}

	public void flightSound()
	{
		if(isMoving)
		{
			if(isSpeedingUp == false)
			{
				audioSrc.time = 0;
				isSpeedingUp = true;
			}

			if (audioSrc.time == 0)
			{
				audioSrc.Play();
			}
			else if (audioSrc.time > 1.405f)
			{
				audioSrc.time = 0.85f;
			}
		}
		else if (audioSrc.time != 0 && audioSrc.time < 1.4f)
		{
			isSpeedingUp = false;
			audioSrc.time = 1.4f;
			audioSrc.Play();
		}

		if (audioSrc.time == 2.548f)
		{
			audioSrc.time = 0;
		}
	}

	public void physicsControl(){
		//print ("Physics controls");

		//Single bullet
		if(Input.GetKeyDown(shootBullet)){
			AudioSource.PlayClipAtPoint (shot, GameObject.Find("Main Camera").GetComponent<Transform>().position);

			Instantiate(bullet, ship.transform.position - (new Vector3(0.5f, 
			                                                           (ship.transform.position.y - target.transform.position.y) * 0.5f, 0f)), new Quaternion());
		}

		//Double bullet
		if(Input.GetKeyDown(shootDoubleBullets)){
			// Bullet on the right side of the ship
			Instantiate(doubleBullet, ship.transform.position - (new Vector3((float)ship.transform.rotation.y + 0.5f, 
			                                                                 (ship.transform.position.y - target.transform.position.y) * 0.3f, 0f)), new Quaternion());
			
			// Bullet on the left side of the ship
			Instantiate(doubleBullet, ship.transform.position - (new Vector3((float)ship.transform.rotation.y - 0.5f, 
			                                                                 (ship.transform.position.y - target.transform.position.y) * 0.3f, 0f)), new Quaternion());
			
		}

		if (Input.GetKey (right)) {
			GetComponent<Rigidbody>().AddForce(new Vector3(0,speed,0),ForceMode.Force);
		}

		if (Input.GetKey (left)) {
			GetComponent<Rigidbody>().AddForce(new Vector3(0,-speed,0),ForceMode.Force);
		}

		if (Input.GetKey (up)) {
			if(PlayerNumber == 1){
				GetComponent<Rigidbody>().AddForce(new Vector3(-speed,0,0),ForceMode.Force);
			}
			if(PlayerNumber == 2){
				GetComponent<Rigidbody>().AddForce(new Vector3(speed,0,0),ForceMode.Force);
			} 
		}

		if (Input.GetKey (down)) {
			if(PlayerNumber == 1){
				GetComponent<Rigidbody>().AddForce(new Vector3(speed*0.5f,0,0),ForceMode.Force);
			}
			if(PlayerNumber == 2){
				GetComponent<Rigidbody>().AddForce(new Vector3(-speed*0.5f,0,0),ForceMode.Force);
			} 		
		}

		// For Sound
		if(Input.GetKey (right) || Input.GetKey (left) || Input.GetKey (up) || Input.GetKey (down)) 
			 isMoving = true;
		else isMoving = false;
	}

	public void asteroidsControl(){
		//transform.rotation = Vector3.RotateTowards(transform.position,target.transform.position,2f,2f);
		
		if (Input.GetKey (up)) {
			Vector3 direction = ship.transform.position - target.transform.position;
			print (direction);
			GetComponent<Rigidbody>().AddForce(-direction*1.5f,ForceMode.Force);
		}
		
		if (Input.GetKey (down)) {
			Vector3 direction = ship.transform.position - target.transform.position;
			print (direction);
			GetComponent<Rigidbody>().AddForce(direction,ForceMode.Force);
		}
		
		if (Input.GetKey (left)) {
			print (target.transform.position.y - ship.transform.position.y);
			
			if (PlayerNumber == 1 && (target.transform.position.y - ship.transform.position.y > -1)){
				target.transform.Translate (new Vector3 (-0.12f, 0, 0));
				ship.transform.Rotate (Vector3.back * 5);
				ship.transform.Rotate (0,3f,0);
			}
			
			if(PlayerNumber == 2 && (target.transform.position.y - ship.transform.position.y < 1) ){
				target.transform.Translate(new Vector3(0.12f,0,0));
				ship.transform.Rotate (Vector3.back * 5);
				ship.transform.Rotate (0,-3f,0);
			}
		}
		
		if (Input.GetKey (right) ) {
			if(PlayerNumber == 1 && (target.transform.position.y - ship.transform.position.y < 1)){
				target.transform.Translate(new Vector3(0.12f,0,0));
				ship.transform.Rotate(-Vector3.back * 5);
				ship.transform.Rotate (0,-3f,0);
				
			}
			
			if(PlayerNumber == 2 && (target.transform.position.y - ship.transform.position.y > -1)){
				target.transform.Translate(new Vector3(-0.12f,0,0));
				ship.transform.Rotate(-Vector3.back * 5);
				ship.transform.Rotate (0,3f,0);
			}
		}
		
		// Shooting a bullet
		if(Input.GetKeyDown(shootBullet)){
			Instantiate(bullet, ship.transform.position - (new Vector3(0.5f, 
			                                                           (ship.transform.position.y - target.transform.position.y) * 0.5f, 0f)), new Quaternion());
		}
		
		// Shooting double bullets
		if(Input.GetKeyDown(shootDoubleBullets)){
			
			// Bullet on the right side of the ship
			Instantiate(doubleBullet, ship.transform.position - (new Vector3((float)ship.transform.rotation.y + 0.5f, 
			                                                                 (ship.transform.position.y - target.transform.position.y) * 0.3f, 0f)), new Quaternion());
			
			// Bullet on the left side of the ship
			Instantiate(doubleBullet, ship.transform.position - (new Vector3((float)ship.transform.rotation.y - 0.5f, 
			                                                                 (ship.transform.position.y - target.transform.position.y) * 0.3f, 0f)), new Quaternion());
			
			print("Ship rotation: " + ship.transform.rotation);
		}
	}

	public void translateControl(){
		if (Input.GetKey (up)) {
			transform.Translate(new Vector3(0,speed,0));
		}
		
		if (Input.GetKey (down)) {
			transform.Translate(new Vector3(0,-speed,0));
		}
		
		if (Input.GetKey (left)) {
			transform.Translate(new Vector3(-speed,0,0));
		}
		
		if (Input.GetKey (right)) {
			transform.Translate(new Vector3(speed,0,0));
		}
	}
}
