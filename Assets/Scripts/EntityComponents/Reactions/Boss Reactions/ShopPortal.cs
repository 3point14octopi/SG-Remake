using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShopDoor", menuName = "ScriptableObjects/Reactions/ShopDoor", order = 1)]

public class ShopPortal : Reaction
{
    // Start is called before the first frame update
    private GameObject door;
    public override void OnStart(GameObject g)
    {
        isCoroutine = false;

        door = GameObject.Find("Shop Portal");
        door.SetActive(false);
    }

    public override void ReactFunction()
    {
        door.SetActive(true);
    }
}
