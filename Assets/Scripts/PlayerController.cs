using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class PlayerController : PhysicsObject
{

    [SerializeField] private bool crouching;
    private GameObject opp;
    private HitboxStats hs;
    private RoomController rc;
    private BoxCollider2D bc;

    private enum State { idle, dashing, walking, crouching, jumping, air,
                         std_att, air_att, low_att }
    private State newState = State.idle;
    private State prevState = State.idle;
    [SerializeField] private State state = State.idle;

    public bool left, right, jump, down, dash, changeStance,
                 prevLeft, prevRight, light_att, medium_att, heavy_att,
                 prev_light, prev_medium, prev_heavy, std_block, low_block;

    private float dashTimer = 0;
    private float jumpTimer = 0;
    private bool dashingLeft;
    private int dashInputFrames = 10;
    private int dashLeftInputFrames = 0;
    private int dashRightInputFrames = 0;

    protected override void Start()
    {
        base.Start();

        string oppName = (name == "Player (1)" ? "Player (2)" : "Player (1)");
        opp = GameObject.Find(oppName);

        Application.targetFrameRate = 60;
        
        hs = GetComponent<HitboxStats>();
        rc = GameObject.Find("RoomManager").GetComponent<RoomController>();
        bc = GetComponent<BoxCollider2D>();

        stats.setWalkingSpeed(4);
        stats.setDashingSpeed(10);
        stats.setJumpSpeed(10);
    }

    protected override void Update()
    {
        GamePad.Index index = (name == "Player (1)" ? GamePad.Index.One: GamePad.Index.Two);

        /*
        if (!grounded)
        {
            bc.isTrigger = true;
        }
        else
        {
            bc.isTrigger = false;
        }
        */
        prevLeft = left;
        prevRight = right;
        prev_light = light_att;
        prev_medium = medium_att;
        prev_heavy = heavy_att;

        left = GamePad.GetAxis(GamePad.Axis.Dpad, index).x < 0;
        right = GamePad.GetAxis(GamePad.Axis.Dpad, index).x > 0;
        jump = GamePad.GetAxis(GamePad.Axis.Dpad, index).y > 0 || GamePad.GetButtonDown(GamePad.Button.A, index);
        down = GamePad.GetAxis(GamePad.Axis.Dpad, index).y < 0;
        dash = GamePad.GetButtonDown(GamePad.Button.LeftShoulder, index) || 
               (left && !prevLeft && dashLeftInputFrames > 0) || 
               (right && !prevRight && dashRightInputFrames > 0);
        changeStance = GamePad.GetButtonDown(GamePad.Button.RightShoulder, index);
        light_att = GamePad.GetButtonDown(GamePad.Button.X, index);
        medium_att = GamePad.GetButtonDown(GamePad.Button.Y, index);
        heavy_att = GamePad.GetButtonDown(GamePad.Button.B, index);

        string attackName;

        // Reset animations
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("standing_hitstun"))
            resetAnim();

        //Debug.Log("state: " + rc.state);
        if (stats.Actionable() && rc.state == RoomController.RoomState.round)
        {
            //Debug.Log(name + ": " + state, this);
            // Character state machine
            switch (state)
            {
                case State.idle:
                    changeAnim("idle");

                    //Debug.Break();

                    if (prevState != State.idle)
                        velocityX = 0;

                    // Transition logic
                    if (!grounded)
                        newState = State.air;
                    else if (dash)
                        newState = State.dashing;
                    else if (right || left)
                        newState = State.walking;
                    else if (jump)
                        newState = State.jumping;
                    else if (down)
                        newState = State.crouching;
                    
                    if (light_att || medium_att || heavy_att)
                        newState = State.std_att;

                    break;

                case State.walking:

                    // Animation logic and setting speed
                    if (right)
                    {
                        if (transform.localScale.x < 0)
                        {
                            changeAnim("walk backward");
                        }
                        else
                        {
                            changeAnim("walk forward");
                        }
                        velocityX = stats.getWalkingSpeed("right");
                    }
                    else if (left)
                    {
                        if (transform.localScale.x < 0)
                        {
                            changeAnim("walk forward");
                        }
                        else
                        {
                            changeAnim("walk backward");
                        }
                        velocityX = stats.getWalkingSpeed("left");
                    }
                    
                    // Transition logic
                    if (dash)
                        newState = State.dashing;
                    else if (down)
                        newState = State.crouching;
                    else if (jump)
                        newState = State.jumping;
                    else if (right || left)
                        newState = State.walking;
                    else if (!(right || left))
                        newState = State.idle;
                    if (light_att || medium_att || heavy_att)
                        newState = State.std_att;

                    break;

                case State.crouching:
                    changeAnim("crouching");

                    if (prevState != State.crouching)
                        velocityX = 0;

                    if (!grounded)
                        newState = State.air;
                    if (light_att || medium_att || heavy_att)
                        newState = State.low_att;
                    else if (down)
                        newState = State.crouching;
                    else
                        newState = State.idle;
                    
                    break;

                case State.jumping:
                    changeAnim("jumping");

                    if (prevState != State.jumping)
                    {
                        jumpTimer = 0.05f;
                    }
                    
                    if (jumpTimer <= 0)
                    {
                        velocity.y = stats.getJumpSpeed();
                        newState = State.air;
                    }

                    break;

                case State.air:
                    changeAnim("air");

                    // If we just landed
                    if (!wasGrounded && grounded)
                    {
                        if (left || right)
                        {
                            newState = State.walking;
                        } 
                        else
                        {
                            newState = State.idle;
                        } 
                    }
                    else
                    {
                        if (dash)
                            newState = State.dashing;
                        else if (light_att || medium_att || heavy_att)
                            newState = State.air_att;
                        else
                            newState = State.air;
                    }

                    break;

                case State.dashing:

                    // Store which way we're dashing for later, and start dash timer
                    if (prevState != State.dashing)
                    {
                        if (left)
                        {
                            dashingLeft = true;
                        }
                        else if (right)
                        {
                            dashingLeft = false;
                        }
                        // If we're facing left
                        else if (transform.localScale.x < 0)
                        {
                            dashingLeft = false;
                        }
                        else
                        {
                            dashingLeft = true;
                        }
                        dashTimer = 0.2f;
                    }
                    // If the dashing timer has finished
                    else if (dashTimer <= 0)
                    {
                        if (grounded)
                        {
                            newState = State.idle;
                        }
                        else
                        {
                            if (dashingLeft)
                            {
                                velocityX = stats.getWalkingSpeed("left");
                            }
                            else
                            {
                                velocityX = stats.getWalkingSpeed("right");
                            }
                            newState = State.air;
                        }
                        break;
                    }

                    // Do the boost
                    if (dashingLeft)
                    {
                        velocityX = stats.getDashingSpeed("left");
                    }
                    else
                    {
                        velocityX = stats.getDashingSpeed("right");
                    }

                    // Animation logic
                    if (transform.localScale.x < 0)
                    {
                        if (dashingLeft)
                        {
                            changeAnim("dash forward");
                        }
                        else
                        {
                            changeAnim("dash backward");
                        }
                    }
                    else
                    {
                        if (dashingLeft)
                        {
                            changeAnim("dash backward");
                        }
                        else
                        {
                            changeAnim("dash forward");
                        }
                    }

                    break;

                case State.std_att:

                    if (prev_light)
                    {
                        attackName = "std_light";
                    }
                    else if (prev_medium)
                    {
                        attackName = "std_medium";
                    }
                    else
                    {
                        attackName = "std_heavy";
                    }

                    changeAnim(attackName);
                    hs.setStats(attackName);

                    if (grounded)
                        newState = State.idle;
                    else
                        newState = State.air;

                    break;

                case State.low_att:

                    if (prev_light)
                    {
                        attackName = "low_light";
                    }
                    else if (prev_medium)
                    {
                        attackName = "low_medium";
                    }
                    else
                    {
                        attackName = "low_heavy";
                    }

                    changeAnim(attackName);
                    hs.setStats(attackName);

                    if (grounded)
                        newState = State.idle;
                    else
                        newState = State.air;

                    break;

                case State.air_att:

                    if (prev_light)
                    {
                        attackName = "air_light";
                    }
                    else if (prev_medium)
                    {
                        attackName = "air_medium";
                    }
                    else
                    {
                        attackName = "air_heavy";
                    }

                    changeAnim(attackName);
                    hs.setStats(attackName);

                    if (grounded)
                        newState = State.idle;
                    else
                        newState = State.air;

                    break;
            }
            prevState = state;
            state = newState;
        }
        
        if (dashTimer > 0)
            dashTimer -= Time.deltaTime;
        if (jumpTimer > 0)
            jumpTimer -= Time.deltaTime;

        if (dashLeftInputFrames > 0)
            dashLeftInputFrames--;
        else if (left && dashLeftInputFrames <= 0)
            dashLeftInputFrames = dashInputFrames;

        if (dashRightInputFrames > 0)
            dashRightInputFrames--;
        else if (right && dashRightInputFrames <= 0)
            dashRightInputFrames = dashInputFrames;

        std_block = false;
        low_block = false;

    }

    protected override void ComputeVelocity()
    {
        if (stats.Actionable())
        {
            
            // Crouching
            if (down && grounded)
            {
                crouching = true;
            }
            else
            {
                crouching = false;
            }

            // Jumping
            if (jump && grounded)
            {
                velocityY = stats.getJumpSpeed();
            }

        }
        else
        {
            velocity = new Vector2(0, 0);
        }

    }

    public void KnockBack(Vector2 knockback)
    {
        if (transform.localScale.x < 0)
        {
            velocityX = knockback.x;
            velocity.y = knockback.y;
        }
        else
        {
            velocityX = -knockback.x;
            velocity.y = knockback.y;
        }
        anim.SetBool("hit", false);
    }

    private void changeAnim(string state)
    {
        string[] states = {"idle", "dash forward", "dash backward",
                           "walk forward", "walk backward",
                           "crouching", "jumping", "air",
                           "std_block", "low_block",
                           "std_light", "std_medium", "std_heavy",
                           "air_light", "air_medium", "air_heavy",
                           "low_light", "low_medium", "low_heavy"};
        for (int i = 0; i < states.Length; i++)
        {
            if (state == states[i])
            {
                anim.SetBool(states[i], true);
            }
        }
    }

    private void resetAnim()
    {
        string[] states = {"idle", "dash forward", "dash backward",
                           "walk forward", "walk backward",
                           "crouching", "jumping", "air",
                           "std_block", "low_block",
                           "std_light", "std_medium", "std_heavy",
                           "air_light", "air_medium", "air_heavy",
                           "low_light", "low_medium", "low_heavy"};
        for (int i = 0; i < states.Length; i++)
        {
            anim.SetBool(states[i], false);
        }
    }

}