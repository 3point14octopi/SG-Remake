using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FBIceBar : MonoBehaviour
{
    public Image iceBar; //reference for the UI element
    public Sprite[] iceSprites = new Sprite[6]; //References to the 5 ice images

    public void IceBar(int ice){
        iceBar.sprite = iceSprites[ice];
    }
}
