using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
    public float speed = 1;
    public float x_amount = 0;
    public float y_amount = 1;
    public float z_amount = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.eulerAngles += new Vector3(x_amount * speed * Time.deltaTime, y_amount * speed * Time.deltaTime, z_amount * speed * Time.deltaTime);
	}
}
