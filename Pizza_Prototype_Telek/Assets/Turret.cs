using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public GameObject bulletPrefab;
    public Transform target;

    float time = 0;

    int loop = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (time < 0)
        {
            GameObject clone = Instantiate(bulletPrefab);
            clone.transform.position = transform.position;
            clone.GetComponent<Rigidbody>().velocity = (target.position - transform.position).normalized * 30;
            clone.transform.forward = clone.GetComponent<Rigidbody>().velocity.normalized;
            loop++;

            if (loop % 4 == 0)
            {
                time = 2;
            }
            else
            {
                time = 0.2f;
            }
        }

        time -= Time.deltaTime;
	}
}
