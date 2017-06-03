using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSword : MonoBehaviour {
    
    float attackTime = 0;

    Vector3 OGscale;

	// Use this for initialization
	void Start () {
        OGscale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("X_Button"))
        {
            attackTime = 0.4f;
        }
        
        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
            transform.localScale = new Vector3(OGscale.x, OGscale.y, OGscale.z * 6);
            transform.Rotate(new Vector3(0, Time.deltaTime * 600, 0));
        }
        else
        {
            transform.localScale = OGscale;
            transform.forward = transform.parent.forward;
        }
    }
}
