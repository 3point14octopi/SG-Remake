using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornKnightBehaviour : MonoBehaviour
{
    public float health = 100;
    public float speed = 1;
    public float damage = 50;
    public int direction = 1;


     // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * direction * Time.deltaTime * speed;


    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.tag == "Bullet")
        {
            health = health - other.gameObject.GetComponent<BulletBehaviour>().bDamage;

            if(health <= 0){
                Destroy(gameObject);
            }
        }
        else{
            direction = -direction;
        }
    }
}
