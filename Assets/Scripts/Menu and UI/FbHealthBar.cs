using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FbHealthBar : MonoBehaviour
{
    public List<Image> heads = new List<Image>(); //References to the 5 heads
    public Sprite fullHead; //the sprites for the heads
    public Sprite halfHead;
    public Sprite emptyHead;
    public Sprite fullSplitHead; //half head UI image
    public Sprite emptySplitHead;

    private bool splitHead; //acts as a bool to remember if 

    public GameObject UIItem; //the gameobject used to add new health icons

    private float maxHealth;
    private int head = 0; //int so we can keep our place in the array when traversing for loops

    public void HealthBar(float health){
        
        if (splitHead) //for if max health is odd
        {
            //sets our full heads (number of times 2 goes into our health)
            for (int i = head; i < (Mathf.FloorToInt(health) / 2); i++) {heads[i].sprite = fullHead; head++;}

            //sets our final half head to full if we have full health
            if (head == heads.Count - 1 && health == maxHealth) {heads[head].sprite = fullSplitHead; head++; }

            //Sets our half head (if we need it)
            else if (Mathf.FloorToInt(health) % 2 == 1) {heads[head].sprite = halfHead; head++;}

            //Sets our remaining heads to empty
            for (int i = head; i < heads.Count - 1; i++) {heads[i].sprite = emptyHead; head++;}

            //Sets the final half head to empty
            if(head == heads.Count - 1) heads[head].sprite = emptySplitHead;
        }

        else if (splitHead) //for if max health is even
        {
            //sets our full heads (number of times 2 goes into our health)
            for (int i = head; i < (Mathf.FloorToInt(health) / 2); i++) {heads[i].sprite = fullHead; head++;}

            //Sets our half head (if we need it)
            if (Mathf.FloorToInt(health) % 2 == 1) {heads[head].sprite = halfHead; head++;}

            //Sets our remaining heads to empty
            for (int i = head; i < heads.Count; i++)heads[i].sprite = emptyHead;
        }
        head = 0;
    }

    //When our max health is changed this will add
    public void MaxHealthBar(float max, float currentHealth)
    {
        while(heads.Count < max / 2f) //adds heads until we have 1 for every 2 health
        {
            //creates the new ui icon and adds it to our array
            GameObject temp = Instantiate(UIItem, gameObject.transform); 
            heads.Add(temp.GetComponent<Image>());
        }

        //stores if the max health is even(all full heads) or odd (has a a half icon)
        if (Mathf.FloorToInt(max) % 2 == 1) splitHead = true;
        else if (Mathf.FloorToInt(max) % 2 == 0) splitHead = true;
        maxHealth = max;
        HealthBar(currentHealth);

    }
}


