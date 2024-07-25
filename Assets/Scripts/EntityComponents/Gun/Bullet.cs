using System;
using System.Collections.Generic;
using UnityEngine;
using EntityStats;

public enum BulletStyles
{
    Straight,
    Tracking,
    Arcing,
    Spinning, 
    PrinceArcing, 
    PrinceFakeout,
    Ripple
}

[CreateAssetMenu(menuName = "ScriptableObjects/Bullet")]

[Serializable] 
public class Bullet : ScriptableObject
{
    [Tooltip("What should the bullet do in the air after it is fired?")]
    public BulletStyles style;
    [Tooltip("How fast should it move through the air?")]
    public float speed;
    [Tooltip("How many seconds between bullets?")]
    public float firerate;
    [Tooltip("How many times should the bullet bounce off walls?")]
    public int rebound;
    [Tooltip("How far should the bullet be able to travel before despawning?(set this to -1 to have no limit)")]
    public float lifeSpan;
    [Tooltip("How big should the bullet be on its spawn?(1 is the standard prefab size")]
    public float size = 1;

    [Tooltip("How much health (or other stats) should it take away?")]
    public List<HitEffect> bulletEffects = new List<HitEffect>();

    [Tooltip("How many fired in a row?")]
    [Range(1, 100)] public int spreadNum;
    [Tooltip("How many bullets should be fired in a burst?")]
    [Range(1, 10)] public int burstNum;

    [Tooltip("Distance between bullets in a row?")]
    public float spreadDis;
    [Tooltip("Angle seperation between bullets in a row?")]
    public float spreadAngle;
    [Tooltip("Angle they should curve at in the air?")]
    public float arcAngle;

    [Tooltip("If you want to override distance between bullets")]
    public List<float> spreadsDis = new List<float>();
    [Tooltip("If you want to override angle between bullets")]
    public List<float> spreadsAngle = new List<float>();


    [Tooltip("Leave unticked if the bullet spread or angle was changed")]
    public bool calculated = false;
}