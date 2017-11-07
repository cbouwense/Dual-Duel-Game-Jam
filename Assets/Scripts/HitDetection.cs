using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{

    GameObject opp;

    void Start()
    {
        string oppName = (name == "Player1" ? "Player2" : "Player1");
        opp = GameObject.Find(oppName);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.transform.root.GetComponent<Stats>().Hittable());
        if (col.gameObject.tag == "Hurtbox" && col.transform.root.GetComponent<Stats>().Hittable())
        {
            int damage = GetComponent<HitboxStats>().getDamage();
            Debug.Log("hit " + opp.name);
            Debug.Log("sending " + damage + " damage");
            col.transform.root.GetComponent<Stats>().Injure(damage);
        }
    }

}