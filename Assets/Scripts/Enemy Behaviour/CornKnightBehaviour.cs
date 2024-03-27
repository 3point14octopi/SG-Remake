using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornKnightBehaviour : MonoBehaviour, EnemyObserver
{   
    private int direction = 1; //direction it is walking in
    private Animator anim;
    private bool dead = false;
    private EnemySubject enemyManager; 

    [Header("Tree Knight Stats")]
    public int health = 50; //enemy health
    public float speed = 1; //walk speed
    public int damage = 1; //damage it deals to player


    void Start()
    {
        //find our animation
        anim = gameObject.GetComponent<Animator>();

        //find our subject and become an observer of it
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemySubject>();
        enemyManager.AddObserver(this);
    }

     // Update is called once per frame
    void Update()
    {
        if(!dead){
            //walk in the current direction
            transform.position += transform.right * direction * Time.deltaTime * speed;
 
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
            
            //if the damage is too much the enemy dies
            if(health <= 0){
                anim.SetBool("Death", true);
                enemyManager.RemoveObserver(this);
                dead = true;
                Destroy(gameObject, 1.00f);
            }
        }
        
        //damages the player if we wall into the player
        else if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<FbStateManager>().TakeDamage(damage);
        }
    }

    //required by our observer interface but currently not used
    public void OnNotify(){
        
    }
}
