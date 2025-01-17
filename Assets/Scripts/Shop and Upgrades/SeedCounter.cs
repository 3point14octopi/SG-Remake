using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeedCounter : MonoBehaviour
{
    public TMP_Text seedCountText;
    public GameObject seedBank;
    void Start()
    {
        seedBank = GameObject.FindWithTag("DDOL");
        seedBank.GetComponent<SeedBank>().SubscribeCounter(gameObject);
        ChangeCounter(seedBank.GetComponent<SeedBank>().seedsCurrent);
    }

    public void ChangeCounter(int i)
    {
        seedCountText.SetText(i.ToString());
    }
}
