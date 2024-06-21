using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class FireballObj : MonoBehaviour
{
    public float damage = 1;

    public bool falling = true;


    public void ToggleFalling(bool value)
    {
        falling = value;
        Debug.Log("toggled falling to " + value.ToString());
    }

}