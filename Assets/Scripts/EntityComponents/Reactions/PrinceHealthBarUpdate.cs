using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PrinceHealthBarUpdate", menuName = "ScriptableObjects/Reactions/PrinceHealthBarUpdate", order = 1)]
[Serializable]public class PrinceHealthBarUpdate : Reaction
{
    private Slider slider;
    private Brain prince;
    public override void OnStart(GameObject g)
    {
        isCoroutine = false;

        slider = GameObject.Find("Prince Slider").GetComponent<Slider>();
        prince = g.GetComponent<Brain>();
    }

    public override void ReactFunction()
    {
        slider.value = prince.currentStats[0] / prince.Stats[0];
    }
}
