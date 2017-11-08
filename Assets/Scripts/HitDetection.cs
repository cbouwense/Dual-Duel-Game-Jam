using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{

    GameObject opp;

    void Start()
    {
        string oppName = (name == "Player (1)" ? "Player (2)" : "Player (1)");
        opp = GameObject.Find(oppName);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Hurtbox")
        {
            Stats myStats = transform.root.GetComponent<Stats>();
            Stats oppStats = col.transform.root.GetComponent<Stats>();
            HitboxStats hStats = transform.root.GetComponent<HitboxStats>();

            int damage = hStats.getDamage();
            float hitstun = hStats.getHitStun();
            float hitstop = hStats.getHitStop();
            Vector2 knockback = hStats.getKnockBack();
            
            Debug.Log("hit " + opp.name);
            oppStats.Injure(damage);
            oppStats.HitStun(hitstun);
            myStats.HitStop(hitstop);
            oppStats.PrimeKnockBack(knockback);
        }
    }

}