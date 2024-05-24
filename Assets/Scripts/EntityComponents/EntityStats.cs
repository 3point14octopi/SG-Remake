using System;


namespace EntityStats
{
    public enum EntityStat
    {
        Health
    }

    [Serializable]public struct HitEffect
    {
        public EntityStat targetedStat;
        public int modifier;
    }

}
