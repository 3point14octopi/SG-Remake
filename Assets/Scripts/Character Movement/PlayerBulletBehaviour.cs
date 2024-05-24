using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour
{
    //all 3 of these things are updated by the person that calls them
    public float bSpeed; //bullet speed
    public float bDamage; //bullet damage

     // Update is called once per frame
    void Update()
    {
        //moves the bullet continously in the direction
        transform.position += transform.right * Time.deltaTime * bSpeed;
    }

    void OnCollisionEnter2D(Collision2D other){
        // checks if it is a player bullet hitting themself
        if(other.gameObject.tag == "Player"  || other.gameObject.tag == "PlayerBullet" || other.gameObject.tag == "EnemyBullet"){

        }
        //anything else the bullet explodes
        else{
        
            Destroy(gameObject);
        }
    }
}
