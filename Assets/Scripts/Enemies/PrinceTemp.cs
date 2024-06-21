using JAFprocedural;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public interface PrincePhase
{
    void Update(PrinceTemp p);
    void UpdateState(PrinceTemp p);
}

public class HighHealth : PrincePhase
{
    float sinceLastAttack = 0f;
    float fireCount = 0f;
    public void Update(PrinceTemp p)
    {
        if(p.health <= 900)
        {
            sinceLastAttack = 0f;
            fireCount = 0f;
            UpdateState(p);
        }
        else
        {
            if (fireCount >= 14f)
            {
                if (!BossRoomManager.Instance.princeAttacks.IsFireballing())
                {
                    p.princeAnim.Play("Pumpkin Prince Fire Attack");
                    p.audioSource.clip = p.meteorSound;
                    p.audioSource.Play(); 
                    BossRoomManager.Instance.princeAttacks.FireRain();
                    fireCount = 0f;
                }
                else
                {
                    fireCount -= 3;
                }
            }
            else if (sinceLastAttack >= 10f)
            {
                p.princeAnim.Play("Pumpkin Prince Vine Attack");
                p.audioSource.clip = p.vineSound;
                p.audioSource.Play(); 
                BossRoomManager.Instance.princeAttacks.VineWaves();
                sinceLastAttack = 0f;
            }
            else
            {
                sinceLastAttack += Time.deltaTime;
                fireCount += Time.deltaTime;
            }
        }
        
    }

    public void UpdateState(PrinceTemp p)
    {
        p.phase = new MediumHealth();
        BossRoomManager.Instance.princeAttacks.ActivateBouncer();
    }
}

public class MediumHealth : PrincePhase
{
    float sinceLastAttack = 0f;
    float fireCount = 0f;
    public void Update(PrinceTemp p)
    {

        if (p.health <= 450)
        {
            sinceLastAttack = 0f;
            UpdateState(p);
        }
        else
        {
            if (fireCount >= 12f)
            {
                if (!BossRoomManager.Instance.princeAttacks.IsFireballing())
                {
                    p.princeAnim.Play("Pumpkin Prince Fire Attack");
                    p.audioSource.clip = p.meteorSound;
                    p.audioSource.Play(); 
                    BossRoomManager.Instance.princeAttacks.FireRain();
                    fireCount = 0f;
                }
                else
                {
                    fireCount -= 3;
                }
            }
            else if (sinceLastAttack >= 10f)
            {
                p.princeAnim.Play("Pumpkin Prince Vine Attack");
                p.audioSource.clip = p.vineSound;
                p.audioSource.Play(); 
                BossRoomManager.Instance.princeAttacks.VineWaves();
                sinceLastAttack = 0f;
            }
            else
            {
                sinceLastAttack += Time.deltaTime;
                fireCount += Time.deltaTime;
            }
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
    float fireCount = 5f;
    public void Update(PrinceTemp p)
    {

        if (p.health <= 0)
        {
            sinceLastAttack = 0f;
            UpdateState(p);
        }
        else
        {
            if(fireCount >= 10f)
            {
                if (!BossRoomManager.Instance.princeAttacks.IsFireballing())
                {
                    p.princeAnim.Play("Pumpkin Prince Fire Attack");
                    p.audioSource.clip = p.meteorSound;
                    p.audioSource.Play(); 
                    BossRoomManager.Instance.princeAttacks.FireRain();
                    fireCount = 0f;
                }
                else
                {
                    fireCount -= 3;
                }
            }
            else if(sinceLastAttack >= 10f)
            {
                PickAttack(p);
                sinceLastAttack = 0f;
            }
            else
            {
                sinceLastAttack += Time.deltaTime;
                fireCount += Time.deltaTime;
            }
        }

    }

    public void PickAttack(PrinceTemp p)
    {
        int attack = RNG.GenRand(1, 5);
        if (attack <= 4)
        {
            p.princeAnim.Play("Pumpkin Prince Lantern Attack");
            p.audioSource.clip = p.laserSound;
            p.audioSource.Play(); 
            BossRoomManager.Instance.princeAttacks.HorizontalLasers();
        }
        else
        {
            p.princeAnim.Play("Pumpkin Prince Vine Attack");
            p.audioSource.clip = p.vineSound;
            p.audioSource.Play(); 
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

    public AudioSource audioSource;
    public AudioClip vineSound;
    public AudioClip meteorSound;
    public AudioClip meteorLandSound;
    public AudioClip laserSound;

    
    [Header("Flash Hit")]
    public Material flash;
    private Material material;
    public float flashDuration;
    private Coroutine flashRoutine;


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

        //grabs our material for flash effect
        material = gameObject.GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
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
            TakeDamage(other.gameObject.GetComponent<PlayerBulletBehaviour>().bDamage);

        }
    }

    public void TakeDamage(float damage){

        health = health - damage;
        //if the damage is too much the enemy dies
        if (health <= 0 && !dead)
        {
            Die();
        }
        else if(!dead){
            StartCoroutine(FlashRoutine());
        }

    }

    IEnumerator FlashRoutine(){

        gameObject.GetComponent<SpriteRenderer>().material = flash;    
        yield return new WaitForSeconds(flashDuration);
        gameObject.GetComponent<SpriteRenderer>().material = material;

        flashRoutine = null;

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
        yield return new WaitForSeconds(3);
        GameObject.FindGameObjectWithTag("DDOL").GetComponent<DontDestroy>().win = true;
        SceneManager.LoadScene("EndGame");
    }

    void Assign(float value, int index)
    {
        float temp = value;
        stats[index] = temp;
    }

}
