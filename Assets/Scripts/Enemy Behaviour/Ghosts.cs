using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghosts : MonoBehaviour, EnemyObserver
{
    [SerializeField] EnemySubject enemyManager;
    public float health = 100; //enemy health
    public float speed = 1; //walk speed
    public float damage = 50; //damage it deals to player 

    public Rigidbody2D rb; //player rigidbody
    public GameObject player; // player so we can shoot at them
    public bool LOS; //Used to track if we can see the player
    public LayerMask playerMask; //layer reference for player
    public LayerMask barrierMask; //layer reference for trees

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyManager.AddObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        //finds the player with a raycast and then raycasts up until the player looking for a tree
        RaycastHit2D playerRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 20, playerMask);         
        RaycastHit2D barrierRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, playerRay.distance, barrierMask);

        //If it can see the player but not see a tree it sets line of sight to true
        if(playerRay.collider != null){
            if(playerRay.collider.CompareTag("Player") && barrierRay.collider == null){
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
                LOS = true;
            }
            else{
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
                LOS = false;
            }
        }
        
        //if we have line of sight we walk towards the player
        if(LOS){       
         transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

        void OnCollisionEnter2D(Collision2D other){
        //checks if it is hit by a bullet from a player
        if (other.gameObject.tag == "PlayerBullet")
        {
            health = health - other.gameObject.GetComponent<PlayerBulletBehaviour>().bDamage;

            //if health is 0 destorys the object
            if(health <= 0){
                enemyManager.RemoveObserver(this);
                Destroy(gameObject);
            }
        }
    }
    
    public void OnNotify(){
        
    }
}
