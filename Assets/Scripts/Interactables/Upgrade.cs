using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "ScriptableObjects/Upgrades")]
[Serializable] public class Upgrade : ScriptableObject
{
    public string upgradeName; 
    public string upgradeDescription;
    public Sprite uiPic;
    public List<PlayerUpgrade> playerEffects = new List<PlayerUpgrade>();
    public List<IceUpgrade> iceEffects = new List<IceUpgrade>();
    public List<GunUpgrade> gunEffects = new List<GunUpgrade>();

}
