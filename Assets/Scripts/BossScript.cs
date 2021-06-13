using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    //private Transform t;

    //private (float distance, float time, float velocity) movement;

    /*
     * Logic:
     * There's 4 seconds between attacks.
     * The first phase of the boss can do a horizontal, vertical, or orb attack (horizontal or vertical feathers, or gust).
     * There's a 60/40% split between feather attacks and gust attacks respectively (30/30/40).
     * Boss takes 8 hits to beat. He can be damaged only during the 4 seconds when he's not attacking, by attacking the orb in his chest.
     * Once the boss hits 1/2 or 1/3 hp, he adds his special orb attack into the mix (flare) and uses it once immediately (with a small charge-up).
     * The new move frequency becomes 20/20/30/30 (horizontal/vertical/gust/flare).
     * Play animation when boss dies.
     */
    public int test1 = 1;
    public int test2 = 2;
    public int test3 = 3;
    public int test4 = 4;
    public int test5 = 5;

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

    //void Move(float distance, float time)
    //{
    //    movement = (distance, time, distance / time);
    //}
}
