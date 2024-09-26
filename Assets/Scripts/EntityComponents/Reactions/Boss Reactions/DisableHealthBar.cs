using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DisableHealthBar", menuName = "ScriptableObjects/Reactions/DisableHealthBar", order = 1)]
public class DisableHealthBar : Reaction
{
    private GameObject healthBar;
    public override void OnStart(GameObject g)
    {
        isCoroutine = false;

        //slider = GameObject.Find("Prince Slider").GetComponent<Slider>();
        healthBar = g.GetComponent<PrinceController>().healthBar;
    }

    public override void ReactFunction()
    {
        healthBar.SetActive(false);
    }
}
