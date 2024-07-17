using System;


namespace EntityStats
{
    public enum EntityStat //All entities that use our brain script have these stats on them in a array
    {
        Health, 
        Speed, 
        Damage
    }

    [Serializable]public struct HitEffect //Hiteffects have a reference to the stat they are trying to affect and how it affects it
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
