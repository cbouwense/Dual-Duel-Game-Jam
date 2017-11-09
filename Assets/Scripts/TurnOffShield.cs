using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffShield : MonoBehaviour {

    SpriteRenderer sr;
    [SerializeField] float enabledFrames = 0;

	void Start () {
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        if (sr.enabled && enabledFrames == 0)
            enabledFrames = 6;
        else if (enabledFrames > 0)
            enabledFrames--;

        if (enabledFrames == 0)
            sr.enabled = false;
	
    }
}
