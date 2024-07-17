using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PrinceStateUpdate_", menuName = "ScriptableObjects/Reactions/PrinceStateUpdate", order = 1)]
[Serializable]public class PrinceStateUpdate : Reaction
{
    PrinceController pCon;

    public override void OnStart(GameObject g)
    {
        isCoroutine = false;

        pCon = g.GetComponent<PrinceController>();
    }

    public override void ReactFunction()
    {
        pCon.CheckHealthState();
    }
}