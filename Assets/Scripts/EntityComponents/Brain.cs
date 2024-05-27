using EntityStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public bool isAlive = true;
    //public stats
    public bool showStats = true;
    public int[] Stats = new int[1];
    //internal stats
    private int[] currentStats;
    //stuff that damages the entity (probably player bullets)
    public List<string> damageTags = new List<string>();
    //reactions to taking damage and dying (two different things!)
    [SerializeField]public List<Reaction> damageReactions = new List<Reaction>();
    [SerializeField] public List<Reaction> deathReactions = new List<Reaction>();

    private void OnEnable()
    {
        isAlive = true;
        //this is so that we don't have any holdover from previously being alive
        currentStats = new int[Stats.Length];
        for (int i = 0; i < Stats.Length; currentStats[i] = Stats[i], i++) ;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Reaction r in damageReactions) r.OnStart(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAlive && damageTags.Contains(collision.gameObject.tag))
        {
            foreach (HitEffect effect in collision.gameObject.GetComponent<OnHit>().effects) ApplyOnHit(effect);
            if (currentStats[(int)EntityStat.Health] <= 0)
            {
                StartCoroutine(Die());
            }
            else
            {
                ReactToHit();
            }
        }
        
    }

    private void ApplyOnHit(HitEffect effect)
    {
        if((int)effect.targetedStat < Stats.Length)
        {
            currentStats[(int)effect.targetedStat] += effect.modifier;
        }
    }

    void StartReactCoroutine(Reaction r)
    {
        if (r.routine != null)
        {
            StopCoroutine(r.routine);
        }

        r.routine = StartCoroutine(r.ReactCoroutine());
    }

    private void ReactToHit()
    {
        foreach(Reaction r in damageReactions)
        {
            if (r.isCoroutine) StartReactCoroutine(r);
            else r.ReactFunction();
        }
    }

    private IEnumerator Die()
    {
        isAlive = false;
        Debug.Log("the entity has died. we'd do something here");
        foreach (Reaction r in deathReactions)
        {
            if (r.isCoroutine) StartReactCoroutine(r);
            else r.ReactFunction();
        }
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
