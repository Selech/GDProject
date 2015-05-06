using UnityEngine;
using System.Collections;

public class BulletPowerUpPurple : MonoBehaviour 
{
	public float pushBackForce;

	GameObject ship = null;
	GameObject ship2 = null;

	public PlayerControl playerScript;

	public GameObject bulletPowerUpPurpleBeamLeft;
	public GameObject bulletPowerUpPurpleBeamRight;
	public GameObject bulletPowerUpPurpleHit;
	public GameObject bulletPowerUpPurpleEntry;
	public GameObject bulletPowerUpPurpleBeamAcross;
	public GameObject bulletPowerUpPurpleEntryBlast;

	public AudioClip sfxShot;
	public AudioSource sfxChargingShot;
	
	bool atRight;
	bool firstShoot = true;
	int shotLevel;

	void Update()
	{
		print (Time.timeScale);
		sfxChargingShot.pitch = Time.timeScale;
	}

	// Use this for initialization
	void Start () 
	{
		// Play Sound
		sfxChargingShot.Play();

		// Find references
		ship = GameObject.Find("Ship");
		ship2 = GameObject.Find("Ship2");

		// Rotate if on right
		atRight = Camera.main.WorldToScreenPoint (transform.root.gameObject.transform.position).x > Screen.width / 2;
		if(atRight) {transform.root.gameObject.transform.Rotate(new Vector3(0, 0, 180));}
		
		// Shot level
		shotLevel = playerScript.currentShotLevel;

		// Beam on other side
		if(ship != null && ship2 != null)
		{
			Invoke("DoAttack", 0.65f); 

			if (shotLevel == 2)
			{
				Invoke("DoAttack", 0.8f); 
			}
			else if (shotLevel == 3)
			{
				Invoke("DoAttack", 0.8f); 
				Invoke("DoAttack", 0.95f); 
				Invoke("DoAttack", 1.05f); 
			}
		}
	}

	public void DoAttack()
	{
		// Player references
		GameObject opponent = (atRight) ? ship : ship2;

		if(opponent)
		{
			// Positions
			Vector3 position 	= transform.root.gameObject.transform.position;
			if(firstShoot == false) {position.y += Random.Range(0.0f, 1.5f) - 0.7f;}
			
			// Opponents position
			Vector3 oppoPos 	= opponent.transform.position;
			
			// Is it a hit?
			float hitDist 		= 0.2f; 
			bool hit 			= ((position.y < oppoPos.y+hitDist) && (position.y > oppoPos.y-hitDist));
			
			// Show blasts if not first shot
			if (firstShoot == false)
			{
				// Blast Pfx
				GameObject gObj = (Instantiate(bulletPowerUpPurpleEntryBlast, position, new Quaternion())) as GameObject;
				gObj.GetComponent<ParticleSystem>().Play();
				gObj.GetComponent<ParticleSystem>().Emit(150);
				
				// Sound
				AudioSource.PlayClipAtPoint (sfxShot, GameObject.Find("Main Camera").GetComponent<Transform>().position);
				
				// Beam Across (Shown in front of ship and towards edge)
				GameObject gObj2 = (Instantiate(bulletPowerUpPurpleBeamAcross, new Vector3(position.x + ((atRight) ? (8.5f) : -(8.5f)), position.y + 1.0f, 0), new Quaternion())) as GameObject;
				gObj2.GetComponent<ParticleSystem>().Play();
				gObj2.GetComponent<ParticleSystem>().Emit(150);
				//if(atRight) {transform.root.gameObject.transform.Rotate(new Vector3(0, 0, 180));}
			}
			
			// Hit or no hit
			if(hit)
			{
				// Spawn Pfx on hit and push back
				GameObject gObj = (Instantiate((atRight) ? bulletPowerUpPurpleBeamLeft : bulletPowerUpPurpleBeamRight, new Vector3(oppoPos.x + ((atRight) ? -(8.5f) : (8.5f)), position.y + 1.0f, 0), new Quaternion())) as GameObject;
				gObj.GetComponent<ParticleSystem>().Play();
				gObj.GetComponent<ParticleSystem>().Emit(150);
				GameObject pfxHit = Instantiate(bulletPowerUpPurpleHit, oppoPos, new Quaternion()) as GameObject;
				if(atRight == false) pfxHit.transform.Rotate(new Vector3(0,0,Random.Range(130, 180)));
				
				// Shake screen and push player back
				opponent.GetComponent<Rigidbody>().AddForce (((opponent.name=="Ship") ? pushBackForce : -pushBackForce),0,0);
				Camera.main.GetComponent<Animation>().Play("AsteroidsShake");
			}
			else 
			{	
				GameObject gObj = (Instantiate((atRight) ? bulletPowerUpPurpleBeamLeft : bulletPowerUpPurpleBeamRight, new Vector3((atRight) ? -8.5f : 8.5f, position.y + 1.0f, 0), new Quaternion())) as GameObject;
				gObj.GetComponent<ParticleSystem>().Play();
				gObj.GetComponent<ParticleSystem>().Emit(150);
			}
			
			firstShoot = false;
		}
	}
}
