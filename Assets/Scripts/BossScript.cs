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
    
    public RuntimeAnimatorController bossAnimator;
    public Animator animator;
    private string state;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetAnimator(string leave)
    {
        if (leave != "idle")
            animator.SetBool("idle", false);
        if (leave != "attacking_horizontal")
            animator.SetBool("attacking_horizontal", false);
        if (leave != "attacking_vertical")
            animator.SetBool("attacking_vertical", false);
        if (leave != "attacking_orbGust")
            animator.SetBool("attacking_orbGust", false);
        if (leave != "attacking_orbFlare")
            animator.SetBool("attacking_orbFlare", false);
        if (!animator.GetBool(leave))
            animator.SetBool(leave, true);
        state = leave;
    }

    void FixedUpdate()
    {

    }

    //void Move(float distance, float time)
    //{
    //    movement = (distance, time, distance / time);
    //}
}
