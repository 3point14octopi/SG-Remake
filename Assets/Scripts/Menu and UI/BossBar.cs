using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    public Slider slider;

    public void BossHealthOn(float maxHealth){
        slider.maxValue = Mathf.FloorToInt(maxHealth);
        slider.value = Mathf.FloorToInt(maxHealth);
        slider.enabled = true;
    }

    public void BossHealthSet(float health){
        slider.value = Mathf.FloorToInt(health);
    }

    public void BossHealthOff(){
        slider.enabled = false;
    }
}