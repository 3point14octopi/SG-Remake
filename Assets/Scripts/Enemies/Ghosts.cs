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


    public AStarCalculator aStar;
    public Space2D roomMap;
    public bool isPathfinding = false;
    public Queue<Coord> ghostPath = new Queue<Coord>();
    bool firstUpdate = true;

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
            firstUpdate = true;
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

        aStar = new AStarCalculator(new Space2D(), 1);
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

    private Coord WorldLocToCoord(Vector3 loc)
    {
        return new Coord(Mathf.FloorToInt(loc.x - roomMap.worldOrigin.x - 0.5f), Mathf.FloorToInt(-loc.y - roomMap.worldOrigin.y - 0.5f)+1);
    }

    private void UpdateMovementAnim()
    {
        //Determines what walking direction animation to use
        anim.SetFloat("playerX", player.transform.position.x - transform.position.x);
        anim.SetFloat("playerY", player.transform.position.y - transform.position.y);

        if (Mathf.Abs(anim.GetFloat("playerX")) > Mathf.Abs(anim.GetFloat("playerY"))) { anim.SetBool("Vertical", false); }
        else { anim.SetBool("Vertical", true); }
    }

    private void CopyToQueue(List<Coord> path)
    {
        int i;
        for (i = 0, isPathfinding = true, ghostPath.Clear(); i < path.Count; ghostPath.Enqueue(path[i]), i++) ;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (firstUpdate)
            {
                aStar = new AStarCalculator(roomMap, 1);
                firstUpdate = false;
            }
            if (!isPathfinding)
            {
                Scan();
                if (LOS)
                {
                    //is this LOS?
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    //update the animations
                    UpdateMovementAnim();
                }
                else
                {
                    //we can't see the player, so grab a pathfinding list
                    //GmapDisplay.Instance.UpdateMap(roomMap);
                    List<Coord> toPlayer = aStar.AStar(WorldLocToCoord(transform.position), WorldLocToCoord(player.transform.position), 2000);
                    if (toPlayer != null) CopyToQueue(toPlayer);
                }
            }

            
            //follow our path
            if (isPathfinding)
            {
                //travel towards top value in the queue
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    new Vector2(ghostPath.Peek().x + roomMap.worldOrigin.x + 0.5f, -ghostPath.Peek().y - roomMap.worldOrigin.y + 0.5f),
                    speed * Time.deltaTime);

                if (new Vector2(transform.position.x, transform.position.y) == new Vector2(ghostPath.Peek().x + roomMap.worldOrigin.x + 0.5f, -ghostPath.Peek().y - roomMap.worldOrigin.y + 0.5f))
                {
                    //remove position from queue
                    ghostPath.Dequeue();
                    //check if within sight
                    Scan();
                    //stop current pathfinding if we see player or have arrived somewhere and still cant see player
                    if (LOS || ghostPath.Count == 0) isPathfinding = false;
                    else
                    {
                        List<Coord> toPlayer = aStar.AStar(WorldLocToCoord(transform.position), WorldLocToCoord(player.transform.position), 2000);
                        if (toPlayer != null && toPlayer.Count < ghostPath.Count) CopyToQueue(toPlayer);
                    }

                }
                //update the animations
                UpdateMovementAnim();
            }


            //ghost saying oooo
            if(audioSource.isPlaying == false){
            if(RNG.GenRand(1, 2) == 1){audioSource.clip = ooh1Sound;}
            else{audioSource.clip = ooh2Sound;}
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
            //if health is 0 destorys the object
            if(health <= 0 && !dead){
                StartCoroutine(Death());
            }
        }

        //damages the player if we wall into the player
        else if (other.gameObject.tag == "Player")
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