using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeDisplay : MonoBehaviour
{
    public GameObject seedBank;
    public GameObject glass;
    public int price;
    private bool isBought = false;
    void Start()
    {
        seedBank = GameObject.FindWithTag("DDOL");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isBought)
        {
            if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.Space)) {

                seedBank.GetComponent<SeedBank>().SpendSeed(price);
                glass.SetActive(false);
                isBought = true;
            }
        }
    }
}
