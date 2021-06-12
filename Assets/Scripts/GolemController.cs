using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Character;

public class GolemController : MonoBehaviour
{
    public float jumpHeight;
    public float walkSpeed, fallSpeed;

    public float dashSpeed, dashDuration, dashCooldown;
    public float groundPoundSpeed;

    public Character character;

    private (bool active, bool available, bool aerial, float progress, float height) jump;
    private (bool active, bool available) ability = (false, true);
    private (float direction, float progress, float speed, float duration, float cooldown) dash;
    private ((int count, Collider2D nearest) magic, (int count, Collider2D nearest) sturdy, (int count, Collider2D nearest) tricky) objects;

    private Rigidbody2D rb;
    private Transform t;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        t = rb.transform;
        Debug.Log(character);

        jump = (false, true, false, -1, jumpHeight);
        dash = (1, -1, dashSpeed, dashDuration, dashCooldown);
        objects = ((0, new Collider2D()), (0, new Collider2D()), (0, new Collider2D()));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            character = (Character)(((int)character + 1) % 3);
        }

        if (Input.GetButton("Jump"))
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

        jump.available = Input.GetButtonUp("Jump") || jump.available;
        float horizontal = (Input.GetAxis("Horizontal") < -0.1f ? -walkSpeed : 0) + (Input.GetAxis("Horizontal") > 0.1f ? walkSpeed : 0);
        float vertical = (jump.active && jump.progress > 0) ? jumpHeight : (rb.velocity.y < -fallSpeed ? -fallSpeed : rb.velocity.y);
        rb.velocity = new Vector2(horizontal, vertical);
        jump.active &= (jump.progress >= 1);
        dash.direction = (rb.velocity.x > 0 ? 1 : (rb.velocity.x < 0 ? -1 : dash.direction));

        if (ability.active)
        {
            switch (character)
            {
                case LorgeBoi:
                    rb.velocity = new Vector2(0, -groundPoundSpeed);
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "magic")
        {
            objects.magic.count++;
            objects.magic.nearest = col;
        }
        else if (col.tag == "sturdy")
        {
            objects.sturdy.count++;
            objects.sturdy.nearest = col;
        }
        else if (col.tag == "tricky")
        {
            objects.tricky.count++;
            objects.tricky.nearest = col;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "magic")
        {
            objects.magic.count--;
        }
        else if (col.tag == "sturdy")
        {
            objects.sturdy.count--;
        }
        else if (col.tag == "tricky")
        {
            objects.tricky.count--;
        }
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
}