using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {
    public float time = 10;
    float currTime = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if ((Catcher.instance.transform.position - transform.position).magnitude > 250)
        //{
            currTime += Time.deltaTime;

            if (currTime > time)
                Destroy(gameObject);
       // }
	}
}
