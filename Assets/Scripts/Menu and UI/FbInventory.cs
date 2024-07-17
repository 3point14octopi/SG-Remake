using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FbInventory : MonoBehaviour
{
    public GameObject upgradeIcon;
    public GameObject itemHolder;
    public TMP_Text descriptionText;
    
    public void AddUpgrade(Upgrade upgrade)
    {
        GameObject temp = Instantiate(upgradeIcon, itemHolder.transform);
        temp.GetComponent<UpgradeIcon>().SetUpgradeUI(upgrade, descriptionText);
    }

}
