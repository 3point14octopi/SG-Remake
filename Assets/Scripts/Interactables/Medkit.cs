using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    //stuff that damages the entity (probably player bullets)
    public List<string> effectTags = new List<string>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (effectTags.Contains(collision.gameObject.tag))
        {
            collision.gameObject.GetComponent<FbStateManager>().TakeDamage(-1f);
            Destroy(gameObject);
        }

    }
    
}
