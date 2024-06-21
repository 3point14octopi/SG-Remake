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

    //The lists that are used to store current upgrades
    [SerializeField] public List<PlayerUpgrade> CurrentPlayerUpgrades = new List<PlayerUpgrade>();
    [SerializeField] public List<IceUpgrade> CurrentIceUpgrades = new List<IceUpgrade>();
    [SerializeField] public List<GunUpgrade> CurrentGunUpgrades = new List<GunUpgrade>();

    //Looks our pickups which are labelled with "Pickup:" calls a function built into their cubclass which will redirect to the correct FB script
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Pickup")
        {
            for (int i = 0; i < collision.gameObject.GetComponent<Pickup>().effects.Count; i++)
            {
                collision.gameObject.GetComponent<Pickup>().effects[i].ApplyUpgrade(this);
            }
            Destroy(collision.gameObject);
        }
    }
    
    //These 3 functions maintain a list of upgrades currently applied to our character.
    public void PlayerUpgradeTracker(PlayerUpgrade playerUpgrade)
    {
        CurrentPlayerUpgrades.Add(playerUpgrade);
    }
    public void IceUpgradeTracker(IceUpgrade iceUpgrade)
    {
        CurrentIceUpgrades.Add(iceUpgrade);
    }

    public void GunUpgradeTracker(GunUpgrade gunUpgrade)
    {
        CurrentGunUpgrades.Add(gunUpgrade);
    }

}
