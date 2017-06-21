using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public GameObject bulletPrefab;
    public Transform target;
    public bool justShootForward;
    public float bulletSpeed = 30;
    public int bulletsPerShot = 4;
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
            clone.GetComponent<Rigidbody>().velocity = (justShootForward ? transform.forward : (target.position - transform.position).normalized) * bulletSpeed;
            clone.transform.forward = clone.GetComponent<Rigidbody>().velocity.normalized;
            loop++;

            if (loop % bulletsPerShot != 0 || (GetComponent<Pokable>() != null && GetComponent<Pokable>().IsPoked))
            {
                time = 0.2f;
            }
            else
            {
                time = 2;
            }
        }

        time -= Time.deltaTime;
	}
}
