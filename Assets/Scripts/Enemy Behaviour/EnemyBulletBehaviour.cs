using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    //all 3 of these things are updated by the person that calls them
    public float bSpeed; //bullet speed
    public int bDamage; //bullet damage

     // Update is called once per frame
    void Update()
    {
        //moves the bullet continously in the direction
        transform.position += transform.up * Time.deltaTime * bSpeed;
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "PlayerBullet" || other.gameObject.tag == "EnemyBullet"){
            
        }
        //anything else the bullet explodes
        else{
        
            Destroy(gameObject);
        }        
    }

}
