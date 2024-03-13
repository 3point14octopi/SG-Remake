using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornKnightBehaviour : MonoBehaviour
{
    public float health = 100; //enemy health
    public float speed = 1; //walk speed
    public float damage = 50; //damage it deals to player
    public int direction = 1; //direction it is walking in


     // Update is called once per frame
    void Update()
    {
        //walk in the current direction
        transform.position += transform.right * direction * Time.deltaTime * speed;


    }

    void OnCollisionEnter2D(Collision2D other){
        //if it is hit by a bullet it takes damage
        if (other.gameObject.tag == "Bullet" && other.gameObject.GetComponent<BulletBehaviour>().bPlayer == true)
        {
            health = health - other.gameObject.GetComponent<BulletBehaviour>().bDamage;
            
            //if the damage is too much the enemy dies
            if(health <= 0){
                Destroy(gameObject);
            }
        }
        //if the collision was not a bullet it switches directions
        else{
            direction = -direction;
        }
    }
}
