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


    public float health = 100; //bean shooter health
    public float damage = 50; //bullet damage
    public float speed = 2; //speed of the bullets
   


    void Start()
    {
        //so we can have the players transform
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //only shoots when the firerate timer is done
        if(beanTimer <= 0){
            //uses a little trig (arc tan) to calculate the angle to shoot at based on the players position and the launch position
            launch = gameObject.transform;
            float angle = Mathf.Atan2(player.transform.position.x - launch.position.x, player.transform.position.y - launch.position.y) * Mathf.Rad2Deg;
            Debug.Log(angle);
            //the negative and + 90 were because the bean was tracking the player but not quite looking at them
            launch.rotation = Quaternion.Euler(0, 0,-angle + 90);

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
