using UnityEngine;
using System.Collections;

public class Reloading : MonoBehaviour {

//	bool isReady = false;
	Vector3 startPositionPfx1;
	Vector3 startPositionPfx2;
	public string type;

	GameObject pfx1Container;
	GameObject pfx2Container;
	ParticleSystem pfx1;
	ParticleSystem pfx2;

	float cooldDown { get; set; }

	bool atRight;

	// Use this for initialization
	void Start () 
	{
		pfx1Container = transform.FindChild ("pfx1").gameObject;
		pfx2Container = transform.FindChild ("pfx2").gameObject;
		
		startPositionPfx1 = pfx1Container.transform.localPosition;
		startPositionPfx2 = pfx2Container.transform.localPosition;

		pfx1 = pfx1Container.GetComponent<ParticleSystem> ();
		pfx2 = pfx2Container.GetComponent<ParticleSystem> ();

		// At right?
		atRight = Camera.main.WorldToScreenPoint (transform.root.gameObject.transform.position).x > Screen.width / 2;
	}

	bool allowSignalCooldown = false;
	public void reset()
	{
		pfx1Container.transform.localPosition = startPositionPfx1;
      	pfx1.Clear();
      	pfx1.Play();

		pfx2Container.transform.localPosition = startPositionPfx2;
		pfx2.Clear();
		pfx2.Play();

		cooldDown = 10;
		allowSignalCooldown = true;
	}

	// Update is called once per frame
	void Update () 
	{
		if(pfx1.IsAlive() == true)
		{
			if(type=="yellow") 		doYellow();
			else if (type=="blue") 	doBlue();
			else if (type=="purple")doPurple();

			// Ready Signal
			if(cooldDown < 0 && allowSignalCooldown == true)
			{
				allowSignalCooldown = false;

				GameObject obj = transform.FindChild("pfxReady").gameObject;
				ParticleSystem pSys = obj.GetComponent<ParticleSystem>();
				pSys.Play();
				pSys.loop = true;

				GameObject obj2 = transform.FindChild("pfxReady2").gameObject;
				ParticleSystem pSys2 = obj2.GetComponent<ParticleSystem>();
				pSys2.Play();
				pSys2.loop = true;
			}
			else if(cooldDown >= 0)
			{
				cooldDown -= Time.timeScale;
			}
		}
	}

	void doYellow ()
	{
		//transform.Rotate(new Vector3(10f,0,0));
		//Vector3 pos = new Vector3(transform.root.transform.position.x, 0, transform.root.transform.position.z);
		GameObject gObj = transform.FindChild("pfx1").gameObject;
		GameObject gObj2 = transform.FindChild("pfx2").gameObject;

		float multiplier = 3.2f;
		float speedX = 0.01f * multiplier * Time.timeScale;
		float speedY = 0.007f * multiplier * Time.timeScale;
		gObj.transform.localPosition -= new Vector3((atRight) ? -speedX : speedX, speedY, 0);
		gObj2.transform.localPosition -= new Vector3((atRight) ? -speedX : speedX, -speedY, 0);
	}

	void doBlue ()
	{
		//transform.Rotate(new Vector3(10f,0,0));
		//Vector3 pos = new Vector3(transform.root.transform.position.x, 0, transform.root.transform.position.z);
		GameObject gObj = transform.FindChild("pfx1").gameObject;
		GameObject gObj2 = transform.FindChild("pfx2").gameObject;
		
		float multiplier = 3.2f;
		float speedX = 0.01f * multiplier * Time.timeScale;
		float speedY = 0.007f * multiplier * Time.timeScale;
		gObj.transform.localPosition -= new Vector3((atRight) ? -speedX : speedX, speedY, 0);
		gObj2.transform.localPosition -= new Vector3((atRight) ? -speedX : speedX, -speedY, 0);


//		transform.Rotate(new Vector3(10f,0,0));
//		Vector3 pos = new Vector3(transform.root.transform.position.x, 0, transform.root.transform.position.z);
//		GameObject gObj = transform.FindChild("charge").gameObject;
//		gObj.transform.LookAt(transform.position);
//		gObj.transform.position += gObj.transform.forward * 1.2f;
		//= Vector3.Lerp(transform.position, pos, 0.2f * Time.timeScale);
	}

	void doPurple ()
	{
		//transform.Rotate(new Vector3(10f,0,0));
		//Vector3 pos = new Vector3(transform.root.transform.position.x, 0, transform.root.transform.position.z);
		GameObject gObj = transform.FindChild("pfx1").gameObject;
		GameObject gObj2 = transform.FindChild("pfx2").gameObject;
		
		float multiplier = 3.2f;
		float speedX = 0.01f * multiplier * Time.timeScale;
		float speedY = 0.007f * multiplier * Time.timeScale;
		gObj.transform.localPosition -= new Vector3((atRight) ? -speedX : speedX, speedY, 0);
		gObj2.transform.localPosition -= new Vector3((atRight) ? -speedX : speedX, -speedY, 0);
	}
}
