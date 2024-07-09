using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EntityStats;
using UnityEditor.Experimental.GraphView;

public class HazardBehaviour : MonoBehaviour
{
    //Different hazard types that are available
    public enum HazardType {
        Pit,
        Spikes,
    }
    public enum Directions{
        Up,
        Left,
        Down,
        Right
    }

    [Header("Damaging Player Variables")]
    [Tooltip("Should the hazard hurt the player?")]
    public bool hitPlayer;

    [Tooltip("Should it Suck you in or push you out?")]
    public HazardType hazardType; //lets our hazard know what type of reaction to have

    [Tooltip("How long is the player stunned for?")]
    public float stunTime;

    [Tooltip("How far back should the player be pushed?")]
    public float knockBack; //distance the player is pushed back in tiles

    private Vector3 knockBackLocation;


    [Header("\nMovement Variables")]
    [Tooltip("Does the hazard move?")]
    public bool moving;

    [Tooltip("Which direction should the spike move in?")]
    public Directions movementDirections;

    [Tooltip("How fast should it move")]
    public float speed;

    [Tooltip("Should it passthrough walls and other hazards?")]
    public bool passthrough;

    private Vector3 movementVector = new Vector3();


    [Header("\nDamageable Variables")]

    [Tooltip("Can the Hazard be broken?")]
    public bool breakable;

    [Tooltip("How much health does it have?")]
    public float health;

    [Tooltip("What reactions should it have when hit?")]
    [SerializeField] public List<Reaction> damageReactions = new List<Reaction>();

    [Tooltip("What reactions should it have when it is broken?")]
    [SerializeField] public List<Reaction> deathReactions = new List<Reaction>();

    void Start()
    {
        ChangeDirection(movementDirections);

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

    // Update is called once per frame
    void Update() { if (moving) transform.position += movementVector * Time.deltaTime * speed; }


    void OnTriggerEnter2D(Collider2D other){

        //damages the player if it is walked into by the player
        if (other.gameObject.tag == "Player" && hitPlayer)
        {
            other.gameObject.GetComponent<FbStateManager>().Stunned(stunTime);
            if (hazardType == HazardType.Spikes)
            {
                BounceOff(other);
            }

            else if (hazardType == HazardType.Pit)
            {
                StartCoroutine(FallIn(other));
            }

        }

        //takes damage from player bullets
        if (other.gameObject.tag == "PlayerBullet" && breakable)
        {
            Debug.Log("Ouchie");
            TakeDamage(other);
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
            else
            {
                ReactToHit();
            }
        }

        //if it bumps into a wall it will stop moving unless passthrough is enabled
        if((other.gameObject.tag == "Barrier" || other.gameObject.tag == "Hazard") && !passthrough)
        {
            speed = 0;
        }
    }





    //in order it turns off our game, calls the damage function, knocks the player back on a normalized vector, pauses time form
    void BounceOff(Collider2D other)
    {
        other.transform.position += new Vector3((other.transform.position.x - transform.position.x) * knockBack, (other.transform.position.y - transform.position.y) * knockBack, 0);
        StartCoroutine(other.gameObject.GetComponent<FbStateManager>().Stunned(stunTime));
    }

    IEnumerator FallIn(Collider2D other)
    {

        knockBackLocation = new Vector3((other.transform.position.x - transform.position.x) * knockBack, (other.transform.position.y - transform.position.y) * knockBack, 0);
        other.transform.position = transform.position;
        // other.gameObject.GetComponent<FbStateManager>().anim.Play("FrostbiteFall")
        StartCoroutine(other.gameObject.GetComponent<FbStateManager>().Stunned(stunTime));
        yield return new WaitForSeconds(stunTime);
        if (other.gameObject.GetComponent<FbBrain>().isAlive)
        {
            other.transform.position += knockBackLocation;
        }
    }


    #region Functions for being damaged
    void TakeDamage(Collider2D other)
    {
        health += other.gameObject.GetComponent<OnHit>().effects[0].modifier;
    }

    protected void ReactToHit()
    {
        foreach (Reaction r in damageReactions)
        {
            if (r.isCoroutine) StartReactCoroutine(r);
            else r.ReactFunction();
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

    private IEnumerator Die()
    {
        foreach (Reaction r in deathReactions)
        {
            if (r.isCoroutine) StartReactCoroutine(r);
            else r.ReactFunction();
        }
        yield return new WaitForSeconds(0);
        gameObject.SetActive(false);
    }

    #endregion

    //Used to set the direction the hazard moves in
    public void ChangeDirection(Directions direction)
    {
        switch ((int)direction)
        {
            case 0:
                {
                    movementVector = new Vector3(0, 1, 0);
                    break;
                }
            case 1:
                {
                    movementVector = new Vector3(-1, 0, 0);
                    break;
                }
            case 2:
                {
                    movementVector = new Vector3(0, -1, 0);
                    break;
                }
            case 3:
                {
                    movementVector = new Vector3(1, 0, 0);
                    break;
                }

        }
        movementDirections = direction;
    }
}

