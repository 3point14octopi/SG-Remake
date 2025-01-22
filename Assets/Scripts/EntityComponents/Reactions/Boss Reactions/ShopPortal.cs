using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
[CreateAssetMenu(fileName = "ShopDoor", menuName = "ScriptableObjects/Reactions/ShopDoor", order = 1)]

public class ShopPortal : Reaction
{
    // Start is called before the first frame update
    private GameObject door;
    public GameObject seedBank;
    public override void OnStart(GameObject g)
    {
        isCoroutine = false;
        seedBank = GameObject.FindWithTag("DDOL");
        door = GameObject.Find("Shop Portal");
        door.SetActive(false);
    }

    public override void ReactFunction()
    {
        if(seedBank != null) seedBank.GetComponent<SeedBank>().EarnSeed();
        door.SetActive(true);
    }
}
