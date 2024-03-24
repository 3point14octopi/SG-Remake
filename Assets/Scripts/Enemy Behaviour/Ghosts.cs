using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghosts : MonoBehaviour
{
    
    private GameObject player; // player so we can shoot at them

    private bool LOS; //Used to track if we can see the player
    private LayerMask playerMask; //layer reference for player
    private LayerMask barrierMask; //layer reference for trees

    private Animator anim;
    private bool dead = false;

    [Header("Ghost Stats")]
    public float health = 50; //enemy health
    public float speed = 1; //walk speed
    public float damage = 10; //damage it deals to player 


    private float[] stats = new float[3];
    private bool instantiated = false;

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

    // Start is called before the first frame update
    void Start()
    {
        //so we can have the players transform
        player = GameObject.FindWithTag("Player");

        //find our layers used for our raycast masks
        playerMask |= 0x1 << 6;
        barrierMask |= 0x1 << 7;

        //find our animation
        anim = gameObject.GetComponent<Animator>();

        

        //spooky stuff
        instantiated = true;
        Assign(health, 0);
        Assign(speed, 1);
        Assign(damage, 2);
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
                LOS = true;
            }
            else{
                LOS = false;
            }
        }
        
        //if we have line of sight we walk towards the player
        if(LOS && !dead){       
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            
            //Rest of the stuff in this loop is used for the animation paramaters. Determines what walking direction animation to use
            anim.SetFloat("playerX",player.transform.position.x - transform.position.x);
            anim.SetFloat("playerY",player.transform.position.y - transform.position.y);

            if(Mathf.Abs(anim.GetFloat("playerX")) > Mathf.Abs(anim.GetFloat("playerY"))){ anim.SetBool("Vertical", false);}
            else{anim.SetBool("Vertical", true);}
        }


    }

    void OnCollisionEnter2D(Collision2D other){
        //checks if it is hit by a bullet from a player
        if (other.gameObject.tag == "PlayerBullet")
        {
            health = health - other.gameObject.GetComponent<PlayerBulletBehaviour>().bDamage;

            //if health is 0 destorys the object
            if(health <= 0 && !dead){
                StartCoroutine(Death());
            }
        }

        //damages the player if we wall into the player
        else if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<FbStateManager>().health = other.gameObject.GetComponent<FbStateManager>().health - damage;
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
        anim.SetBool("Death", true);
        RoomPop.Instance.EnemyKilled();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
