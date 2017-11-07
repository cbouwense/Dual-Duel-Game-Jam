using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxStats : MonoBehaviour {

    private int std_light_dmg = 3;
    private int std_medium_dmg = 4;
    private int std_heavy_dmg = 5;
    private int air_light_dmg = 3;
    private int air_medium_dmg = 4;
    private int air_heavy_dmg = 5;
    private int low_light_dmg = 3;
    private int low_medium_dmg = 4;
    private int low_heavy_dmg = 5;

    private int currentDamage = 0;
    private float hitStun = 0.3f;
    private float hitLag = 0.1f;

    public void setDamage(string type)
    {
        switch (type)
        {
            case "std_light": currentDamage = std_light_dmg; break;
            case "std_medium": currentDamage = std_medium_dmg; break;
            case "std_heavy": currentDamage = std_heavy_dmg; break;
            case "air_light": currentDamage = air_light_dmg; break;
            case "air_medium": currentDamage = air_medium_dmg; break;
            case "air_heavy": currentDamage = air_heavy_dmg; break;
            case "low_light": currentDamage = low_light_dmg; break;
            case "low_medium": currentDamage = low_medium_dmg; break;
            case "low_heavy": currentDamage = low_medium_dmg; break;
            default: Debug.Log("Invalid damage requested"); currentDamage = 0; break;
        }
    }
    public int getDamage()
    {
        return currentDamage;
    }
    public float getHitStun() { return hitStun; }
    public float getHitLag() { return hitLag; }

}
