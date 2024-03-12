using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float bSpeed;
    public float bDamage;
    public bool bPlayer;

     // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * bSpeed;
    }

    void OnCollisionEnter2D(Collision2D other){
        if(bPlayer == true && other.gameObject.tag == "Player"){

        }
        else if(bPlayer == false && other.gameObject.tag == "Enemy"){
            
        }
        else{
            Destroy(gameObject);
        }
        
    }

}
