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

    [SerializeField] private float health = 100;

    private PlayerController pc;
    
    public void Injure(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void HitStun(float hitstun)
    {
        Debug.Log("setting " + name + "'s hitstun to " + hitstun);
        hitStunLeft = hitstun;
    }

    public void HitStop(float hitstop)
    {
        hitStopLeft = hitstop;
    }

    // Set knockback for when hitstun is over
    public void PrimeKnockBack(Vector2 knockback)
    {
        currentKnockBack = knockback;
    }

    private void Die()
    {
        // TODO: Opponent wins
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
        pc = GetComponent<PlayerController>();
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
            
        if (hitStopLeft > 0) hitStopLeft -= Time.deltaTime;
    }

}
