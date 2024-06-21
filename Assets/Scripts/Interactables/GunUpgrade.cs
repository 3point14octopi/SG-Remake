using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeStats;


[CreateAssetMenu(fileName = "New Upgrade", menuName = "ScriptableObjects/Upgrades/GunUpgrade", order = 2)]
public class GunUpgrade : Upgrade
{


    public GunUpgrades gunUpgrade;
    public int modifier;

    public override void ApplyUpgrade(FbUpgradeManager upgradeManager)
    {
        upgradeManager.gun.GunUpgrade(this);
        upgradeManager.GunUpgradeTracker(this);
    }
}
