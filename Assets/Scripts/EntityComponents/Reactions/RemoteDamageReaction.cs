using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RemoteDamage_", menuName = "ScriptableObjects/Reactions/Remote Damage", order = 1)]
[Serializable]public class RemoteDamageReaction : Reaction
{
    public GameObject trigger;
    Transform location;

    public override void OnStart(GameObject g)
    {
        isCoroutine = true;

        location = g.transform.parent;
    }

    public override IEnumerator ReactCoroutine()
    {
        trigger = Instantiate(trigger, location.parent, true);
        trigger.transform.position = location.position;

        yield return new WaitForSeconds(0.25f);

        Destroy(trigger);
    }

}
