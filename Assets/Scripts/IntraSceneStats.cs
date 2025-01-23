using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntraSceneStats : MonoBehaviour
{
    public float health;
    public bool destroyed = false; //if this is not the prime DDOL destoryed this bool is flipped by the not prime DDOL script

    
    private void OnLevelWasLoaded()
    {
        if (!destroyed)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<FbBrain>().SetHealth(health);
            player.GetComponent<FbStateManager>().RechargeAbilities();
        }
    }
}
