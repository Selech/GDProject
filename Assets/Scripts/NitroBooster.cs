using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NitroBooster : MonoBehaviour {

	public Transform nitroParticle;
	public KeyCode nitroBtnLeft;
	public KeyCode nitroBtnRight;

	public float force;

	public GameObject playerLeft;
	public GameObject playerRight;

	private Image nitroBar;
	private AudioSource sound;
	
//	private int fillLeft = 1;
//	private int fillRight = 1;

	private float nitroLoss = 0.05f;

	private bool allowNitro = true;

	// Use this for initialization
	void Start () {
		nitroBar = GetComponent<Image> ();
		sound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (nitroBar.fillAmount == 1)
						allowNitro = true;
				else if (nitroBar.fillAmount < 0.03) {
						allowNitro = false;
						sound.Stop();
				}

		if (Input.GetKey(nitroBtnLeft) && allowNitro) 
		{
			// Nitro Bar
			nitroBar.fillAmount -= nitroLoss;

			// Movement of player
			var particles = Instantiate(nitroParticle.gameObject, new Vector3(playerLeft.transform.position.x + 0.4f, 
			                                                               playerLeft.transform.position.y, 
			                                                               playerLeft.transform.position.z), 
			          new Quaternion());

			playerLeft.GetComponent<Rigidbody>().AddForce(new Vector3(-force, 0f, 0f),ForceMode.Force);

			// Destroying particles
			Destroy(particles, 0.05f);
		}

		if(Input.GetKeyDown(nitroBtnLeft) && allowNitro) {
			sound.Play();
		}
		   
		if(Input.GetKeyUp(nitroBtnLeft)) {
			sound.Stop();
		}

		nitroBar.fillAmount += 0.01f;
	}
}
