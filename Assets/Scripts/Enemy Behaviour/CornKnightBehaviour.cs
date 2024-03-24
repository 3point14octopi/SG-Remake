using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CornKnightBehaviour : MonoBehaviour
{   
    private int direction = 1; //direction it is walking in
    private Animator anim;
    private bool dead = false;

    [Header("Tree Knight Stats")]
    public float health = 50; //enemy health
    public float speed = 1; //walk speed
    public float damage = 10; //damage it deals to player

    private float[] stats = new float[3];
    bool instantiated = false;

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

        instantiated = true;
        Assign(health, 0);
        Assign(speed, 1);
        Assign(damage, 2);
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

    IEnumerator Death()
    {
        anim.SetBool("Death", true);
        dead = true;
        RoomPop.Instance.EnemyKilled();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }



    void Assign(float val, int index)
    {
        float temp = val;
        stats[index] = temp;
    }
}
