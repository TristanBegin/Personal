using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public LayerMask Triggers;

    bool Exploding = false;
    float StartScale;
    public float ExplosionScale = 5;
    float ExplosionTime = 0;
    float ExplotionSpeed = 1;

    void Update()
    {
        if (Exploding)
        {
            ExplosionTime += Time.deltaTime * ExplotionSpeed;
            float scale = StartScale + Mathf.Sin(ExplosionTime * Mathf.PI) * (ExplosionScale - StartScale);
            transform.localScale = Vector3.one * scale;

            if (ExplosionTime > 1)
                Destroy(gameObject);
        }
    }

	// Update is called once per frame
	void OnTriggerEnter (Collider hit) {
		if (Triggers == (Triggers | (1 << hit.gameObject.layer)))
        {
            Explode();
        }
	}

    void Explode()
    {
        Exploding = true;
        StartScale = transform.localScale.x;
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (GetComponent<Pokable>() != null)
        {
            GetComponent<Pokable>().Detach();
        }
    }
}
