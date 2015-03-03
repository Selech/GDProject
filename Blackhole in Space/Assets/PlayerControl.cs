using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	public int PlayerNumber;

	public KeyCode up;
	public KeyCode down;
	public KeyCode left;
	public KeyCode right;
	public float speed;
	public KeyCode shoot;

	public GameObject ship;
	public GameObject target;

	public GameObject bullet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position,new Vector3(0,0,0),0.01f); 

		asteroidsControl ();
		//physicsControl ();
		//translateControl ();
	}

	public void asteroidsControl(){
		//transform.rotation = Vector3.RotateTowards(transform.position,target.transform.position,2f,2f);

		if (Input.GetKey (up)) {
			Vector3 direction = ship.transform.position - target.transform.position;
			print (direction);
			rigidbody.AddForce(-direction*1.5f,ForceMode.Force);
		}
		
		if (Input.GetKey (down)) {
			Vector3 direction = ship.transform.position - target.transform.position;
			print (direction);
			rigidbody.AddForce(direction,ForceMode.Force);
		}
		
		if (Input.GetKey (left)) {
			print (target.transform.position.y - ship.transform.position.y);

			if (PlayerNumber == 1 && (target.transform.position.y - ship.transform.position.y > -1)){
				target.transform.Translate (new Vector3 (-0.12f, 0, 0));
				ship.transform.Rotate (Vector3.back * 5);
			}
			
			if(PlayerNumber == 2 && (target.transform.position.y - ship.transform.position.y < 1) ){
				target.transform.Translate(new Vector3(0.12f,0,0));
				ship.transform.Rotate (Vector3.back * 5);

			}
		}
		
		if (Input.GetKey (right) ) {
			if(PlayerNumber == 1 && (target.transform.position.y - ship.transform.position.y < 1)){
				target.transform.Translate(new Vector3(0.12f,0,0));
				ship.transform.Rotate(-Vector3.back * 5);

			}

			if(PlayerNumber == 2 && (target.transform.position.y - ship.transform.position.y > -1)){
				target.transform.Translate(new Vector3(-0.12f,0,0));
				ship.transform.Rotate(-Vector3.back * 5);
			}
		}

		if(Input.GetKeyDown(shoot)){
			Instantiate(bullet, ship.transform.position - (new Vector3(0.5f, 
			           (ship.transform.position.y - target.transform.position.y) * 0.5f, 0f)), new Quaternion());
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

	public void physicsControl(){
		if (Input.GetKey (up)) {
			rigidbody.AddForce(new Vector3(0,speed,0),ForceMode.Force);
		}

		if (Input.GetKey (down)) {
			rigidbody.AddForce(new Vector3(0,-speed,0),ForceMode.Force);
		}

		if (Input.GetKey (left)) {
			rigidbody.AddForce(new Vector3(-speed,0,0),ForceMode.Force);
		}

		if (Input.GetKey (right)) {
			rigidbody.AddForce(new Vector3(speed,0,0),ForceMode.Force);
		}
	}
}
