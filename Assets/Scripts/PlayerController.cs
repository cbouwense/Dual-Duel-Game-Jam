using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class PlayerController : PhysicsObject
{

    [SerializeField] private bool crouching;
    
    private SpriteRenderer sr;
    private Stats stats;

    private enum State { idle, dashing, walking, crouching, jumping, air,
                         std_att, air_att, low_att }
    private State newState = State.idle;
    private State prevState = State.idle;
    private State state = State.idle;

    private bool left, right, jump, down, dash, changeStance,
                 prevLeft, prevRight, light_att, medium_att, heavy_att,
                 prev_light, prev_medium, prev_heavy;

    private float dashTimer = 0;
    private float jumpTimer = 0;
    private bool dashingLeft;
    private int dashInputFrames = 10;
    private int dashLeftInputFrames = 0;
    private int dashRightInputFrames = 0;

    protected override void Start()
    {
        base.Start();

        Application.targetFrameRate = 60;
        
        sr = GetComponent<SpriteRenderer>();
        stats = GetComponent<Stats>();

        stats.setWalkingSpeed(4);
        stats.setDashingSpeed(8);
        stats.setJumpSpeed(10);
    }

    protected override void Update()
    {
        GamePad.Index index = (name == "Player1" ? GamePad.Index.One: GamePad.Index.Two);

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

        // Reset animations
        resetAnim();
        
        if (moveable)
        {
            // Character state machine
            switch (state)
            {
                case State.idle:
                    //Debug.Log(name + "idle");
                    changeAnim("idle");

                    velocityX = 0;

                    // Transition logic
                    if (dash)
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
                    //Debug.Log(name + " walking");

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
                    //Debug.Log(name + " crouching");
                    changeAnim("crouching");

                    velocityX = 0;

                    if (light_att || medium_att || heavy_att)
                        newState = State.low_att;
                    else if (down)
                        newState = State.crouching;
                    else
                        newState = State.idle;
                    
                    break;

                case State.jumping:
                    //Debug.Log(name + " jumping");
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
                    //Debug.Log(name + " air");
                    changeAnim("air");

                    // If we just landed
                    if (!wasGrounded && grounded)
                    {
                        if (left || right)
                            newState = State.walking;
                        else
                            newState = State.idle;
                            
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
                    //Debug.Log(name + " dashing");

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
                        if (name == "Player2") //Debug.Log("started dash timer");
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
                        changeAnim("std_light");

                    }
                    else if (prev_medium)
                    {
                        changeAnim("std_medium");

                    }
                    else if (prev_heavy)
                    {
                        changeAnim("std_heavy");

                    }

                    if (grounded)
                        newState = State.idle;
                    else
                        newState = State.air;

                    break;

                case State.low_att:
                    //Debug.Log(name + " std_heavy");

                    if (prev_light)
                    {
                        changeAnim("low_light");

                    }
                    else if (prev_medium)
                    {
                        changeAnim("low_medium");

                    }
                    else if (prev_heavy)
                    {
                        changeAnim("low_heavy");

                    }

                    if (grounded)
                        newState = State.idle;
                    else
                        newState = State.air;

                    break;

                case State.air_att:
                    //Debug.Log(name + " std_heavy");

                    if (prev_light)
                    {
                        changeAnim("air_light");

                    }
                    else if (prev_medium)
                    {
                        changeAnim("air_medium");

                    }
                    else if (prev_heavy)
                    {
                        changeAnim("air_heavy");

                    }

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

    }

    protected override void ComputeVelocity()
    {

        if (moveable)
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

    }

    private void changeAnim(string state)
    {
        string[] states = {"idle", "dash forward", "dash backward",
                           "walk forward", "walk backward",
                           "crouching", "jumping", "air",
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

    private void resetAnim()
    {
        string[] states = {"idle", "dash forward", "dash backward",
                           "walk forward", "walk backward",
                           "crouching", "jumping", "air",
                           "std_light", "std_medium", "std_heavy",
                           "air_light", "air_medium", "air_heavy"};
        for (int i = 0; i < states.Length; i++)
        {
            anim.SetBool(states[i], false);
        }
    }

}