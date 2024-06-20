using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeStats;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "ScriptableObjects/Upgrades/IceUpgrade", order = 3)]
public class IceUpgrade : Upgrade
{



    public IceUpgrades iceUpgrade;

    public override void ApplyUpgrade(FbUpgradeManager upgradeManager)
    {
        upgradeManager.states.IceUpgrade(this);
        upgradeManager.IceUpgradeTracker(this);
    }
}
