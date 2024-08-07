using EntityStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public bool isAlive = true;
    //public stats
    public bool showStats = true;
    public float[] Stats = new float[3];
    //internal stats
    [SerializeField] public float[] currentStats { get; protected set; }
    

    [Tooltip("What should this take damage from?")]
    public List<string> damageTags = new List<string>();
    [Tooltip("What should this do when it takes damage?")]
    [SerializeField]public List<Reaction> damageReactions = new List<Reaction>();
    [Tooltip("What should this do when it dies?")]
    [SerializeField] public List<Reaction> deathReactions = new List<Reaction>();

    //room stuff
    public RoomTemplate mom;
    public int roomIndex = -1;

    private void OnEnable()
    {
        isAlive = true;
        roomIndex = -1;
        //this is so that we don't have any holdover from previously being alive
        currentStats = new float[Stats.Length];
        for (int i = 0; i < Stats.Length; currentStats[i] = Stats[i] + 0, i++) ;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < damageReactions.Count; i++)
        {
            damageReactions[i] = Instantiate(damageReactions[i]);
            damageReactions[i].OnStart(gameObject);
        }

        for (int i = 0; i < deathReactions.Count; i++)
        {
            deathReactions[i] = Instantiate(deathReactions[i]);
            deathReactions[i].OnStart(gameObject);
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

    protected void StartReactCoroutine(Reaction r)
    {
        if (r.routine != null)
        {
            StopCoroutine(r.routine);
        }

        r.routine = StartCoroutine(r.ReactCoroutine());
    }

    protected void ReactToHit()
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
        
        if(roomIndex != -1)
        {
            mom.RemoveEnemy(roomIndex);
        }
        foreach (Reaction r in deathReactions)
        {
            if (r.isCoroutine) StartReactCoroutine(r);
            else r.ReactFunction();
        }
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
