using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrinceHealthBarUpdate", menuName = "ScriptableObjects/Reactions/PhaseDeath", order = 1)]
[Serializable]public class PhaseDeath : Reaction
{
    private PhaseFrameWork phases;
    public override void OnStart(GameObject g)
    {
        isCoroutine = false;
        phases = g.GetComponent<PhaseFrameWork>();
    }

    // Update is called once per frame
    public override void ReactFunction()
    {
        phases.OnDeath(); //calls the phaseframeworks death function
    }
}
