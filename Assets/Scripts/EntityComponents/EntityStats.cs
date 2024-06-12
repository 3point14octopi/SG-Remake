using System;


namespace EntityStats
{
    public enum EntityStat
    {
        Health,
        Speed
    }

    [Serializable]public struct HitEffect
    {
        public EntityStat targetedStat;
        public int modifier;
    }

}
