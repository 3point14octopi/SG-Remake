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
    public bool inventoryActive = false;



    private void Update()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            inventoryActive = !inventoryActive;
            inventoryScreen.SetActive(inventoryActive);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pickup")
        {
            foreach (PlayerUpgrade upgrade in collision.gameObject.GetComponent<Pickup>().upgrade.playerEffects) brain.PlayerUpgrade(upgrade);
            foreach (IceUpgrade upgrade in collision.gameObject.GetComponent<Pickup>().upgrade.iceEffects) states.IceUpgrade(upgrade);
            foreach (GunUpgrade upgrade in collision.gameObject.GetComponent<Pickup>().upgrade.gunEffects) gun.GunUpgrade(upgrade);
            if(collision.gameObject.GetComponent<Pickup>().upgrade.storeInUI)inventoryScreen.GetComponent<FbInventory>().AddUpgrade(collision.gameObject.GetComponent<Pickup>().upgrade);
            Destroy(collision.gameObject);
        }
    }
    



}
