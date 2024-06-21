using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace UpgradeStats
{
    public enum PlayerUpgrades
    {
        Health,
        Speed
    }
    public enum IceUpgrades
    {
        Block,
        Wall,
        Decoy
    }

    public enum GunUpgrades
    {
        Damage,
        Speed,
        Size,
        Firerate,
        Rebound,
        Spread,
        Burst
    }



    [Serializable]public class Upgrade: ScriptableObject
    {
        

        public virtual void ApplyUpgrade(FbUpgradeManager upgradeManager)
        {

        }
    }
}
