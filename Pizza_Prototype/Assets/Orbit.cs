using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {
    public Transform target;
    public float frequency = 1;
    public float amplitude = 10;
    public float height = 0;

    float time;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;
        transform.position = target.position + new Vector3(Mathf.Sin(time * frequency) * amplitude, height, Mathf.Cos(time * frequency) * amplitude);
	}
}
