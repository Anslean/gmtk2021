using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorScript : MonoBehaviour
{
    private bool nearby = false;
    private Character activeCharacter;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            activeCharacter = (Character)(((int)activeCharacter + 1) % 3);
        }
        if (Input.GetKeyDown(KeyCode.S) && nearby)
        {
            if (gameObject.tag == "magic" && activeCharacter == Character.Steven)
            {
                transform.GetChild(0).GetComponent<PlatformScript>().activate();
            }
            else if (gameObject.tag == "sturdy" && activeCharacter == Character.LorgeBoi)
            {
                transform.GetChild(0).GetComponent<PlatformScript>().activate();
            }
            else if (gameObject.tag == "tricky" && activeCharacter == Character.SmolBoi)
            {
                transform.GetChild(0).GetComponent<PlatformScript>().activate();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "body")
        {
            nearby = true;
            activeCharacter = col.GetComponent<GolemController>().character;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "body")
        {
            nearby = false;
        }
    }
}
