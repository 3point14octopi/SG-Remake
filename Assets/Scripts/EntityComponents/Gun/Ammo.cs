using EntityStats;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[Serializable]public class Ammo
{
    public Bullet bullet;
    public GameObject casing;
    public static Ammo operator +(Ammo a, Ammo b)
    {
        Ammo c = new Ammo();
        c.bullet = b.bullet;
        c.casing = b.casing;
        return c;

    }
}
