using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FrosbiteHitAnim", menuName = "ScriptableObjects/Reactions/FrosbiteHitAnim", order = 1)]
[Serializable]public class FrostbiteHitAnim : Reaction
{
    public Animator animator;

    // Start is called before the first frame update
    public override void OnStart(GameObject g)
    {
        isCoroutine = true;
        animator = g.GetComponent<FbBrain>().anim;
    }

    public override IEnumerator ReactCoroutine()
    {
        animator.SetBool("Hit", true);
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("Hit", false);
        routine = null;
    }
}
