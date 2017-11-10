using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontTouchMe : MonoBehaviour {

    GameObject opp;
    PlayerController myPC, oppPC;
    public LayerMask wallLayer;

    // Use this for initialization
    void Start () {
        string oppName = (name == "Player (1)" ? "Player (2)" : "Player (1)");
        opp = GameObject.Find(oppName);
        myPC = GetComponent<PlayerController>();
        oppPC = opp.GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 myPos = transform.position;
        if (myPos.x < -8.6f)
            transform.position = new Vector2(-8.5f, myPos.y);
        /*
        Vector2 oppPos = opp.transform.position;
        // Too close horizontally
        //Debug.Log("horiztonal space: " + Mathf.Abs(myPos.x - oppPos.x));
        if (Mathf.Abs(myPos.x - oppPos.x) < 1.424f)
        {
            // Check that we aren't going to go into a wall
            Vector2 dir;
            if (transform.localScale.x > 0)
            {
                dir = Vector2.left;
            }
            else
            {
                dir = Vector2.right;
            }
            RaycastHit2D back = Physics2D.Raycast(transform.position, dir, 3, wallLayer);
            if (back.collider != null)
                Debug.Log(name + ": colliding with " + back.collider.name);
            if (back.collider.name != "Platform (1)" && back.collider.name != "Platform (2)")
            {
                if (transform.localScale.x > 0)
                {
                    Debug.Log("moving " + name + "left");
                    transform.position = new Vector2(myPos.x - 0.3f, myPos.y);
                }
                    
                else
                {
                    Debug.Log("moving " + name + "right");
                    transform.position = new Vector2(myPos.x + 0.3f, myPos.y);
                }
                    
            }
            else
            {
                Debug.Log("colliding with " + back.collider.name);
            }
            Debug.Log("working");
        }
        while (Mathf.Abs(myPos.y - oppPos.y) < 2.9682f)
        {

        }
        */
    }

}
