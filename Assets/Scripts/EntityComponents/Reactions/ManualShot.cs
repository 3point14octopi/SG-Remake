using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ManualShot_", menuName = "ScriptableObjects/Reactions/Manual Shot", order = 2)]
[Serializable]public class ManualShot : Reaction
{
    private GunModule gun;
    public Ammo ammo;

    public override void OnStart(GameObject g)
    {
        isCoroutine = false;
        gun = g.GetComponent<GunModule>();
        ammo = (ammo.casing == null) ? gun.currentAmmo : ammo;
    }


    public override void ReactFunction()
    {
        gun.StartCoroutine(gun.PresetShoot(ammo));
    }
}
