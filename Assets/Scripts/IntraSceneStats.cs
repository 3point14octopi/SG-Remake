using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntraSceneStats : MonoBehaviour
{
    public float health;
    public bool destroyed = false;

    
    private void OnLevelWasLoaded()
    {
        if (!destroyed)
        {
            GameObject.FindWithTag("Player").GetComponent<FbBrain>().SetHealth(health);
        }
    }
}
