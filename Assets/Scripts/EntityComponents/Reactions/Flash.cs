using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Flash_", menuName = "ScriptableObjects/Reactions/Flash", order = 1)]
[Serializable]public class Flash:Reaction
{
    
    public Material flashMaterial;
    public Material baseMaterial { private get; set; }
    public float flashDuration;
    private SpriteRenderer srRef;
    

    public override void OnStart(GameObject g)
    {
        isCoroutine = true;
        srRef = g.GetComponent<SpriteRenderer>();
        baseMaterial = srRef.material;
    }

    public override IEnumerator ReactCoroutine()
    {
        Debug.Log("FLASHING");
        srRef.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        srRef.material = baseMaterial;
        routine = null;
    }
}

