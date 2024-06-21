using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour
{
    //all 3 of these things are updated by the person that calls them
    public float bSpeed; //bullet speed
    public float bDamage; //bullet damage
    public int bRebound;

    // Update is called once per frame
    void Update()
    {
        //moves the bullet continously in the direction
        transform.position += transform.right * Time.deltaTime * bSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other){
        // checks if it is a player bullet hitting themself
        Debug.Log("Bullet Trigger acitvated");
        if(other.gameObject.tag == "Player"  || other.gameObject.tag == "PlayerBullet" || other.gameObject.tag == "EnemyBullet"){

        }
        else
        {
            if (bRebound > 0)
            {
                bRebound--;
                transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
            }
            else if(bRebound == 0){Destroy(gameObject);}
        }
    }
    public void SetBullet(Ammo a)
    {

        bSpeed = a.speed;
        bDamage = a.damage;
        bRebound = a.rebound;
    }
}
