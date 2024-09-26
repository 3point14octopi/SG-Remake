using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WispOnDeath_", menuName = "ScriptableObjects/Reactions/Wisp OnDeath", order = 3)]
[Serializable]public class WispOnDeath : Reaction
{
    Animator anim;
    Rigidbody2D body;

    public override void OnStart(GameObject g)
    {
        anim = g.GetComponent<Animator>();
        body = g.GetComponent<Rigidbody2D>();

        isCoroutine = true;
    }

    public override IEnumerator ReactCoroutine()
    {
        anim.Play("WispDeath");
        body.bodyType = RigidbodyType2D.Kinematic;
        body.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(0);
        routine = null;
    }
}
