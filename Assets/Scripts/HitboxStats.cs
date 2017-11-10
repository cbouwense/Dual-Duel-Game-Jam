using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxStats : MonoBehaviour {

    private string attackName;

    private int std_light_dmg = 3;
    private int std_medium_dmg = 4;
    private int std_heavy_dmg = 5;
    private int air_light_dmg = 3;
    private int air_medium_dmg = 4;
    private int air_heavy_dmg = 5;
    private int low_light_dmg = 3;
    private int low_medium_dmg = 4;
    private int low_heavy_dmg = 5;

    private float std_light_hitstun = 0.167f * 2;
    private float std_medium_hitstun = 0.233f * 2;
    private float std_heavy_hitstun = 0.317f * 2;
    private float air_light_hitstun = 0.167f * 2;
    private float air_medium_hitstun = 0.233f * 2;
    private float air_heavy_hitstun = 0.317f * 2;
    private float low_light_hitstun = 0.167f * 2;
    private float low_medium_hitstun = 0.233f * 2;
    private float low_heavy_hitstun = 0.317f * 2;

    private float std_light_hitstop = 0.183f;
    private float std_medium_hitstop = 0.217f;
    private float std_heavy_hitstop = 0.250f;
    private float air_light_hitstop = 0.183f;
    private float air_medium_hitstop = 0.217f;
    private float air_heavy_hitstop = 0.250f;
    private float low_light_hitstop = 0.183f;
    private float low_medium_hitstop = 0.217f;
    private float low_heavy_hitstop = 0.250f;

    private Vector2 std_light_knockback = new Vector2(2, 4);
    private Vector2 std_medium_knockback = new Vector2(2, 8);
    private Vector2 std_heavy_knockback = new Vector2(3, 8);
    private Vector2 air_light_knockback = new Vector2(2, 4);
    private Vector2 air_medium_knockback = new Vector2(2, 8);
    private Vector2 air_heavy_knockback = new Vector2(3, 8);
    private Vector2 low_light_knockback = new Vector2(2, 4);
    private Vector2 low_medium_knockback = new Vector2(2, 8);
    private Vector2 low_heavy_knockback = new Vector2(3, 8);

    private Vector2 std_light_pushback = new Vector2(3, 0);
    private Vector2 std_medium_pushback = new Vector2(3, 0);
    private Vector2 std_heavy_pushback = new Vector2(3, 0);
    private Vector2 air_light_pushback = new Vector2(3, 0);
    private Vector2 air_medium_pushback = new Vector2(3, 0);
    private Vector2 air_heavy_pushback = new Vector2(3, 0);
    private Vector2 low_light_pushback = new Vector2(3, 0);
    private Vector2 low_medium_pushback = new Vector2(3, 0);
    private Vector2 low_heavy_pushback = new Vector2(3, 0);

    private int currentDamage = 0;
    private float currentHitStun = 0.0f;
    private float currentHitStop = 0.0f;
    private Vector2 currentKnockBack = new Vector2();
    private Vector2 currentPushBack = new Vector2();
    
    public void setStats(string type)
    {
        attackName = type;

        switch (type)
        {
            case "std_light":
                currentDamage = std_light_dmg;
                currentHitStun = std_light_hitstun;
                currentHitStop = std_light_hitstop;
                currentKnockBack = std_light_knockback;
                currentPushBack = std_light_pushback;
                break;
            case "std_medium":
                currentDamage = std_medium_dmg;
                currentHitStun = std_medium_hitstun;
                currentHitStop = std_medium_hitstop;
                currentKnockBack = std_medium_knockback;
                currentPushBack = std_medium_pushback;
                break;
            case "std_heavy":
                currentDamage = std_heavy_dmg;
                currentHitStun = std_heavy_hitstun;
                currentHitStop = std_heavy_hitstop;
                currentKnockBack = std_heavy_knockback;
                currentPushBack = std_heavy_pushback;
                break;
            case "air_light":
                currentDamage = air_light_dmg;
                currentHitStun = air_light_hitstun;
                currentHitStop = air_light_hitstop;
                currentKnockBack = air_light_knockback;
                currentPushBack = air_light_pushback;
                break;
            case "air_medium":
                currentDamage = air_medium_dmg;
                currentHitStun = air_medium_hitstun;
                currentHitStop = air_medium_hitstop;
                currentKnockBack = air_medium_knockback;
                currentPushBack = air_medium_pushback;
                break;
            case "air_heavy":
                currentDamage = air_heavy_dmg;
                currentHitStun = air_heavy_hitstun;
                currentHitStop = air_heavy_hitstop;
                currentKnockBack = air_heavy_knockback;
                currentPushBack = air_heavy_pushback;
                break;
            case "low_light":
                currentDamage = low_light_dmg;
                currentHitStun = low_light_hitstun;
                currentHitStop = low_light_hitstop;
                currentKnockBack = low_light_knockback;
                currentPushBack = low_light_pushback;
                break;
            case "low_medium":
                currentDamage = low_medium_dmg;
                currentHitStun = low_medium_hitstun;
                currentHitStop = low_medium_hitstop;
                currentKnockBack = low_medium_knockback;
                currentPushBack = low_medium_pushback;
                break;
            case "low_heavy":
                currentDamage = low_heavy_dmg;
                currentHitStun = low_heavy_hitstun;
                currentHitStop = low_heavy_hitstop;
                currentKnockBack = low_heavy_knockback;
                currentPushBack = low_heavy_pushback;
                break;
            default:
                Debug.Log("Invalid attack sent");
                currentDamage = 0;
                currentHitStun = 0.0f;
                currentHitStop = 0.0f;
                currentKnockBack = new Vector2();
                currentPushBack = new Vector2();
                break;
        }
    }

    public string getName() { return attackName; }
    public int getDamage() { return currentDamage; }
    public float getHitStun() { return currentHitStun; }
    public float getHitStop() { return currentHitStop; }
    public Vector2 getKnockBack() { return currentKnockBack; }
    public Vector2 getPushBack() { return currentPushBack; }

}
