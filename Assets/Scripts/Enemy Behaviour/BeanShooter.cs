using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanShooter : MonoBehaviour
{
    public GameObject player; // player so we can shoot at them
    public GameObject bulletPrefab; //bullet prefab
    public GameObject activeBullet; // husk used to init the bullets

    public float beanRate = 1; //firerate
    public float beanTimer = 0; //keeps track of firerate
    public Transform launch; // this is where the bullet is launched from
    public float launchAngle; //this tracks the player so we know when to shoot the bullet
    public bool LOS; //Used to track if we can see the player
    public LayerMask playerMask; //layer reference for player
    public LayerMask barrierMask; //layer reference for trees


    public float health = 100; //bean shooter health
    public float damage = 50; //bullet damage
    public float speed = 2; //speed of the bullets
   

    void Start()
    {
        //so we can have the players transform
        player = GameObject.FindWithTag("Player");
        launch = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //uses a little trig (arc tan) to calculate the angle to shoot at based on the players position and the launch position
        launchAngle = -(Mathf.Atan2(player.transform.position.x - launch.position.x, player.transform.position.y - launch.position.y) * Mathf.Rad2Deg) + 90;

        //finds the player with a raycast and then raycasts up until the player looking for a tree
        RaycastHit2D playerRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 20, playerMask);         
        RaycastHit2D barrierRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, playerRay.distance, barrierMask);

        //If it can see the player but not see a tree it sets line of sight to true
        if(playerRay.collider != null){
            if(playerRay.collider.CompareTag("Player") && barrierRay.collider == null){
                //Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
                LOS = true;
            }
            else{
                //Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
                LOS = false;
            }
        }

        //only shoots when the firerate timer is done
        if(beanTimer <= 0 && LOS){

            //sets out launch angle and location
            launch.rotation = Quaternion.Euler(0, 0, launchAngle);

            //calls shooting and resets the firerate timer
            Shooting();
            beanTimer = beanRate;
        }

        //counts down until the next shot
        beanTimer -= Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D other){
        //checks if it is hit by a bullet from a player
        if (other.gameObject.tag == "Bullet" && other.gameObject.GetComponent<BulletBehaviour>().bPlayer == true)
        {
            health = health - other.gameObject.GetComponent<BulletBehaviour>().bDamage;

            //if health is 0 destorys the object
            if(health <= 0){
                Destroy(gameObject);
            }
        }
    }

    //when shooting happens it inits the bullet and updates its variables
    public void Shooting(){    
        activeBullet = (GameObject)Instantiate(bulletPrefab, gameObject.transform.position, launch.rotation);
        activeBullet.GetComponent<BulletBehaviour>().bSpeed = speed;
        activeBullet.GetComponent<BulletBehaviour>().bDamage = damage;
        activeBullet.GetComponent<BulletBehaviour>().bPlayer = false;          
    }
}
