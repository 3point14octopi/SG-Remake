using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace UpgradeStats
{
    public enum PlayerUpgrades
    {
        Health,
        MaxHealth,
        Speed,
        NoRiskNoReward,
        BleedingHearts
    }
    public enum IceUpgrades
    {
        Block,
        Wall,
        Time,
        Drill,
        Teleport,
        AbilityUses
    }

    public enum GunUpgrades
    {
        Damage,
        Speed,
        Size,
        Firerate,
        Lifespan,
        Rebound,
        Spread,
        Burst,
        NoRiskNoReward
    }

}
