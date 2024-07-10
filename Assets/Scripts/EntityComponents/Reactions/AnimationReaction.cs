using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationReaction_", menuName = "ScriptableObjects/Reactions/AnimationReaction", order =1)]
[Serializable]public class AnimationReaction : Reaction
{
    public string animation;

    Animator animator;

    public override void OnStart(GameObject g)
    {
        isCoroutine = false;

        animator = g.GetComponent<Animator>();
    }

    public override void ReactFunction()
    {
        animator.Play(animation);
    }
}