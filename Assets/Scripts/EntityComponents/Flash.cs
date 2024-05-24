using System;
using System.Collections;
using UnityEngine;

[Serializable]public class Flash
{
    public Material flashMaterial;
    public Material baseMaterial { private get; set; }
    public float flashDuration;
    public Coroutine flashRoutine;

    public IEnumerator FlashRoutine(GameObject g)
    {
        g.GetComponent<SpriteRenderer>().material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        g.GetComponent<SpriteRenderer>().material = baseMaterial;
        flashRoutine = null;
    }

}

