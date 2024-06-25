using EntityStats;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BulletStyles
{
    Straight,
    Tracking
}

[CreateAssetMenu(menuName = "ScriptableObjects/Bullet")]

[Serializable]public class Ammo : ScriptableObject
{
    public string ammoName;
    public BulletStyles style;
    public float speed;
    public float firerate;
    public int rebound;
    public List<HitEffect> bulletEffects = new List<HitEffect>();

    [Range(1, 100)] public int spreadNum;
    [Range(1, 10)] public int burstNum ;

    public float spreadDis;
    public float spreadAngle;

    public GameObject prefab; 

    public static Ammo operator+(Ammo a, Ammo b)
    {
        Ammo c = new Ammo();
        c.ammoName = "Copy of " + b.ammoName;
        c.style = b.style;
        c.speed = b.speed;
        c.firerate = b.firerate;
        c.bulletEffects = b.bulletEffects;
        c.rebound = b.rebound;
        c.spreadNum = b.spreadNum;
        c.burstNum = b.burstNum;
        c.spreadDis = b.spreadDis;
        c.spreadAngle = b.spreadAngle;
        c.prefab = b.prefab;
        return c;

    }

}
