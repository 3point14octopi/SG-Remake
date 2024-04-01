using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAFprocedural;
using Unity.VisualScripting;

public class WispTemp : MonoBehaviour
{
    private bool dead = false;
    public int wispIndex = -1;
    public Vector3 addPosition = Vector3.zero;

    [Header("Wisp Stats")]
    public float health = 10;
    public float speed = 0.75f;
    public float damage = 10;

    private float[] stats = new float[3];
    private bool instantiated = false;

    public LayerMask cLayermask;

    void OnEnable()
    {
        if (instantiated)
        {
            health = stats[0];
            speed = stats[1];
            damage = stats[2];

            dead = false;
            Debug.Log("I LIVED, BITCH");
        }
        if(wispIndex == -1)wispIndex = WispManager.Instance.AddWisp(gameObject);
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
        RoomPop.Instance.EnemyKilled();
        WispManager.Instance.RemoveWisp(wispIndex);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }


    

    public IEnumerator RotateWisp(float wRot)
    {
        float rotVal = (wRot > 0) ? 1 : -1;
        for (float i = 0; i != wRot; i += rotVal)
        {
            transform.Rotate(0, 0, rotVal);
            yield return new WaitForNextFrameUnit();
        }
    }
}
