using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    //all 3 of these things are updated by the person that calls them
    public float bSpeed; //bullet speed
    public float bDamage; //bullet damage
    public bool bPlayer; //if the player shot the bullet 1 for player 0 for enemy

     // Update is called once per frame
    void Update()
    {
        //moves the bullet continously in the direction
        transform.position += transform.right * Time.deltaTime * bSpeed;
    }

    void OnCollisionEnter2D(Collision2D other){
        // checks if it is a player bullet hitting themself
        if(bPlayer == true && other.gameObject.tag == "Player"){

        }
        //checks if it is an enemy bullet hitting an enemy
        else if(bPlayer == false && other.gameObject.tag == "Enemy"){
            
        }
        else if(other.gameObject.tag == "Bullet"){
            
        }
        //anything else the bullet explodes
        else{
        
            Destroy(gameObject);
        }
        
    }

}
