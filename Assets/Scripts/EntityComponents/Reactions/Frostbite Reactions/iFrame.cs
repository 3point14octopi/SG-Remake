using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "IFrame_", menuName = "ScriptableObjects/Reactions/IFrame", order = 1)]
[Serializable]public class iFrame : Reaction
{
    public float iFrameDuration;
    private FbBrain brain;


    public override void OnStart(GameObject g)
    {
        isCoroutine = true;
        brain = g.GetComponent<FbBrain>();
    }

    public override IEnumerator ReactCoroutine()
    {
        brain.iFrame = true;
        yield return new WaitForSeconds(iFrameDuration);
        brain.iFrame = false;
        routine = null;
    }
}
