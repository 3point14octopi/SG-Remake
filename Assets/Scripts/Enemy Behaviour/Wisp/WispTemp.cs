using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAFprocedural;
using Unity.VisualScripting;

public class WispTemp : MonoBehaviour
{

    public int wispIndex = -1;
    public Vector3 addPosition = Vector3.zero;

    [Header("Animators & Sounds")]
    private Animator anim; //our animator
    private bool dead = false; //a bool to know to stop shooting while we are doing our dead animation

    // private AudioSource audioSource;
    // public AudioClip shoot1Sound;
    // public AudioClip shoot2Sound;
    // public AudioClip deathSound;

    [Header("Wisp Stats")]
    public float health = 10;
    public float speed = 0.75f;
    public float damage = 10;

    private float[] stats = new float[3];
    private bool instantiated = false;

    public LayerMask cLayermask;

    [Header("Flash Hit")]
    public Material flash;
    private Material material;
    public float flashDuration;
    private Coroutine flashRoutine;

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

        anim = gameObject.GetComponent<Animator>();

        
        //grabs our material for flash effect
        material = gameObject.GetComponent<SpriteRenderer>().material;
        gameObject.GetComponent<SpriteRenderer>().material = material;

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
            Flash();

            //if the damage is too much the enemy dies
            if (health <= 0 && !dead)
            {
                StartCoroutine(Death());
            }
        }

        //damages the player if we wall into the player
        else if (other.gameObject.tag == "Player" && !dead)
        {
            other.gameObject.GetComponent<FbStateManager>().TakeDamage(damage);
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
        anim.Play("WispDeath");
        RoomPop.Instance.EnemyKilled();
        WispManager.Instance.RemoveWisp(wispIndex);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<SpriteRenderer>().material = material;
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


    IEnumerator FlashRoutine(){

        gameObject.GetComponent<SpriteRenderer>().material = flash;
        
        yield return new WaitForSeconds(flashDuration);

        gameObject.GetComponent<SpriteRenderer>().material = material;

        flashRoutine = null;

    }

    void Flash(){
        if(flashRoutine != null){
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }
}
