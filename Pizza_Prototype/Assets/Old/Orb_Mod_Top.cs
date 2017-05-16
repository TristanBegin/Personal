using UnityEngine;
using System.Collections;

public class Orb_Mod_Top : Orb_Mod {


	// Update is called once per frame
	void Update () {
		
		if (PlayerAttached && Input.GetAxis("RightTrigger") < -0.2f)
		{
			Orb.GetComponent<Rigidbody>().velocity = new Vector3 (Orb.GetComponent<Rigidbody>().velocity.x, 22, Orb.GetComponent<Rigidbody>().velocity.z); 
		}

	}
}
