using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeStats;

public class Pickup : MonoBehaviour
{
    //entities that can pick it up, currently dont see a reason why it would be anything other than the player
    public List<string> effectTags = new List<string>();
    [SerializeField]public List<Upgrade> effects = new List<Upgrade>();

}
