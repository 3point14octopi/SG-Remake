using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAFprocedural;

public class TurkeyBehaviour : MonoBehaviour
{
    private GameObject player; // player so we can shoot at them
    public GameObject shadow; 


    [Header("Animators & Sounds")]
    private Animator anim;
    private bool dead = false;

    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip deathSound;

    [Header("Turkey Stats")]
    public float health = 50; //enemy health
    public float speed = 1; //walk speed
    public float damage = 2; //damage it deals to player 
    

    private float[] stats = new float[3];
    private bool instantiated = false;

    [Header("Flash Hit")]
    public Material flash;
    private Material material;
    public float flashDuration;
    private Coroutine flashRoutine;

     //this lets us reset the ghost by re-enabling the game object without having to hardcode our stats
    void OnEnable()
    {
        if (instantiated)
        {
            health = stats[0];
            speed = stats[1];
            damage = stats[2];

            dead = false;
            anim.SetBool("Death", false);
        }
    }

    void Start()
    {
        //spooky stuff
        instantiated = true;
        Assign(health, 0);
        Assign(speed, 1);
        Assign(damage, 2);

        //so we can have the players transform
        player = GameObject.FindWithTag("Player");

        //find our animation
        anim = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();

        //grabs our material for flash effect
        material = gameObject.GetComponent<SpriteRenderer>().material;
        gameObject.GetComponent<SpriteRenderer>().material = material;

        StartCoroutine(TurkeyStart());
    }

    IEnumerator TurkeyStart()
    {
        yield return new WaitForSeconds(RNG.GenRand(1, 3));
        StartCoroutine(TurkeyJump());

    }

    IEnumerator TurkeyJump()
    {
        anim.Play("TurkeyJump");
        audioSource.clip = jumpSound;
        audioSource.Play();
        yield return new WaitForSeconds(1);

        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(TurkeyLand());
        yield return null;

    }

    IEnumerator TurkeyLand()
    {
        shadow.SetActive(true);
        gameObject.transform.position = player.transform.position;
        yield return new WaitForSeconds(1);
        shadow.SetActive(false);
        anim.Play("TurkeyLand");


        yield return new WaitForSeconds(0.6f);
        audioSource.clip = landSound;
        audioSource.Play();
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<CircleCollider2D>().radius = 0.4f;
        
        yield return new WaitForSeconds(0.4f);
        gameObject.GetComponent<CircleCollider2D>().radius = 0.2f;
   
        yield return new WaitForSeconds(2f);
        StartCoroutine(TurkeyJump());
        yield return null;
    }

    void OnTriggerEnter2D(Collider2D other){
        //checks if it is hit by a bullet from a player
        if (other.gameObject.tag == "PlayerBullet"  && !dead)
        {
            health = health - other.gameObject.GetComponent<PlayerBulletBehaviour>().bDamage;
            Destroy(other.gameObject);
            Flash();
            //if health is 0 destorys the object
            if(health <= 0){
                StartCoroutine(Death());
            }
        }

        //damages the player if we wall into the player
        else if (other.gameObject.tag == "Player" && !dead)
        {
            other.gameObject.GetComponent<FbStateManager>().TakeDamage(damage);
        }
    }

    
    void Assign(float val, int index)
    {
        float temp = val;
        stats[index] = temp;
    }



    IEnumerator Death()
    {
        dead = true;
        anim.Play("TurkeyDeath");
        audioSource.clip = deathSound;
        audioSource.Play();
        RoomPop.Instance.EnemyKilled();
        yield return new WaitForSeconds(1.25f);
        gameObject.GetComponent<SpriteRenderer>().material = material;
        gameObject.SetActive(false);
    }

    
    IEnumerator FlashRoutine(){

        gameObject.GetComponent<SpriteRenderer>().material = flash;
        
        yield return new WaitForSeconds(flashDuration);

        gameObject.GetComponent<SpriteRenderer>().material = material;

        flashRoutine = null;

    }

    void Flash(){
        if(flashRoutine != null){
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }
}
