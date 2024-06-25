using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ManualShot_", menuName = "ScriptableObjects/Reactions/Manual Shot", order = 2)]
[Serializable]public class ManualShot : Reaction
{
    private GunModule gun;
    public Ammo whatever;

    public override void OnStart(GameObject g)
    {
        isCoroutine = false;
        gun = g.GetComponent<GunModule>();
        whatever = (whatever.prefab == null) ? gun.currentAmmo : whatever;
    }


    public override void ReactFunction()
    {
        gun.StartCoroutine(gun.PresetShoot(whatever));
    }
}
