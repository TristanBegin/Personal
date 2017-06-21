using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MovePattern : MonoBehaviour
{

    [Serializable]
    public class Move
    {
        public float time;
        public Vector3 velocity;
    }

    public Move[] moves;
    public Trigger myTrigger;

    int current;
    float currentTime = 0;

    // Update is called once per frame
    void Update ()
    {
        if (myTrigger == null || myTrigger.Active)
        {
            DoMove();
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
	}

    void DoMove()
    {
        currentTime += Time.deltaTime;
        if (currentTime > moves[current].time)
        {
            currentTime = 0;

            current++;
            if (current >= moves.Length)
                current = 0;
        }

        GetComponent<Rigidbody>().velocity = moves[current].velocity;
    }
}
