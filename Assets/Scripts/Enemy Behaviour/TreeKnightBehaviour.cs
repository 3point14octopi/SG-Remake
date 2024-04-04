using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using JAFprocedural;

public class TreeKnightBehaviour : MonoBehaviour
{   
    [Header("Animators & Sounds")]
    private int direction = 1; //direction it is walking in
    private Animator anim;
    private bool dead = false;
    private AudioSource audioSource;
    public AudioClip walk1Sound;
    public AudioClip walk2Sound;
    public AudioClip deathSound;


    [Header("Tree Knight Stats")]
    public float health = 50; //enemy health
    public float speed = 1; //walk speed
    public float damage = 10; //damage it deals to player

    private float[] stats = new float[3];
    bool instantiated = false;

    [Header("Flash Hit")]
    public Material flash;
    private Material material;
    public float flashDuration;
    private Coroutine flashRoutine;

    private void OnEnable()
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
        //find our animation
        anim = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();

        instantiated = true;
        Assign(health, 0);
        Assign(speed, 1);
        Assign(damage, 2);

        //grabs our material for flash effect
        material = gameObject.GetComponent<SpriteRenderer>().material;
        gameObject.GetComponent<SpriteRenderer>().material = material;
    }

     // Update is called once per frame
    void Update()
    {
        if(!dead){
            //walk in the current direction
            transform.position += transform.right * direction * Time.deltaTime * speed;
 
        }
        if(audioSource.isPlaying == false){
            if(RNG.GenRand(1, 2) == 1){audioSource.clip = walk1Sound;}
            else{audioSource.clip = walk2Sound;}
            audioSource.Play();  
        }

    }

    void OnCollisionEnter2D(Collision2D other){
        //if the collision was a wall it switches directions
        if (other.gameObject.tag == "Barrier"){
            direction = -direction;
            anim.SetFloat("Direction", direction);
        }

        //if it is hit by a bullet it takes damage
        else if (other.gameObject.tag == "PlayerBullet")
        {
            health = health - other.gameObject.GetComponent<PlayerBulletBehaviour>().bDamage;
            Flash();
            //if the damage is too much the enemy dies
            if(health <= 0 && !dead){
                StartCoroutine(Death());
            }
        }
        
        //damages the player if we wall into the player
        else if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<FbStateManager>().TakeDamage(damage);
        }
    }

    IEnumerator Death()
    {
        dead = true;
        anim.SetBool("Death", true);
        audioSource.clip = deathSound;
        audioSource.Play();
        RoomPop.Instance.EnemyKilled();
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<SpriteRenderer>().material = material;
        gameObject.SetActive(false);
    }



    void Assign(float val, int index)
    {
        float temp = val;
        stats[index] = temp;
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
