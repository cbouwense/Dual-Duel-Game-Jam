using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceOpponent : MonoBehaviour {
    
    GameObject opp;
    
	void Start () {

        string oppName = (name == "Player (1)" ? "Player (2)" : "Player (1)");
        opp = GameObject.Find(oppName);

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
