using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static Character;

public class GolemController : MonoBehaviour
{
    public float jumpHeight;
    public float walkSpeed, fallSpeed;
    public float attackDuration, attackCooldown;

    public float dashSpeed, dashDuration, dashCooldown;
    public float groundPoundSpeed;

    public Character character;
    public RuntimeAnimatorController LorgeBoiAnimator, SmolBoiAnimator, StevenAnimator;

    public Animator animator;
    private string state;

    private (bool active, bool available, bool aerial, float progress, float height) jump;
    private (bool active, bool available) ability = (false, true);
    private (float direction, float progress, float speed, float duration, float cooldown) dash;
    private (bool active, bool available, float progress, float duration, float cooldown) attack;

    private Rigidbody2D rb;
    private Transform t;
    private int trySnag = 0;
    private bool invincibleToBoss = false;

    public InGameUIScript inGameUI;

    public int deathY;
    private bool died = false;

    [HideInInspector]
    public bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        t = rb.transform;

        jump = (false, true, false, -1, jumpHeight);
        dash = (1, -1, dashSpeed, dashDuration, dashCooldown);
        attack = (false, true, -1, attackDuration, attackCooldown);
        animator.runtimeAnimatorController = (character == Steven ? StevenAnimator : (character == LorgeBoi ? LorgeBoiAnimator : SmolBoiAnimator));

