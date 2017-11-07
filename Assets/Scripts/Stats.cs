using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {
    
    private float walkingSpeed;
    private float dashingSpeed;
    private float jumpSpeed;

    private float health = 100;

    // TODO
    public bool Hittable()
    {
        return true;
    }

    public void Injure(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: Opponent wins
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

}
