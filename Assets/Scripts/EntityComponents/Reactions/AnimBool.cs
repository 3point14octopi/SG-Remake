using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimBool_", menuName = "ScriptableObjects/Reactions/AnimationBool", order = 1 )]
[Serializable]public class AnimBool : Reaction
{
    public string animName;
    public float resetAfter = 0f;

    Animator anim;

    public override void OnStart(GameObject g)
    {
        isCoroutine = true;

        anim = g.GetComponent<Animator>();
    }

    public override IEnumerator ReactCoroutine()
    {
        anim.SetBool(animName, true);

        if(resetAfter > 0f)
        {
            yield return new WaitForSeconds(resetAfter);
            anim.SetBool(animName, false);
        }

        yield return null;
        routine = null;
    }
}