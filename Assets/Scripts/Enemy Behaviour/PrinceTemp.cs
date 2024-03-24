using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceTemp : MonoBehaviour
{
    private bool dead = false;

    [Header("Pumpkin Prince Stats")]
    public float health = 150;
    public float speed = 0;
    public float contactDamage = 1;

    private float[] stats = new float[3];
    private bool instantiated = false;


    private void OnEnable()
    {
        if (instantiated)
        {
            health = stats[0];
            speed = stats[1];
            contactDamage = stats[2];
            dead = false;
            //anim.SetBool("Death", false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        instantiated = true;
        Assign(health, 0);
        Assign(speed, 0);
        Assign(contactDamage, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //if it is hit by a bullet it takes damage
        if (other.gameObject.tag == "PlayerBullet")
        {
            health = health - other.gameObject.GetComponent<PlayerBulletBehaviour>().bDamage;

            //if the damage is too much the enemy dies
            if (health <= 0 && !dead)
            {
                StartCoroutine(Death());
            }
        }

        //damages the player if we wall into the player
        else if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<FbStateManager>().health = other.gameObject.GetComponent<FbStateManager>().health - contactDamage;
        }
    }

    void Assign(float value, int index)
    {
        float temp = value;
        stats[index] = temp;
    }

    IEnumerator Death()
    {
        //anim.SetBool("Death", true);
        dead = true;
        RoomPop.Instance.EnemyKilled();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
