using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite open;
    public Sprite closed;
    private int triggerCounter = 0; //counter can keep track of if we are inside a trigger or not
    public bool canPlace = true;

    public List<string> avoidTags = new List<string>();

    //checks to make sure nothing is under it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (avoidTags.Contains(collision.gameObject.tag))
        {
            triggerCounter++;
        }
    }

    //if we were on top of something when we leave the counter will go down
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (avoidTags.Contains(collision.gameObject.tag))
        {
            triggerCounter--;
        }
    }

    //if counter is above 0 we can do the logic for a occupied square or == 0 we can do logic for an open square
    private void Update()
    {
        if (triggerCounter == 0)
        {
            sr.sprite = open;
            canPlace = true; //lets state manager place a block
        }
        else if (triggerCounter > 0)
        {
            sr.sprite = closed;
            canPlace = false; //wont let state manager place a block
        }
    }
}
