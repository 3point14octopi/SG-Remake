using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FbHealthBar : MonoBehaviour
{
    public Image[] heads = new Image[5]; //References to the 5 heads
    public Sprite fullHead; //the sprites for the heads
    public Sprite halfHead;
    public Sprite emptyHead;
    private int head = 0; //int so we can keep our place in the array when traversing for loops

    public void HealthBar(int health){
        //Sets our full heads
        for(int i = head; i < (health / 2); i++){
            heads[i].sprite = fullHead;
            head++;
        }
        //Sets our half head (if we need it)
        if(health%2 == 1){
            heads[head].sprite = halfHead;
            head++;
        }
        //Sets our remaining heads to empty
        for(int i = head; i < 5; i++){
            heads[i].sprite = emptyHead;
        }
        head = 0;
    }
}
