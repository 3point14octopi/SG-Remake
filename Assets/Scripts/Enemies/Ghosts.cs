using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAFprocedural;

public class Ghosts : MonoBehaviour
{
    
    private GameObject player; // player so we can shoot at them

    private bool LOS; //Used to track if we can see the player
    private LayerMask playerMask; //layer reference for player
    private LayerMask barrierMask; //layer reference for trees

    [Header("Animators & Sounds")]
    private Animator anim;
    private bool dead = false;

    private AudioSource audioSource;
    public AudioClip ooh1Sound;
    public AudioClip ooh2Sound;
    public AudioClip deathSound;

    [Header("Ghost Stats")]
    public float health = 50; //enemy health
    public float speed = 1; //walk speed
    public float damage = 10; //damage it deals to player 


    private float[] stats = new float[3];
    private bool instantiated = false;

    [Header("Flash Hit")]
    public Material flash;
    private Material material;
    public float flashDuration;
    private Coroutine flashRoutine;


    public bool isPathfinding = false;
    public Queue<Vector2> ghostPath = new Queue<Vector2>();
    float cooldown = 0f;

    //this lets us reset the ghost by re-enabling the game object without having to hardcode our stats
    void OnEnable()
    {
        if (instantiated)
        {
            health = stats[0];
            speed = stats[1];
            damage = stats[2];

            dead = false;
            anim.SetBool("Death", false);
            //pathDisplay = GameObject.Find("A_ Debug").GetComponent<AstarDebugLayer>();
           
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //so we can have the players transform
        player = GameObject.FindWithTag("Player");

        //find our layers used for our raycast masks
        playerMask |= 0x1 << 6;
        barrierMask |= 0x1 << 7;

        //find our animation
        anim = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();

        

        //spooky stuff
        instantiated = true;
        Assign(health, 0);
        Assign(speed, 1);
        Assign(damage, 2);

        //grabs our material for flash effect
        material = gameObject.GetComponent<SpriteRenderer>().material;
        gameObject.GetComponent<SpriteRenderer>().material = material;
    }

    public void Scan()
    {
        //finds the player with a raycast and then raycasts up until the player looking for a tree
        RaycastHit2D playerRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 20, playerMask);         
        RaycastHit2D barrierRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, playerRay.distance, barrierMask);

        //If it can see the player but not see a tree it sets line of sight to true
        if(playerRay.collider != null){
            if(playerRay.collider.CompareTag("Player") && barrierRay.collider == null){
                LOS = true;
            }
            else{
                LOS = false;
            }
        }
    }


    private void UpdateMovementAnim()
    {
        //Determines what walking direction animation to use
        anim.SetFloat("playerX", player.transform.position.x - transform.position.x);
        anim.SetFloat("playerY", player.transform.position.y - transform.position.y);

        if (Mathf.Abs(anim.GetFloat("playerX")) > Mathf.Abs(anim.GetFloat("playerY"))) { anim.SetBool("Vertical", false); }
        else { anim.SetBool("Vertical", true); }
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (!dead)
        {
            
            Scan();
            if (LOS)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                if (ghostPath != null) ghostPath.Clear();
                isPathfinding = false;
                cooldown = 0f;
                UpdateMovementAnim();
            }
            else
            {
                if (!isPathfinding && cooldown <= 0f)
                {
                    ghostPath = AstarDebugLayer.Instance.AstarPath(transform.position, player.transform.position);
                    isPathfinding = (ghostPath.Count > 0);
                    cooldown = 0.5f;
                }
                else if(cooldown > 0f)
                {
                    cooldown -= Time.deltaTime;
                }
            }

            if (isPathfinding)
            {
                transform.position = Vector2.MoveTowards(transform.position, ghostPath.Peek(), speed * Time.deltaTime);
                
                UpdateMovementAnim();
                if((Vector2)transform.position == ghostPath.Peek())
                {
                    ghostPath.Dequeue();
                    if(ghostPath.Count == 0)
                    {
                        isPathfinding = false;
                    }
                    else
                    {
                        Queue<Vector2> potentialPath = AstarDebugLayer.Instance.AstarPath(transform.position, player.transform.position);
                        if (potentialPath.Count > 0 && potentialPath.Count <= ghostPath.Count) ghostPath = potentialPath;
                    }

                }
            }
            


            //ghost saying oooo
            if(audioSource.isPlaying == false)
            {
                audioSource.clip = (RNG.GenRand(1, 2) == 1) ? ooh1Sound : ooh2Sound;
                audioSource.Play();  
            }
                

        }

    }

    void OnCollisionEnter2D(Collision2D other){
        //checks if it is hit by a bullet from a player
        if (other.gameObject.tag == "PlayerBullet")
        {
            health = health - other.gameObject.GetComponent<PlayerBulletBehaviour>().bDamage;
            Flash();
            //if health is 0 destroys the object
            if(health <= 0 && !dead){
                StartCoroutine(Death());
            }
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
        anim.SetBool("Death", true);
        audioSource.clip = deathSound;
        audioSource.Play();
        RoomPop.Instance.EnemyKilled();
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<SpriteRenderer>().material = material;
        gameObject.SetActive(false);
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