using UnityEngine;
using System.Collections;

public class Script_Rotator : MonoBehaviour {

	public float xRotation;
	public float yRotation;
	public float zRotation;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(new Vector3(xRotation,yRotation,zRotation));	
	}
}
