using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class FbInventory : MonoBehaviour
{
    public GameObject upgradeIcon;
    public GameObject itemHolder;
    public TMP_Text descriptionText;

    //The lists that are used to store current upgrades
    [SerializeField] public List<Upgrade> TotalUpgrades = new List<Upgrade>(); //includes repeats
    [SerializeField] public List<GameObject> VisibleUpgrades = new List<GameObject>(); //nor repeats

    /// <summary>
    /// Called to add an upgrade icon to the inventory screen
    /// </summary>
    /// <param name="upgrade">Upgrade that has</param>
    public void AddUpgrade(Upgrade upgrade)
    {
        if (TotalUpgrades.Contains(upgrade)) AddMultiple(upgrade);

        else {
            GameObject temp = Instantiate(upgradeIcon, itemHolder.transform);
            temp.GetComponent<UpgradeIcon>().SetUpgradeUI(upgrade, descriptionText);
            VisibleUpgrades.Add(temp);
        }
        TotalUpgrades.Add(upgrade);

    }

    /// <summary>
    /// Called when we have a preexisting upgrade of the same type we picked up
    /// </summary>
    /// <param name="currentUpgrade">Upgrade that has</param>
    private void AddMultiple(Upgrade currentUpgrade)
    {
        foreach(var visibleUpgrade in VisibleUpgrades){

            var upgradeIconComponent = visibleUpgrade.GetComponent<UpgradeIcon>();
            if (upgradeIconComponent.upgrade == currentUpgrade)
            {
                upgradeIconComponent.MultipleCounter();
                break;
            }
        }
    }


}
