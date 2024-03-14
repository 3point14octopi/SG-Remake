using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornKnightBehaviour : MonoBehaviour, EnemyObserver
{
    public float health = 100; //enemy health
    public float speed = 1; //walk speed
    public float damage = 50; //damage it deals to player
    public int direction = 1; //direction it is walking in

   [SerializeField] EnemySubject enemyManager; 

    void Start()
    {
        //find our subject
        enemyManager.AddObserver(this);
    }

     // Update is called once per frame
    void Update()
    {
        //walk in the current direction
        transform.position += transform.right * direction * Time.deltaTime * speed;


    }

    void OnCollisionEnter2D(Collision2D other){
        //if the collision was a wall it switches directions
        if (other.gameObject.tag == "Barrier"){
            direction = -direction;
        }

        //if it is hit by a bullet it takes damage
        else if (other.gameObject.tag == "PlayerBullet")
        {
            health = health - other.gameObject.GetComponent<PlayerBulletBehaviour>().bDamage;
            
            //if the damage is too much the enemy dies
            if(health <= 0){
                enemyManager.RemoveObserver(this);
                Destroy(gameObject);
            }
        }
    }

    public void OnNotify(){
        
    }
}
