using JAFprocedural;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface PrincePhase
{

    void Update(PrinceTemp p);
    void PickAttack(PrinceTemp p);
    void UpdateState(PrinceTemp p);
}

public class HighHealth : PrincePhase
{
    float sinceLastAttack = 4f;
    public void Update(PrinceTemp p)
    {
        Debug.Log("HIGH HEALTH");
        if(p.health <= 900)
        {
            sinceLastAttack = 0f;
            UpdateState(p);
        }
        else
        {
            if(sinceLastAttack >= 7f)
            {
                PickAttack(p);
                sinceLastAttack = 0f;
            }
            else
            {
                Debug.Log(sinceLastAttack.ToString());
                sinceLastAttack += Time.deltaTime;
            }
        }
        
    }

    public void PickAttack(PrinceTemp p)
    {
        if(RNG.GenRand(1, 5) > 3)
        {
            p.princeAnim.Play("Pumpkin Prince Vine Attack");
            BossRoomManager.Instance.princeAttacks.VineWaves();
        }
        else
        {
            p.princeAnim.Play("Pumpkin Prince Fire Attack");
            BossRoomManager.Instance.princeAttacks.FireRain();
        }
    }

    public void UpdateState(PrinceTemp p)
    {
        p.phase = new MediumHealth();
    }
}

public class MediumHealth : PrincePhase
{
    float sinceLastAttack = 0f;
    public void Update(PrinceTemp p)
    {

        if (p.health <= 450)
        {
            sinceLastAttack = 0f;
            UpdateState(p);
        }
        else
        {
            if (sinceLastAttack >= 7f)
            {
                PickAttack(p);
                sinceLastAttack = 0f;
            }
            else
            {
                sinceLastAttack += Time.deltaTime;
            }
        }

    }

    public void PickAttack(PrinceTemp p)
    {
        if (RNG.GenRand(1, 5) > 3)
        {
            p.princeAnim.Play("Pumpkin Prince Fire Attack");            
            BossRoomManager.Instance.princeAttacks.FireRain();
        }
        else
        {
            p.princeAnim.Play("Pumpkin Prince Vine Attack");
            BossRoomManager.Instance.princeAttacks.VineWaves();
        }
    }

    public void UpdateState(PrinceTemp p)
    {
        p.phase = new LowHealth();
    }
}

public class LowHealth : PrincePhase
{
    float sinceLastAttack = 0f;
    public void Update(PrinceTemp p)
    {

        if (p.health <= 0)
        {
            sinceLastAttack = 0f;
            UpdateState(p);
        }
        else
        {
            if (sinceLastAttack >= 7f)
            {
                PickAttack(p);
                sinceLastAttack = 0f;
            }
            else
            {
                sinceLastAttack += Time.deltaTime;
            }
        }

    }

    public void PickAttack(PrinceTemp p)
    {
        int attack = RNG.GenRand(1, 5);
        if (attack <= 3)
        {
            p.princeAnim.Play("Pumpkin Prince Lantern Attack");            
            BossRoomManager.Instance.princeAttacks.HorizontalLasers();
        }
        else if(attack <= 4)
        {
            p.princeAnim.Play("Pumpkin Prince Fire Attack");
            BossRoomManager.Instance.princeAttacks.FireRain();
        }
        else
        {
            p.princeAnim.Play("Pumpkin Prince Vine Attack");
            BossRoomManager.Instance.princeAttacks.VineWaves();
        }
    }

    public void UpdateState(PrinceTemp p)
    {
        p.Die();
    }
}





public class PrinceTemp : MonoBehaviour
{
    private bool dead = false;

    [Header("Pumpkin Prince Stats")]
    public float maxHealth = 1350;
    public float health = 1350;
    public float speed = 0;
    public float contactDamage = 1;

    private float[] stats = new float[3] { 1350, 0, 1 };
    private bool instantiated = false;
    public PrincePhase phase = new HighHealth();

    [Header("Animators")]
    public Animator princeAnim;


    private void OnEnable()
    {
        if (instantiated)
        {
            health = stats[0];
            speed = stats[1];
            contactDamage = stats[2];
            dead = false;
        }
    }
    // Start is called before the first frame update
    private void Awake()
    {
        Debug.Log("can spawn");
    }
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
        if (!dead)
        {
            Debug.Log("HIGH HEALTH UPDATING");
            phase.Update(this);
        }
        else
        {
            Debug.Log("HIGH HEALTH ISN'T UPDATING");
        }
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
                Die();
            }
        }

        //damages the player if we wall into the player
        else if (other.gameObject.tag == "Player" && !dead)
        {
            other.gameObject.GetComponent<FbStateManager>().TakeDamage(contactDamage);
        }
    }

    public void Die()
    {
        princeAnim.Play("Pumpkin Prince Death");
        Debug.Log("HE FUCKIN DIED");
        StartCoroutine(Death());
    }


    IEnumerator Death()
    {
        dead = true;
        RoomPop.Instance.EnemyKilled();
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }

    void Assign(float value, int index)
    {
        float temp = value;
        stats[index] = temp;
    }

}
