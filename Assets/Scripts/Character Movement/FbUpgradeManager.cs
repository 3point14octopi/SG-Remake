using EntityStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeStats;

public class FbUpgradeManager : MonoBehaviour
{
    //references to our othere player controller scripts that take upgrades
    public FbBrain brain;
    public FbStateManager states;
    public FbGun gun;

    public KeyCode inventoryKey;
    public GameObject inventoryScreen;

    //The lists that are used to store current upgrades
    [SerializeField] public List<Upgrade> CurrentUpgrades = new List<Upgrade>();

    private void Update()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            inventoryScreen.SetActive(true);
        }
        if (Input.GetKeyUp(inventoryKey))
        {
            inventoryScreen.SetActive(false);
        }
    }

    //Looks our pickups which are labelled with "Pickup:" calls a function built into their cubclass which will redirect to the correct FB script
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Pickup")
        {
            UpgradeTracker(collision.gameObject.GetComponent<Pickup>().upgrade);
            
            foreach (PlayerUpgrade upgrade in collision.gameObject.GetComponent<Pickup>().upgrade.playerEffects) brain.PlayerUpgrade(upgrade);
            foreach (IceUpgrade upgrade in collision.gameObject.GetComponent<Pickup>().upgrade.iceEffects) states.IceUpgrade(upgrade);
            foreach (GunUpgrade upgrade in collision.gameObject.GetComponent<Pickup>().upgrade.gunEffects) gun.GunUpgrade(upgrade);
            inventoryScreen.GetComponent<FbInventory>().AddUpgrade(collision.gameObject.GetComponent<Pickup>().upgrade);
            Destroy(collision.gameObject);
        }
    }
    
    //Maintains a list of all the upgrades on our character

    public void UpgradeTracker(Upgrade upgrade)
    {
        CurrentUpgrades.Add(upgrade); 
    }


}
