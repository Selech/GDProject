using UnityEngine;
using System.Collections;

public class BulletPowerUpPurple : MonoBehaviour 
{
	public float pushBackForce;

	GameObject ship = null;
	GameObject ship2 = null;

	public PlayerControl playerScript;
	
	public ParticleSystem bulletPowerUpPurpleEntryBlast;
	public ParticleSystem bulletPowerUpPurpleEntryRing;
	public ParticleSystem bulletPowerUpPurpleEntryStripes;
	public ParticleSystem bulletPowerUpPurpleEntryCloud;
	public ParticleSystem bulletPowerUpPurpleBeamAcross;
	public GameObject bulletPowerUpPurpleBeamLeft;
	public GameObject bulletPowerUpPurpleBeamRight;
	public GameObject bulletPowerUpPurpleHit;
	public GameObject bulletPowerUpPurpleEntry;

	public AudioClip sfxShot;
	public AudioSource sfxChargingShot;
	
	bool atRight;
	bool firstShoot = true;
	int shotLevel;

	public int getShootCooldown (int currentShotLevel)
	{
		if (currentShotLevel == 2) return 80;
		else if (currentShotLevel == 3) return 50;
		return 140;
	}

	// Use this for initialization
	void Start () 
	{
		// Play Charge'n'shoot
		Script_SlowMotionSound_triggered scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered> ();
		scr.playSound1();

		// Find references
		ship = GameObject.Find("Ship");
		ship2 = GameObject.Find("Ship2");

		// Rotate if on right
		atRight = Camera.main.WorldToScreenPoint (transform.root.gameObject.transform.position).x > Screen.width / 2;
		if(atRight) {transform.root.gameObject.transform.Rotate(new Vector3(0, 0, 180));}
		
		// Shot level
		shotLevel = playerScript.currentShotLevel;

		// Charge effects
		bulletPowerUpPurpleEntryRing.Play();
		bulletPowerUpPurpleEntryStripes.Play();
		bulletPowerUpPurpleEntryCloud.Play();

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
		// Short beam sound
		Script_SlowMotionSound_triggered scr = transform.root.gameObject.GetComponent<Script_SlowMotionSound_triggered> ();
		scr.playSound2();

		// Player references
		GameObject opponent = (atRight) ? ship : ship2;

		if(opponent != null)
		{
			// Effect on shot-start
			bulletPowerUpPurpleEntryBlast.Play();
			bulletPowerUpPurpleBeamAcross.Play();

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
				ParticleSystem gObj = (Instantiate(bulletPowerUpPurpleEntryBlast, position, new Quaternion())) as ParticleSystem;
				gObj.GetComponent<ParticleSystem>().Play();

				// Sound
				//AudioSource.PlayClipAtPoint (sfxShot, GameObject.Find("Main Camera").GetComponent<Transform>().position);
				
				// Beam Across (Shown in front of ship and towards edge)
				ParticleSystem gObj2 = (Instantiate(bulletPowerUpPurpleBeamAcross, new Vector3(position.x + ((atRight) ? (8.5f) : -(8.5f)), position.y + 1.0f, 0), new Quaternion())) as ParticleSystem;
				gObj2.GetComponent<ParticleSystem>().Play();
			}
			
			// Hit or no hit
			if(hit)
			{
				// Spawn Pfx on hit and push back
				GameObject gObj = (Instantiate((atRight) ? bulletPowerUpPurpleBeamLeft : bulletPowerUpPurpleBeamRight, new Vector3(oppoPos.x + ((atRight) ? -(8.5f) : (8.5f)), position.y + 1.0f, 0), new Quaternion())) as GameObject;
				gObj.GetComponent<ParticleSystem>().Play();
				gObj.GetComponent<ParticleSystem>().Emit(150);
//				GameObject pfxHit = Instantiate(bulletPowerUpPurpleHit, oppoPos, new Quaternion()) as GameObject;
//				if(atRight == false) pfxHit.transform.Rotate(new Vector3(0,0,Random.Range(130, 180)));
				
				// Shake screen and push player back
				opponent.transform.FindChild("Ship").gameObject.GetComponent<MeshExploder>().Explode();
				Destroy(opponent.gameObject);
				GameObject.Find("TheBlackhole").GetComponent<BlackHoleScript>().doTheEnd(opponent.gameObject, opponent.gameObject.transform.position);
				GameObject.Find("TheBlackhole").GetComponent<BlackHoleScript>().deathByLasor(opponent.gameObject);
				//opponent.GetComponent<Rigidbody>().AddForce (((opponent.name=="Ship") ? pushBackForce : -pushBackForce),0,0);
				//Camera.main.GetComponent<Animation>().Play("AsteroidsShake");
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
