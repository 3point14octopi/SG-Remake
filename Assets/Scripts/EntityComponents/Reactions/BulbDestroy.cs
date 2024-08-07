using System;
using UnityEngine;


[CreateAssetMenu(fileName = "BulbDestroy_", menuName = "ScriptableObjects/Reactions/Bulb Destroy", order = 1)]
[Serializable]public class BulbDestroy : Reaction
{
    BulbController bCon;
    bool hasMom = false;
    GameObject gob;
    public override void OnStart(GameObject g)
    {
        gob = g;
        bCon = g.GetComponent<BulbController>();

        isCoroutine = false;
    }

    public override void ReactFunction()
    {
        gob.GetComponent<Brain>().mom.GetRoomEnemy(0).gameObject.GetComponent<PrinceController>().RetractVineTrail(bCon.vineDirection);
        gob.SetActive(false);
    }
}
