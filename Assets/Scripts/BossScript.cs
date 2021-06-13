using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    private Transform t;

    private (float distance, float time, float velocity) movement;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

    }

    void Move(float distance, float time)
    {
        movement = (distance, time, distance / time);
    }
}
