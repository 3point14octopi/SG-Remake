using EntityStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public int[] Stats = new int[1];
    [Header("the tags of things that hurt this entity")]
    public List<string> damageTags = new List<string>();
    
    public Flash flashObj;
    // Start is called before the first frame update
    void Start()
    {
        flashObj.baseMaterial = GetComponent<SpriteRenderer>().material;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damageTags.Contains(collision.gameObject.tag))
        {
            foreach (HitEffect effect in collision.gameObject.GetComponent<OnHit>().effects) ApplyOnHit(effect);
            Flash();
        }
    }

    private void ApplyOnHit(HitEffect effect)
    {
        if((int)effect.targetedStat < Stats.Length)
        {
            Stats[(int)effect.targetedStat] += effect.modifier;
        }
    }

    void Flash()
    {
        if (flashObj.flashRoutine != null)
        {
            StopCoroutine(flashObj.flashRoutine);
        }

        flashObj.flashRoutine = StartCoroutine(flashObj.FlashRoutine(gameObject));
    }
}
