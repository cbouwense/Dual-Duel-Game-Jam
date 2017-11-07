using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceOpponent : MonoBehaviour {
    
    GameObject opp;
    SpriteRenderer sr;
    
	void Start () {

        string oppName = (name == "Player1" ? "Player2" : "Player1");
        opp = GameObject.Find(oppName);
        sr = GetComponent<SpriteRenderer>();

    }
	
	void Update () {
		
        // If your opponent is on your left
        if (opp.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

	}
}
