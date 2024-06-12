using EntityStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbUpgradeManager : MonoBehaviour
{
    public FbBrain brain;
    public FbStateManager states;
    public FbGun gun;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Pickup")
        {
            for (int i = 0; i < collision.gameObject.GetComponent<Pickup>().effects.Count; i++)
            {
                Debug.Log(collision.gameObject.GetComponent<Pickup>().effects.Count);
                collision.gameObject.GetComponent<Pickup>().effects[i].ApplyUpgrade(this);
            }
            Destroy(collision.gameObject);
        }
    }
}
