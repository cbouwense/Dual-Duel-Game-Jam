using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{

    GameObject opp;
    //Transform origTrans;

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

            string attackName = hStats.getName();
            int damage = hStats.getDamage();
            float hitstun = hStats.getHitStun();
            float hitstop = hStats.getHitStop();
            Vector2 knockback = hStats.getKnockBack();
            Vector2 pushback = hStats.getPushBack();
            
            Debug.Log("hit " + opp.name);
            if (oppStats.isBlocking(attackName))
            {
                oppStats.Injure(damage / 5);
                oppStats.HitStun(0.1f, false);
                oppStats.PrimeKnockBack(new Vector2(1, 0));
            }
            else
            {
                oppStats.Injure(damage);
                oppStats.HitStun(hitstun, true);
                oppStats.PrimeKnockBack(knockback);
            }
            myStats.HitStop(hitstop);
            myStats.PrimeKnockBack(pushback);
        }
    }

}