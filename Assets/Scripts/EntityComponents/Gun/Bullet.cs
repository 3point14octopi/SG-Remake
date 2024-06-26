using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityStats;

public enum BulletStyles
{
    Straight,
    Tracking
}

[CreateAssetMenu(menuName = "ScriptableObjects/Bullet")]

    [Serializable] 
    public class Bullet : ScriptableObject
    {
        public BulletStyles style;
        public float speed;
        public float firerate;
        public int rebound;
        public List<HitEffect> bulletEffects = new List<HitEffect>();

        [Range(1, 100)] public int spreadNum;
        [Range(1, 10)] public int burstNum;

        public float spreadDis;
        public float spreadAngle;
    }

