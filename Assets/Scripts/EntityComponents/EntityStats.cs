using System;


namespace EntityStats
{
    public enum EntityStat
    {
        Health, 
        Speed, 
        Damage
    }

    [Serializable]public struct HitEffect
    {
        public EntityStat targetedStat;
        public int modifier;

        public HitEffect(EntityStat entityStat, int mod)
        {
            targetedStat = entityStat;
            modifier = mod;
        }
    }

}
