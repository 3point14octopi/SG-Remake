using System;
using UnityEngine;
using EntityStats;

[CreateAssetMenu(fileName = "PhaseCheck", menuName = "ScriptableObjects/Reactions/PhaseCheck", order = 1)]

[Serializable]public class PhaseCheck : Reaction
{

    private PhaseFrameWork phases;
    private Brain brain;
    public override void OnStart(GameObject g)
    {
        isCoroutine = false;
        phases = g.GetComponent<PhaseFrameWork>();
        brain = g.GetComponent<Brain>();
    }

    public override void ReactFunction()
    {
        phases.PhaseGate(brain.Stats[(int)EntityStat.Health]); //checks if we have taken enough damage to enter a new phase
    }
}
