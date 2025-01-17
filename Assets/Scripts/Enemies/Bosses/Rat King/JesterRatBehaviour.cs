using EntityStats;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;

public class JesterRatBehaviour : MonoBehaviour
{
    protected delegate void JesterState(); // our delegate variable type
    protected JesterState currentJesterState; // our current phase is stored in this delegate

    public GameObject player;
    private Vector2 playerPosition;
    private float speed;
    public Animator anim;
    public Collider2D collider;

    private bool spawnAnimComplete = false; //used to know when 1.2 seconds has passed and the rat can move

    // Start is called before the first frame update
    private void Start()
    {
        currentJesterState = InAir;
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

    // Update is called once per frame
    private void FixedUpdate()
    {
        currentJesterState();
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
            StartCoroutine(SpawnAnimation());
        }
        
    }

    private void Appear()
    {
        if (spawnAnimComplete)
        {
            gameObject.GetComponent<Brain>().ToggleIFrames(true);
            currentJesterState = Chase;
        }
    }

    private void Chase()
    {
        // Chase logic goes here
    }
    public IEnumerator SpawnAnimation()
    {
        yield return new WaitForSeconds(1.2f);
        spawnAnimComplete = true;
    }
}
