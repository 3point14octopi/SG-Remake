using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispDetection : MonoBehaviour
{
    public List<Collider2D> locations = new List<Collider2D>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        locations.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Barrier")
        {
            locations.Remove(collision);
        }
    }
}