        if (inGameUI != null)
            UpdateCharacterLabel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            character = (Character)(((int)character + 1) % 3);
            ability.active = false;
            ability.available = true;
            animator.runtimeAnimatorController = (character == Steven ? StevenAnimator : (character == LorgeBoi ? LorgeBoiAnimator : SmolBoiAnimator));
            if (state != "dash" && state != "djump" && state != "ground_pound")
            {
                SetAnimator(state);
            }
            if (inGameUI != null)
                UpdateCharacterLabel();
        }

        if (Input.GetKeyDown(KeyCode.W) && attack.progress <= 0)
        {
            attack.active = true;
            attack.progress = attack.duration + attack.cooldown;
        }

        if (Input.GetButton("Jump") && Time.timeScale > 0.0f)
        {
            if (!jump.aerial && jump.available)
            {
                jump.available = false;
                jump.active = true;
                jump.progress = jump.height;
            }
            else
            {
                jump.progress--;
            }
        }
        else
        {
            jump.progress = -1;
        }

        if (Input.GetKeyDown(GetAbilityKey()))
        {
            switch (character)
            {
                case LorgeBoi:
                    ability.active |= jump.aerial && ability.available;
                    break;
                case SmolBoi:
                    ability.active = true;
                    dash.progress = (dash.progress <= 0) ? dash.duration + dash.cooldown : dash.progress;
                    break;
                case Steven:
                    ability.active |= jump.aerial && ability.available;
                    break;
            }
        }

        jump.available = (Input.GetButtonUp("Jump") || jump.available) && Time.timeScale > 0.0f;
        float horizontal = (Input.GetAxis("Horizontal") < -0.1f ? -walkSpeed : 0) + (Input.GetAxis("Horizontal") > 0.1f ? walkSpeed : 0);
        float vertical = (jump.active && jump.progress > 0) ? jumpHeight : (rb.velocity.y < -fallSpeed ? -fallSpeed : rb.velocity.y);
        rb.velocity = new Vector2(horizontal, vertical);
        jump.active &= (jump.progress >= 1);
        dash.direction = (rb.velocity.x > 0 ? 1 : (rb.velocity.x < 0 ? -1 : dash.direction));
        if (dash.direction == 1)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
        }

        if (ability.active)
        {
            switch (character)
            {
                case LorgeBoi:
                    rb.velocity = new Vector2(0, -groundPoundSpeed);
                    isAttacking = true;
                    ability.available = false;
                    break;
                case SmolBoi:
                    rb.velocity = (dash.progress > dash.cooldown) ? new Vector2(dash.speed * dash.direction, 0) : rb.velocity;
                    ability.active = dash.progress > 0;
                    ability.available = dash.progress <= 0;
                    dash.progress--;
                    break;
                case Steven:
                    ability.available = false;
                    ability.active = false;
                    jump.progress = jumpHeight;
                    jump.active = true;
                    break;
            }
        }

        if (attack.active)
        {
            attack.active = attack.progress > 0;
            attack.available = attack.progress <= 0;
            attack.progress--;
            isAttacking = attack.progress > attack.cooldown;
            GetComponent<SpriteRenderer>().color = !isAttacking ? Color.white : (character == Steven ? Color.cyan : (character == LorgeBoi ? Color.yellow : Color.magenta));
        }

        // Kill player if they fall too far
        if (transform.position.y < deathY)
            Die();
    }

    void FixedUpdate()
    {
        if ((attack.active && character == LorgeBoi) || !attack.available)
        {
            SetAnimator("attack");
        }
        else if (rb.velocity.x > walkSpeed + 0.1 || rb.velocity.x < -walkSpeed - 0.1)
        {
            SetAnimator("dash");
            trySnag = 0;
        }
        else if (character == LorgeBoi && ability.active == true)
        {
            SetAnimator("ground_pound");
        }
        else if (jump.aerial)
        {
            if (rb.velocity.y > -fallSpeed / 2)
            {
                if (character == Steven && !ability.available)
                    SetAnimator("djump");
                else if (rb.velocity.y == 0)
                {
                    if (trySnag > 0)
                    {
                        SetAnimator("snag");
                    }
                    else
                    {
                        trySnag++;
                    }
                }
                else
                    SetAnimator("jump");
            }
            else
            {
                if (trySnag == 2)
                    SetAnimator("fall");
            }
        }
        else if (rb.velocity.x > 0.1 || rb.velocity.x < -0.1)
        {
            SetAnimator("walk");
        }
        else
        {
            SetAnimator("idle");
        }
    }

    KeyCode GetAbilityKey()
    {
        KeyCode key = KeyCode.A;
        switch (character)
        {
            case LorgeBoi:
                key = KeyCode.S;
                break;
            case SmolBoi:
                key = KeyCode.LeftShift;
                break;
            case Steven:
                key = KeyCode.Space;
                break;
        }
        return key;
    }

    public void makeInvincibleToBoss()
    {
        invincibleToBoss = true;
    }

    public bool isInvincibleToBoss()
    {
        return invincibleToBoss;
    }

    void SetAnimator(string leave)
    {
        if (leave != "idle")
            animator.SetBool("idle", false);
        if (leave != "walk")
            animator.SetBool("walk", false);
        if (leave != "jump")
            animator.SetBool("jump", false);
        if (leave != "fall")
            animator.SetBool("fall", false);
        if (leave != "djump" && character == Steven)
            animator.SetBool("djump", false);
        if (leave != "dash" && character == SmolBoi)
            animator.SetBool("dash", false);
        if (leave != "ground_pound" && character == LorgeBoi)
            animator.SetBool("ground_pound", false);
        if (leave != "attack")
            animator.SetBool("attack", false);
        if (leave != "snag")
            animator.SetBool("snag", false);
        if (!animator.GetBool(leave))
            animator.SetBool(leave, true);
        state = leave;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.otherCollider.tag == "foot")
        {
            if (jump.aerial && col.collider.tag == "ground")
            {
                jump.aerial = false;
                jump.active = false;
                if (character == Character.LorgeBoi)
                {
                    ability.available = true;
                    ability.active = false;
                }
                isAttacking = false;
                invincibleToBoss = false;
            }
        }
        else if (col.otherCollider.tag == "head")
        {
            jump.progress = -1;
        }
        else if (dash.progress > dash.cooldown)
        {
            dash.progress = dash.cooldown;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.otherCollider.tag == "foot" && col.collider.tag == "ground")
        {
            jump.aerial = true;
            switch (character)
            {
                case LorgeBoi:
                    ability.available = true;
                    ability.active = false;
                    break;
                case SmolBoi:
                    break;
                case Steven:
                    ability.available = true;
                    break;
            }
        }
    }

    // Update the UI label based on the current character
    void UpdateCharacterLabel()
    {
        switch (character)
        {
            case Character.SmolBoi:
                inGameUI.SetCharacterText("Bree (Dash)", Color.cyan);
                break;
            case Character.Steven:
                inGameUI.SetCharacterText("Ellistair (Double Jump)", Color.yellow);
                break;
            case Character.LorgeBoi:
                inGameUI.SetCharacterText("Vellsua (Ground-pound)", Color.magenta);
                break;
        }
    }

    // Player death (show a death message and reload the scene)
    public void Die()
    {
        if (!died)
        {
            died = true;
            GetComponent<DialogueScript>().ShowDialogueAndDie(1, 2);
        }
    }
}