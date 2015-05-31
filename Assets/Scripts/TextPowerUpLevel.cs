using UnityEngine;
using System.Collections;

public class TextPowerUpLevel : MonoBehaviour 
{
	float maxSize 		= 1.2f;
	bool isBig 			= false;

	// Update is called once per frame
	void Update () 
	{
		// Check if big
		if(transform.localScale.x > maxSize - 0.01f)
		{
			isBig = true;
		}

		// Scale Up
		if(isBig == false)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxSize, maxSize, maxSize), 0.15f * Time.timeScale);
		}
		// Scale Down
		else if (transform.localScale.x > 0.1f)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 0), 0.1f * Time.timeScale);
		}
		else
		{
			Destroy(transform.root.gameObject);
		}
	}
}
