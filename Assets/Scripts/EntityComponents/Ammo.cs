using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Ammo
{
    public string ammoName;
    public float speed;
    public float firerate;
    public float damage;
    public int rebound;

    public float spreadNum;
    public float burstNum;

    public float spreadDis;
    public float spreadAngle;

    public GameObject prefab; 

    public static Ammo operator+(Ammo a, Ammo b)
    {
        Ammo c = new Ammo();
        c.ammoName = "Copy of " + b.ammoName;
        c.speed = b.speed;
        c.firerate = b.firerate;
        c.damage = b.damage;
        c.rebound = b.rebound;
        c.spreadNum = b.spreadNum;
        c.burstNum = b.burstNum;
        c.spreadDis = b.spreadDis;
        c.spreadAngle = b.spreadAngle;
        c.prefab = b.prefab;
        return c;

    }

}
