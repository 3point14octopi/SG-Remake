using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityStats;

public class GhostController : MonoBehaviour
{
    //component refs
    Animator anim;
    Brain ghostBrain;

    //LOS tracking
    GameObject player;
    LayerMask playerMask;
    LayerMask barrierMask;
    bool LOS = false;

    //A* variables
    bool isPathfinding = false;
    Queue<Vector2> ghostPath = new Queue<Vector2>();
    bool firstUpdate = true;
    float cooldown = 0f;

    private void OnEnable()
    {
        if (anim != null) anim.SetBool("Death", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        playerMask |= 0x1 << 6;
        barrierMask |= 0x1 << 7;

        anim = GetComponent<Animator>();
        ghostBrain = GetComponent<Brain>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ghostBrain.isAlive)
        {
            Scan();
            if (LOS)
            {
                //walk towards player if visible
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, ghostBrain.currentStats[(int)EntityStat.Speed] * Time.deltaTime);
                if (ghostPath != null) ghostPath.Clear();
                isPathfinding = false;
                cooldown = 0f;
                UpdateMovementAnim();
            }
            else
            {
                //if we lost sight and can pathfind
                if(!isPathfinding && cooldown <= 0f)
                {
                    //pathfind to the player
                    ghostPath = AstarDebugLayer.Instance.AstarPath(transform.position, player.transform.position);
                    //store path if valid list returened
                    isPathfinding = (ghostPath.Count > 0);
                    cooldown = 0.5f;
                }
                else if(cooldown > 0f)
                {
                    //cooldown for pathfinding so that it doesn't try to do 180000 iterations of A* per second
                    cooldown -= Time.deltaTime;
                }
            }

            //if we have a valid pathfinding list
            if (isPathfinding)
            {
                //Move towards next node
                Vector3 myPos = transform.position;
                transform.position = Vector2.MoveTowards(transform.position, ghostPath.Peek(), ghostBrain.currentStats[(int)EntityStat.Speed] * Time.deltaTime);
                UpdateMovementAnim();

                //if we have reached the target node, see if there's a faster path
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
        }
    }

    private void Scan()
    {
        //finds the player with a raycast and then raycasts up until the player looking for a tree
        RaycastHit2D playerRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 20, playerMask);
        RaycastHit2D barrierRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, playerRay.distance, barrierMask);

        //If it can see the player but not see a tree it sets line of sight to true
        if (playerRay.collider != null)
        {
            if (playerRay.collider.CompareTag("Player") && barrierRay.collider == null)
            {
                LOS = true;
            }
            else
            {
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

}
