using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UpgradeStats;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "ScriptableObjects/Upgrades/PlayerUpgrade", order = 1)]
public class PlayerUpgrade : Upgrade
{
 
  
    public PlayerUpgrades playerUpgrade;
    public int modifier;

    public override void ApplyUpgrade(FbBrain fb)
    {
        Debug.Log("Inside PlayerUpgrade");
         fb.PlayerUpgrade(this);
    }
}
