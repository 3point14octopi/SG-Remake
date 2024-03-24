using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TurkeyTemp : MonoBehaviour
{
    private bool dead = false;

    [Header("Turkey Stats")]
    public float health = 25;
    public float speed = 1.25f;
    public float damage = 20;

    private float[] stats = new float[3];
    private bool instantiated = false;

    private void OnEnable()
    {
        if (instantiated)
        {
            health = stats[0];
            speed = stats[1];
            damage = stats[2];

            dead = false;
            //anim.SetBool("Death", false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        instantiated = true;
        Assign(health, 0);
        Assign(speed, 1);
        Assign(damage, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        //checks if it is hit by a bullet from a player
        if (other.gameObject.tag == "PlayerBullet")
        {
            health = health - other.gameObject.GetComponent<PlayerBulletBehaviour>().bDamage;

            //if health is 0 destorys the object
            if (health <= 0 && !dead)
            {
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
        //anim.SetBool("Death", true);
        RoomPop.Instance.EnemyKilled();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
