using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using JAFprocedural;

public class BeanShooter : MonoBehaviour
{
    private GameObject player; // player so we can shoot at them
    private GameObject activeBullet; // husk used to init the bullets
    private float beanTimer = 0; //keeps track of firerate
    private Quaternion launch; // this is where the bullet is launched from
    private float launchAngle; //this tracks the player so we know when to shoot the bullet
    private bool LOS; //Used to track if we can see the player
    private LayerMask playerMask; //layer reference for player
    private LayerMask barrierMask; //layer reference for trees
    
    [Header("Animators & Sounds")]
    private Animator anim; //our animator
    private bool dead = false; //a bool to know to stop shooting while we are doing our dead animation

    private AudioSource audioSource;
    public AudioClip shoot1Sound;
    public AudioClip shoot2Sound;
    public AudioClip deathSound;

    [Header("Bean Shooter Stats")]
    public float health = 30; //bean shooter health
    public float damage = 1; //bullet damage
    public float speed = 5; //speed of the bullets
    public float beanRate = 2; //firerate
    public int rebound = 1;
    public GameObject bulletPrefab; //bullet prefab

    [Header("Flash Hit")]
    public Material flash;
    private Material material;
    public float flashDuration;
    private Coroutine flashRoutine;


    private float[] stats = new float[4];
    private bool instantiated = false;

    void OnEnable()
    {
        if (instantiated)
        {
            health = stats[0];
            damage = stats[1];
            speed = stats[2];
            beanRate = stats[3];

            dead = false;
            anim.SetBool("Death", false);
        }
    }

    void Start()
    {
        //so we can have the players transform
        player = GameObject.FindWithTag("Player");

        //find our layers used for our raycast masks
        playerMask |= 0x1 << 6;
        barrierMask |= 0x1 << 7;

        //find our animation and beginning rotation
        launch = gameObject.transform.rotation;
        anim = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();

        

        instantiated = true;
        Assign(health, 0);
        Assign(damage, 1);
        Assign(speed, 2);
        Assign(beanRate, 3);

        //grabs our material for flash effect
        material = gameObject.GetComponent<SpriteRenderer>().material;
        gameObject.GetComponent<SpriteRenderer>().material = material;
    }


    void Update()
    {
        //uses a little trig (arc tan) to calculate the angle to shoot at based on the players position and the launch position
        launchAngle = (Mathf.Atan2(player.transform.position.y - gameObject.transform.position.y, player.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg - 90f);

        //finds the player with a raycast and then raycasts up until the player looking for a tree
        RaycastHit2D playerRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 20, playerMask);         
        RaycastHit2D barrierRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, playerRay.distance, barrierMask);

        //If it can see the player but not see a tree it sets line of sight to true
        if(playerRay.collider != null){
            if(playerRay.collider.CompareTag("Player") && barrierRay.collider == null){
                LOS = true;

            }
            else{
                LOS = false;
                anim.SetBool("LOS", false);
            }
        }

        //only shoots when the firerate timer is done
        if(beanTimer <= 0 && LOS && !dead){

            //sets out launch angle and location
            launch = Quaternion.Euler(0, 0, launchAngle);

            //calls shooting and resets the firerate timer
            Shooting();

            beanTimer = beanRate;
        }

        //counts down until the next shot
        beanTimer -= Time.deltaTime;
    }


    //when shooting happens it inits the bullet and updates its variables
    public void Shooting(){    
        activeBullet = (GameObject)Instantiate(bulletPrefab, gameObject.transform.position, launch);
        activeBullet.GetComponent<BulletBehaviour>().bSpeed = speed;
        activeBullet.GetComponent<BulletBehaviour>().bDamage = damage;
        activeBullet.GetComponent<BulletBehaviour>().bRebound = rebound;
        anim.SetBool("LOS", true);
        
        if(RNG.GenRand(1, 2) == 1){audioSource.clip = shoot1Sound;}
        else{audioSource.clip = shoot2Sound;}
        audioSource.Play();       
    }

    //required by our observer interface but currently not used
    public void OnNotify(Vector3 pos){
        gameObject.transform.position = pos;
        dead = false;
    }

    IEnumerator Death()
    {
        dead = true;
        anim.SetBool("Death", true);
        audioSource.clip = deathSound;
        audioSource.Play();
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
