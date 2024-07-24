using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeStats;

public class Pickup : MonoBehaviour
{
    //entities that can pick it up, currently dont see a reason why it would be anything other than the player
    public List<string> effectTags = new List<string>();
    public Upgrade upgrade;

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = upgrade.uiPic;
    }

}
