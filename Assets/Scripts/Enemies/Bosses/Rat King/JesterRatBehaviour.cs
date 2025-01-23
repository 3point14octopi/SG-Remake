using EntityStats;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class JesterRatBehaviour : MonoBehaviour
{
    protected delegate void JesterState(); // our delegate variable type
    protected JesterState currentJesterState; // our current phase is stored in this delegate

    public GameObject player;
    private Vector2 playerPosition;
    private float chargeAngle;
    private float speed;
    public Animator anim;
    public Collider2D collider;

    private float waitTime; //how long we yield return new time
    private bool timeWaited = false; //used to know when the yield time is completed

    // Start is called before the first frame update
    private void Start()
    {
        currentJesterState = InAir;
    }

    private void FixedUpdate()
    {
        currentJesterState();
    }

    public void SpawnIn(GameObject p)
    {
        player = p;
        playerPosition = p.transform.position;
        gameObject.transform.position += new Vector3(0, 0, 1f); 
        speed = gameObject.GetComponent<Brain>().Stats[(int)EntityStat.Speed];
        gameObject.GetComponent<Brain>().ToggleIFrames(true);
        gameObject.transform.rotation = Quaternion.Euler( new Vector3(0, 0, Vector3.Angle(player.transform.position, gameObject.transform.position)));
        currentJesterState = InAir;
    }


    private void InAir()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, playerPosition) < 0.1f)
        {
            currentJesterState = Appear;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            anim.SetBool("Spawn", true);
            collider.enabled = true;
            WaitTime(1.2f);
        }
        
    }

    private void Appear()
    {
        if (timeWaited)
        {
            timeWaited = false;
            gameObject.GetComponent<Brain>().ToggleIFrames(true);
            currentJesterState = LockOn;
            WaitTime(1f);
        }
    }

    private void LockOn()
    {
        if(Mathf.Abs(player.transform.position.y - transform.position.y) > Mathf.Abs(player.transform.position.x - transform.position.x))
        {
            Debug.Log("player is vertical");
            if (player.transform.position.y - transform.position.y > 0) anim.Play("RatJesterLookUp");
            else anim.Play("RatJesterLookDown");
        }
        else
        {
            if (player.transform.position.x - transform.position.x > 0)
            {
                anim.Play("RatJesterLookRight");
                Debug.Log("player is to the right");
            }

            else anim.Play("RatJesterLookLeft");
        }
        if (timeWaited)
        {
            timeWaited = false;
            playerPosition = new Vector2( (player.transform.position.x - transform.position.x) * 100, (player.transform.position.y - transform.position.y) * 100);
            anim.SetBool("Charge", true);
            currentJesterState = Charge;
        }
    }

    private void Charge()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerPosition, speed * 1.5f * Time.deltaTime);
        //Vector2.MoveTowards
        // Chase logic goes here
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("Bonked", true);
        anim.SetBool("Charge", false);
        currentJesterState = Bonk;
        WaitTime(2.5f);
    }
    private void Bonk()
    {
        if (timeWaited)
        {
            timeWaited = false;
            anim.SetBool("Bonked", false);
            currentJesterState = LockOn;
            WaitTime(1f);

        }
    }
    public void WaitTime(float f)
    {
        waitTime = f;
        StartCoroutine(TimeWaiter());
    }
    public IEnumerator TimeWaiter()
    {
        yield return new WaitForSeconds(waitTime);
        timeWaited = true;
    }
}
