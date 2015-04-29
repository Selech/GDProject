using UnityEngine;
using System.Collections;

public class PowerUpAbsorb : MonoBehaviour {

	private ParticleSystem ps;

	// Use this for initialization
	void Start () 
	{
		ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.transform.Rotate (0f,0f,-3f);

		if(ps)
		{
			if(!ps.IsAlive())
			{
				Destroy(transform.root.gameObject);
			}
		}
	}
}
