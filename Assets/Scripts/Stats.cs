using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {
    
    private float walkingSpeed;
    private float dashingSpeed;
    private float jumpSpeed;

    [SerializeField] private float hitStunLeft = 0.0f;
    [SerializeField] private float hitStopLeft = 0.0f;
    [SerializeField] private Vector2 currentKnockBack = new Vector2();
    [SerializeField] private float prevHitStun = 0.0f;
    [SerializeField] private float prevHitStop = 0.0f;

    [SerializeField] private float health = 300;

    private Transform block;
    private PlayerController pc;
    private Animator anim;
    private RoomController rc;
    private SpriteRenderer sr;

    public void Injure(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void HitStun(float hitstun, bool playAnim)
    {
        if (playAnim)
            anim.SetBool("hit", true);
        //Debug.Log("setting " + name + "'s hitstun to " + hitstun);
        hitStunLeft = hitstun;
    }

    public void HitStop(float hitstop)
    {
        hitStopLeft = hitstop;
    }

    public bool isBlocking(string attack)
    {
        //Debug.Log("Checking against " + attack);
        if (attack == "std_light" || attack == "std_medium" || attack == "std_heavy" ||
            attack == "air_light" || attack == "air_medium" || attack == "air_heavy")
        {
            // If we are walking backwards
            if ((transform.localScale.x < 0 && pc.right && !pc.down) ||
                (transform.localScale.x > 0 && pc.left && !pc.down))
            {
                sr.enabled = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if ((transform.localScale.x < 0 && pc.down && pc.right) ||
                (transform.localScale.x > 0 && pc.down && pc.left))
            {
                //changeAnim("low_block");
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    // Set knockback for when hitstun is over
    public void PrimeKnockBack(Vector2 knockback)
    {
        currentKnockBack = knockback;
    }

    private void Die()
    {
        string oppName = (name == "Player (1)" ? "Player (2)" : "Player (1)");
        rc.Winrar(oppName);
    }

    public bool Actionable()
    {
        //Debug.Log("Actionable: " + (hitStunLeft <= 0 && hitStopLeft <= 0));
        return (hitStunLeft <= 0 && hitStopLeft <= 0);
    }

    public void setWalkingSpeed(float speed)
    {
        walkingSpeed = speed;
    }
    public void setDashingSpeed(float speed)
    {
        dashingSpeed = speed;
    }
    public void setJumpSpeed(float speed)
    {
        jumpSpeed = speed;
    }
    public void setHealth(float hp)
    {
        health = hp;
    }

    public float getWalkingSpeed(string dir)
    {
        if (dir == "right")
        {
            return walkingSpeed;
        }
        return -walkingSpeed;
    }
    public float getDashingSpeed(string dir)
    {
        if (dir == "right")
        {
            return dashingSpeed;
        }
        return -dashingSpeed;
    }
    public float getJumpSpeed()
    {
        return jumpSpeed;
    }

    private void Start()
    {
        GameObject roomManager = GameObject.Find("RoomManager");
        block = transform.GetChild(0);
        rc = roomManager.GetComponent<RoomController>();
        pc = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        sr = block.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

        if (hitStunLeft > 0)
        {
            prevHitStun = hitStunLeft;
            hitStunLeft -= Time.deltaTime;
        }
        else if (prevHitStun > 0)
        {
            pc.KnockBack(currentKnockBack);
            prevHitStun = 0;
        }

        if (hitStopLeft > 0)
        {
            prevHitStop = hitStopLeft;
            hitStopLeft -= Time.deltaTime;
        }
        else if (prevHitStop > 0)
        {
            pc.KnockBack(currentKnockBack);
            prevHitStop = 0;
        }
    }

    private void changeAnim(string state)
    {
        string[] states = {"idle", "dash forward", "dash backward",
                           "walk forward", "walk backward",
                           "crouching", "jumping", "air",
                           "std_block", "low_block",
                           "std_light", "std_medium", "std_heavy",
                           "air_light", "air_medium", "air_heavy"};
        for (int i = 0; i < states.Length; i++)
        {
            if (state == states[i])
            {
                anim.SetBool(states[i], true);
            }
        }
    }

}

