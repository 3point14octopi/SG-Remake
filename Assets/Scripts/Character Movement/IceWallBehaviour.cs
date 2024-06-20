using EntityStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWallBehaviour : MonoBehaviour
{
    private int hitPoints;
    private GameObject frostBite;
    public List<string> damageTags = new List<string>();
    private SpriteRenderer sr;
    public List<Sprite> iceSprites = new List<Sprite>();
    private bool isAlive = true;

    //Called by statemanager when the ice wall is built
    public void InstantiateWall(int health, GameObject fb)
    {
        frostBite = fb;
        hitPoints = health;
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damageTags.Contains(collision.gameObject.tag))
        {
            hitPoints--;
            if(hitPoints > 0) sr.sprite = iceSprites[hitPoints - 1];

            //when dies kills itself and tells statemanger there is one less object on the loose
            else if (hitPoints <= 0 && isAlive)
            {
                isAlive = false;
                frostBite.GetComponent<FbStateManager>().currentIceUses++;
                Destroy(gameObject);
            }
        }
    }

    //uses trigger and collision because trigger is bullets and collision is enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageTags.Contains(collision.gameObject.tag))
        {
            hitPoints--;
            if (hitPoints > 0) sr.sprite = iceSprites[hitPoints - 1];

            else if (hitPoints <= 0 && isAlive)
            {
                isAlive = false;
                frostBite.GetComponent<FbStateManager>().currentIceUses++;
                Destroy(transform.parent.gameObject);
            }
        }
    }


}
