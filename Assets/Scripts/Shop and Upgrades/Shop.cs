using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<Upgrade> upgrades;
    public GameObject item1;
    public GameObject item2;
    void Start()
    {
        DecideShopItems();
    }
    public void DecideShopItems()
    {
        if (upgrades != null)
        {
            int firstIndex = Random.Range(0, upgrades.Count);
            int secondIndex;
            do
            {
                secondIndex = Random.Range(0, upgrades.Count);
            } while (secondIndex == firstIndex);

            item1.GetComponent<Pickup>().AssignUpgrade(upgrades[firstIndex]);
            item2.GetComponent<Pickup>().AssignUpgrade(upgrades[secondIndex]);
        }
    }
}
