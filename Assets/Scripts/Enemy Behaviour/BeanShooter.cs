using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanShooter : MonoBehaviour
{
    public GameObject player;
    public GameObject bulletPrefab;
    public GameObject activeBullet;

    public float beanRate = 1;//firerate
    public float beanTimer = 0;//keeps track of firerate
    public Transform launch;


    public float health = 100;
    public float damage = 50;
    public float speed = 2;
   

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(beanTimer <= 0){
            launch = gameObject.transform;
            float angle = Mathf.Atan2(player.transform.position.x - launch.position.x, player.transform.position.y - launch.position.y) * Mathf.Rad2Deg;
            Debug.Log(angle);
            launch.rotation = Quaternion.Euler(0, 0,-angle + 90);

            Shooting();
            beanTimer = beanRate;
        }

        beanTimer -= Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.tag == "Bullet" && other.gameObject.GetComponent<BulletBehaviour>().bPlayer == true)
        {
            health = health - other.gameObject.GetComponent<BulletBehaviour>().bDamage;
            if(health <= 0){
                Destroy(gameObject);
            }
        }
    }

    public void Shooting(){    
        activeBullet = (GameObject)Instantiate(bulletPrefab, gameObject.transform.position, launch.rotation);
        activeBullet.GetComponent<BulletBehaviour>().bSpeed = speed;
        activeBullet.GetComponent<BulletBehaviour>().bDamage = damage;
        activeBullet.GetComponent<BulletBehaviour>().bPlayer = false;          
    }
}
