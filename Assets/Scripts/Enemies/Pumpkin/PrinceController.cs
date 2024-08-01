using EntityStats;
using UnityEngine;
using System.Collections;
using JAFprocedural;
using System.Collections.Generic;

public class PrinceController : MonoBehaviour
{
    public AudioClip badjoke;
    public GameObject vinePrefab;
    public GameObject bulbPrefab;
    public Transform tracker;
    public int chaseLength = 15;
    public int chaseStartIndex = 20;

    public GameObject lanternPrefab;
    public float lanternSpeed = 1f;
    LanternController[] lanterns = new LanternController[2];

    Animator anims;
    [HideInInspector]public Brain princeBrain;
    GunModule shootyMcShootface;
    [HideInInspector] public PrincePhase attackPhase;
    bool phase1 = true;
    bool phase2 = false;
    bool phase3 = false;

    int chaseDir = -1;
    GameObject[] vines = new GameObject[40];
    GameObject[] bulbs = new GameObject[4];

    private void OnEnable()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        anims = GetComponent<Animator>();
        princeBrain = GetComponent<Brain>();
        shootyMcShootface = GetComponent<GunModule>();

        attackPhase = new FirePhase();
        attackPhase.OnPhaseStart(this);

        for (int i = 0; i < vines.Length; vines[i] = Instantiate(vinePrefab, transform.parent, true), vines[i].SetActive(false), i++) ;
        for (int i = 0; i < bulbs.Length; bulbs[i] = Instantiate(bulbPrefab, transform, true), bulbs[i].GetComponent<Brain>().mom = princeBrain.mom, bulbs[i].SetActive(false), i++) ;
    }

    // Update is called once per frame
    void Update()
    {
        attackPhase.OnPhaseUpdate();
    }

    /// <summary>
    /// EDIT THIS to be a state machine
    /// </summary>
    public void CheckHealthState()
    {
        attackPhase.CheckPhaseStatus();
    }

    //fire phase
    public IEnumerator FunnyFireAttack(bool startpause = false)
    {
        if (startpause)
        {
            GetComponent<AudioSource>().PlayOneShot(badjoke);
            yield return new WaitForSeconds(2.5f);
        }
        
        while(true)
        {
            anims.Play("Pumpkin Prince Fire Attack");
            int index = (RNG.GenRand(0, 5) > 3) ? 1 : 0;
            yield return shootyMcShootface.PresetShoot(shootyMcShootface.ammoList[index]);
            yield return new WaitForSeconds(5);
        }
    }

    public void SingleShot(int bulletIndex)
    {
        anims.Play("Pumpkin Prince Fire Attack");
        StartCoroutine(shootyMcShootface.PresetShoot(shootyMcShootface.ammoList[bulletIndex]));
    }



    //vine phase
    public IEnumerator VineAttack()
    {
        Queue<Vector2> path = AstarDebugLayer.Instance.AstarPath(transform.position, tracker.position);
        int pleg = (path != null) ? path.Count : 0;
        Queue<Vector2> p2 = new Queue<Vector2>();

        Vector2[] dirs = new Vector2[4] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };
        Vector2 sPos = new Vector2();
        Vector2 fDir;

        //will not run if a path could not be found to player
        for (int i = 0; i < pleg; i++)
        {
            //save start (not sure if currently necessary)
            if (i == 0)
            {
                sPos = path.Dequeue();
            }
            else if (i == 1)
            {
                //get second value (so we can calculate the direction)
                fDir = path.Peek();
                //calc direction
                Vector2 sub = fDir - sPos;
                //checking our array for result. should be one of the four, otherwise the path is somehow invalid. 
                //if we find it, we assign the index to pdir and break out of the loop early
                for (int j = 0; j < 4 && chaseDir == -1; chaseDir = (sub == dirs[j]) ? j + 0 : -1, j++) ;
            }

            if (i > 0)
            {
                p2.Enqueue(path.Dequeue());
            }

        }
        anims.Play("Pumpkin Prince Vine Attack");
        //spawn the roots in the other directions
        yield return VineBlock();
        //when RootSpawn has ended (but not RootDespawn, because that's a different process) this function will continue
        if (p2.Count > 0)
        {
            yield return VinePoke(p2);
        }
        else
        {
            anims.Play("Pumpkin Prince Idle");
        }
    }


    //a few changes from the old version (more boring now)
    //goal is to make walls rather than goofy paths
    IEnumerator VineBlock()
    {
        Coord start = new Coord(7, 4);
        //up, right, down, left
        Coord[] directions = new Coord[] { new Coord(0, -1), new Coord(1, 0), new Coord(0, 1), new Coord(-1, 0) };

        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                if(j != chaseDir)
                {
                    if(i < 3 || j % 2 == 1)
                    {
                        vines[(i * 4) + j].SetActive(true);
                        Coord vLoc = new Coord((start.x + (directions[j].x * (i + 1))), (start.y + (directions[j].y * (i + 1))));
                        vines[(i * 4) + j].transform.position = new Vector3(vLoc.x + princeBrain.mom.roomLayout.worldOrigin.x + 0.5f, -vLoc.y - princeBrain.mom.roomLayout.worldOrigin.y + 0.5f, 0.5f);
                    }
                    
                }
            }
            yield return new WaitForSeconds(0.1f);
        }

        if(chaseDir != 1)
        {
            vines[16].SetActive(true);
            Coord vLoc = new Coord((start.x + directions[1].x * 6), start.y + (directions[1].y * 6));
            vines[16].transform.position = new Vector3(vLoc.x + princeBrain.mom.roomLayout.worldOrigin.x + 0.5f, -vLoc.y - princeBrain.mom.roomLayout.worldOrigin.y + 0.5f, 0.5f);
            
        }

        if(chaseDir != 3)
        {
            vines[18].SetActive(true);
            Coord vLoc = new Coord((start.x + directions[3].x * 6), start.y + (directions[3].y * 6));
            vines[18].transform.position = new Vector3(vLoc.x + princeBrain.mom.roomLayout.worldOrigin.x + 0.5f, -vLoc.y - princeBrain.mom.roomLayout.worldOrigin.y + 0.5f, 0.5f);
        }
        
        for(int i = 0; i < 4; i++)
        {
            if(i != chaseDir)
            {
                bulbs[i].SetActive(true);
                bulbs[i].GetComponent<BulbController>().vineDirection = i + 0;
                int multiplier = (i % 2 == 1) ? 6 : 3;
                Coord bLoc = new Coord((start.x + (directions[i].x * multiplier)), (start.y + (directions[i].y * multiplier)));
                bulbs[i].transform.position = new Vector3(bLoc.x + princeBrain.mom.roomLayout.worldOrigin.x + 0.5f, -bLoc.y - princeBrain.mom.roomLayout.worldOrigin.y + 0.5f, 0f);
            }
        }

        yield return null;
    }

    IEnumerator VinePoke(Queue<Vector2> vineLocs)
    {
        Vector2 last = new Vector2();
        for(int i = 0; i < chaseLength && vineLocs.Count != 0; i++)
        {
            vines[chaseStartIndex + i].SetActive(true);
            Vector3 cPos = new Vector3(vineLocs.Peek().x, vineLocs.Peek().y, 0.5f);
            vines[chaseStartIndex + i].transform.position = cPos;

            Queue<Vector2> newPath = AstarDebugLayer.Instance.AstarPath(cPos, tracker.position);
            if(newPath != null && newPath.Count > 0)
            {
                if(vineLocs.Peek() == newPath.Peek())
                {
                    vineLocs = newPath;
                }
            }

            last = vineLocs.Dequeue();
            yield return new WaitForSeconds(0.25f);
        }

        if(last != new Vector2())
        {
            bulbs[chaseDir].SetActive(true);
            bulbs[chaseDir].GetComponent<BulbController>().vineDirection = chaseDir + 0;
            bulbs[chaseDir].transform.position = new Vector3(last.x, last.y, 0);
        }

        anims.Play("Pumpkin Prince Idle");
    }


    public void RetractVineTrail(int index)
    {
        //default value that means the bulb wasn't actually attached to anything
        if (index == -1) return;

        //retracting the chasing vine
        if (index == chaseDir) StartCoroutine(RetractChaseRow());
        //retracting a wall
        else StartCoroutine(RetractRow(index));

    }


    IEnumerator RetractRow(int index)
    {
        if(index % 2 == 1)
        {
            vines[15 + index].GetComponent<SpikyVinesBehaviour>().Wither();
            yield return new WaitForSeconds(0.2f);
        }

        int start = (index % 2 == 1) ? 4 : 2;

        for(int i = start; i >= 0; i--)
        {
            if (vines[i*4 + index].activeInHierarchy)
            {
                vines[i * 4 + index].GetComponent<SpikyVinesBehaviour>().Wither();
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    IEnumerator RetractChaseRow()
    {
        for(int i = chaseStartIndex + chaseLength - 1; i >= chaseStartIndex; i--)
        {
            //catch because it can exit early
            if (vines[i].activeInHierarchy)
            {
                vines[i].GetComponent<SpikyVinesBehaviour>().Wither();
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public void ToggleCollider(bool on)
    {
        if (on)
        {
            if (princeBrain.damageTags.Contains("R_PlayerBullet")) princeBrain.damageTags.Remove("R_PlayerBullet");
            if (!princeBrain.damageTags.Contains("PlayerBullet")) princeBrain.damageTags.Add("PlayerBullet");
        }
        else
        {
            if (princeBrain.damageTags.Contains("PlayerBullet")) princeBrain.damageTags.Remove("PlayerBullet");
            if (!princeBrain.damageTags.Contains("R_PlayerBullet")) princeBrain.damageTags.Add("R_PlayerBullet");
        }
    }


    //lantern phase
    public void InstantiateLanterns()
    {
        for (int i = 0; i < 2; i++)
        {
            lanterns[i] = Instantiate(lanternPrefab, transform, false).GetComponent<LanternController>();
            lanterns[i].transform.Rotate(0, ((i % 2) * 180f), 0);
        }
    }

    public void SendToStartPosition()
    {
        for (int i = 0; i < lanterns.Length; lanterns[i].StartCoroutine(lanterns[i].GoToStart(6, lanternSpeed)), i++) ;
    }

    public void Blast()
    {
        for (int i = 0; i < lanterns.Length; lanterns[i].StartCoroutine(lanterns[i].Blast()), i++) ;
    }
}
