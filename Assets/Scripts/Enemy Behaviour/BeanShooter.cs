using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanShooter : MonoBehaviour, EnemyObserver
{
    private GameObject player; // player so we can shoot at them
    private GameObject activeBullet; // husk used to init the bullets
    private float beanTimer = 0; //keeps track of firerate
    private Quaternion launch; // this is where the bullet is launched from
    private float launchAngle; //this tracks the player so we know when to shoot the bullet
    private bool LOS; //Used to track if we can see the player
    private LayerMask playerMask; //layer reference for player
    private LayerMask barrierMask; //layer reference for trees
    private EnemySubject enemyManager; //our manager to tell when we are dead
    
    //Animation
    private Animator anim; //our animator
    private bool dead = false; //a bool to know to stop shooting while we are doing our dead animation

    [Header("Bean Shooter Stats")]
    public float health = 100; //bean shooter health
    public float damage = 50; //bullet damage
    public float speed = 2; //speed of the bullets
    public float beanRate = 1; //firerate
    public GameObject bulletPrefab; //bullet prefab
   



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

        //find our subject and become an observer of it
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemySubject>();
        enemyManager.AddObserver(this);
    }


    void Update()
    {
        //uses a little trig (arc tan) to calculate the angle to shoot at based on the players position and the launch position
        launchAngle = (Mathf.Atan2(player.transform.position.y - gameObject.transform.position.y, player.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg);

        //finds the player with a raycast and then raycasts up until the player looking for a tree
        RaycastHit2D playerRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 20, playerMask);         
        RaycastHit2D barrierRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, playerRay.distance, barrierMask);

        //If it can see the player but not see a tree it sets line of sight to true
        if(playerRay.collider != null){
            if(playerRay.collider.CompareTag("Player") && barrierRay.collider == null){
                LOS = true;
                anim.SetBool("LOS", true);
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

    void OnCollisionEnter2D(Collision2D other){
        //checks if it is hit by a bullet from a player
        if (other.gameObject.tag == "PlayerBullet")
        {
            health = health - other.gameObject.GetComponent<PlayerBulletBehaviour>().bDamage;

            //if health is 0; start death anim, tell subject to take us off the list and delete the gameobject after the anim is done
            if(health <= 0){
                anim.SetBool("Death", true);
                enemyManager.RemoveObserver(this);
                dead = true;
                Destroy(gameObject, 1.00f);
            }
        }
    }

    //when shooting happens it inits the bullet and updates its variables
    public void Shooting(){    
        activeBullet = (GameObject)Instantiate(bulletPrefab, gameObject.transform.position, launch);
        activeBullet.GetComponent<EnemyBulletBehaviour>().bSpeed = speed;
        activeBullet.GetComponent<EnemyBulletBehaviour>().bDamage = damage;          
    }

    //required by our observer interface but currently not used
    public void OnNotify(){
        
    }
}
