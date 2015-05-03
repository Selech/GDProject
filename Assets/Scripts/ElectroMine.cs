using UnityEngine;
using System.Collections;

public class ElectroMine : MonoBehaviour {

	public GameObject gObj_Rotator;
	private float gObj_RotateSpeed = 20.0f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		gObj_Rotator.transform.Rotate (Vector3.back, gObj_RotateSpeed * Time.deltaTime);
	}
}
