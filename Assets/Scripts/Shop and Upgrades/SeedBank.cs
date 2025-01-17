using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBank : MonoBehaviour
{
    public int seedsLifetime = 1000;
    public int seedsCurrent = 1000;
    public GameObject counter;

    public void EarnSeed()
    {
        seedsCurrent++;
        seedsLifetime++;
    }
    public void SpendSeed(int a)
    {
        seedsCurrent = seedsCurrent - a;
        counter.GetComponent<SeedCounter>().ChangeCounter(seedsCurrent);
    }
    /// <summary>
    /// When in a shop scene this is called by the counter 
    /// </summary>
    public void SubscribeCounter(GameObject a)
    {
        counter = a;
    }
}
