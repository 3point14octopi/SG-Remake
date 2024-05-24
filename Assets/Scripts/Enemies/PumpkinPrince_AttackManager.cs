using JAFprocedural;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Prince_Attack
{
    NONE,
    FIRE,
    VINE,
    LANTERN
}

public class PrinceAttack_Motion
{
    public bool isMoving = false;
    public Vector3 destination = new Vector3(0, 0, 0);
}


public class PumpkinPrince_AttackManager : MonoBehaviour
{
    public Space2D room;
    Prince_Attack currentAttack = Prince_Attack.NONE;
    private bool fireballAttacking = false;


    [Header("Fire Rain Attack")]
    bool fireAttacking = false;
    public GameObject firePrefab;
    public GameObject fireShadowPrefab;
    [SerializeField] public List<GameObject> fireballs = new List<GameObject>();
    [SerializeField] public List<GameObject> fireballShadow = new List<GameObject>();
    private List<PrinceAttack_Motion> fireballMoving = new List<PrinceAttack_Motion>();

    [Header("Vine Wave Attack")]
    public GameObject vinePrefab;
    [SerializeField] public List<GameObject> vineWaves = new List<GameObject>();
    private Stack<int> vineIndexes = new Stack<int>();
    Coord[,] offshoots = new Coord[2, 2]
        { { new Coord(1, 0), new Coord(-1, 0) },
        { new Coord(0, -1), new Coord(0, 1) } };

    [Header("Lantern Blast Attack")]
    public GameObject leftLantern;
    public GameObject rightLantern;
    private PrinceAttack_Motion leftMotion;
    private PrinceAttack_Motion rightMotion;
    private bool windup = false;

    [Header("Screen Saver Attack")]
    public GameObject ss1;
    public GameObject ss2;


    public void OnStart()
    {
        for (int i = 0; i < 20; i++)
        {
            fireballShadow.Add(GameObject.Instantiate(fireShadowPrefab, transform, true));
            fireballShadow[i].SetActive(false);
            fireballs.Add(GameObject.Instantiate(firePrefab, transform, true));
            fireballs[i].SetActive(false);
            fireballMoving.Add(new PrinceAttack_Motion());
        }

        for (int i = 0; i < 40; i++)
        {
            vineWaves.Add(GameObject.Instantiate(vinePrefab, transform, true));
            vineWaves[i].SetActive(false);
        }

        leftLantern = GameObject.Instantiate(leftLantern, transform, true);
        leftLantern.GetComponent<Animator>().Play("Lantern Right");
        leftLantern.SetActive(false);
        rightLantern = GameObject.Instantiate(rightLantern, transform, true);
        rightLantern.SetActive(false);
        leftMotion = new PrinceAttack_Motion();
        rightMotion = new PrinceAttack_Motion();

        
    }
    public void Update()
    {
        //move fireballs
        if (fireballAttacking)
        {
            MoveFireballs();
        }
        if (windup)
        {
            //shift the lanterns up and down the wall
           WindingLanterns();
        }
    }

    public void LoadRoom(Space2D r)
    {
        room = r;
        //tell lanterns they can slay now
        ss1 = GameObject.Instantiate(ss1, transform, true);
        ss1.GetComponent<TempLaternB>().roomCenter = new Vector2(room.worldOrigin.x + 12.5f, -room.worldOrigin.y - 6.5f);
        ss1.SetActive(false);
        ss2 = GameObject.Instantiate(ss2, transform, true);
        ss2.GetComponent<TempLaternB>().roomCenter = new Vector2(room.worldOrigin.x + 12.5f, -room.worldOrigin.y -6.5f);
        ss2.SetActive(false);
    }


    public bool IsFireballing() { return (fireballAttacking == true); }
    public void FireRain()
    {
        if (fireballAttacking)
        {
            return;
        }
        else
        {
            fireballAttacking = true;
            StartCoroutine(FireAttack());
        }

    }
    public void VineWaves()
    {
        StartCoroutine(RootSpawn());
    }
    public void HorizontalLasers()
    {
        StartCoroutine(StartLanternWindup());
    }



    //Fireball functions
    IEnumerator FireAttack()
    {
        StartCoroutine(FireDespawn());
        Debug.Log("starting");
        for (int i = 0; i < fireballs.Count; i++)
        {
            fireballs[i].SetActive(true);
            fireballs[i].GetComponent<FireballObj>().ToggleFalling(true);
            Coord targ = RNG.GenRandCoord(room);
            Vector3 pos = new Vector3(targ.x + room.worldOrigin.x + 0.5f, -targ.y - room.worldOrigin.y + 0.5f, -2);
            fireballShadow[i].SetActive(true);
            fireballShadow[i].transform.position = new Vector3(pos.x, pos.y, 0.5f);
            fireballs[i].transform.position = new Vector3(pos.x, pos.y + 16, -2);

            fireballMoving[i].destination = pos;
            fireballMoving[i].isMoving = true;

            yield return new WaitForSeconds(0.3f);
        }
    }
    private void MoveFireballs()
    {
        for (int i = 0; i < fireballs.Count; i++)
        {
            if (fireballMoving[i].isMoving)
            {
                fireballs[i].transform.position = Vector3.MoveTowards(fireballs[i].transform.position, fireballMoving[i].destination, 20 * Time.deltaTime);
                if (fireballs[i].transform.position.y <= fireballMoving[i].destination.y)
                {
                    Debug.Log("doin a fall thing");
                    fireballs[i].transform.position = new Vector3(fireballMoving[i].destination.x, fireballMoving[i].destination.y, 0.4f);
                    fireballMoving[i].isMoving = false;
                    Debug.Log("calling fall function");
                    fireballs[i].GetComponent<FireballObj>().ToggleFalling(false);
                }
            }
        }
    }
    IEnumerator FireDespawn()
    {
        yield return new WaitForSeconds(6);

        for (int i = 0; i < fireballs.Count; i++)
        {
            fireballs[i].SetActive(false);
            fireballShadow[i].SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
        fireballAttacking = false;
    }

    //Vine Wave functions
    IEnumerator RootSpawn()
    {
        Coord start = new Coord(12, 7);
        Coord[] directions = new Coord[] { new Coord(0, -1), new Coord(1, 0), new Coord(0, 1), new Coord(-1, 0) };
        int[] subtract = new int[4] { 1, 1, 1, 1 };
        Coord[] add = new Coord[4] { new Coord(), new Coord(), new Coord(), new Coord() };
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Debug.Log("root at" + ((i * 4) + j).ToString());
                Debug.Log("root size is" + vineWaves.Count.ToString());
                vineWaves[((i * 4) + j)].SetActive(true);
                if (i > 1 && RNG.GenRand(1, 10) > 8)
                {
                    int alt = RNG.GenRand(0, 2);
                    add[j] = new Coord(add[j].x + offshoots[j % 2, alt].x, add[j].y + offshoots[j % 2, alt].y);
                    subtract[j]--;
                }
                Coord vLoc = new Coord((start.x + (directions[j].x * (i + subtract[j])) + add[j].x), (start.y + (directions[j].y * (i + subtract[j])) + add[j].y));
                //directions[j] = new Coord(directions[j].x + add.x, directions[j].y + add.y);
                vineWaves[(i * 4) + j].transform.position = new Vector3(vLoc.x + room.worldOrigin.x + 0.5f, -vLoc.y - room.worldOrigin.y + 0.5f, 0.5f);

                vineIndexes.Push((i * 4) + j);
            }
            yield return new WaitForSeconds(0.15f);
        }

        StartCoroutine(RootDespawn());
    }
    IEnumerator RootDespawn()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vineWaves[vineIndexes.Pop()].GetComponent<SpikyVinesBehaviour>().Wither();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }


    //Lantern Beam functions
    IEnumerator StartLanternWindup()
    {
        ss1.SetActive(false);
        leftLantern.SetActive(true);
        leftLantern.transform.position = new Vector3(room.worldOrigin.x + 1.5f, -room.worldOrigin.y - RNG.GenRand(1, room.height - 2) + 0.5f, -1);
        leftMotion.destination = new Vector3(room.worldOrigin.x + 1.5f, -room.worldOrigin.y - RNG.GenRand(1, room.height - 2) + 0.5f, -1);

        ss2.SetActive(false);
        rightLantern.SetActive(true);
        rightLantern.transform.position = new Vector3(room.worldOrigin.x + 23.5f, -room.worldOrigin.y - RNG.GenRand(1, room.height - 2) + 0.5f, -1);
        rightMotion.destination = new Vector3(room.worldOrigin.x + 23.5f, -room.worldOrigin.y - RNG.GenRand(1, room.height - 2) + 0.5f, -1);

        windup = true;

        StartCoroutine(LanternBlast());

        yield return null;
    }
    private void WindingLanterns()
    {
        leftLantern.transform.position = Vector3.MoveTowards(leftLantern.transform.position, leftMotion.destination, 5 * Time.deltaTime);
        if(Mathf.Abs(leftLantern.transform.position.y - leftMotion.destination.y) <= 0.1)
        {
            leftMotion.destination = new Vector3(room.worldOrigin.x + 1.5f, -room.worldOrigin.y - RNG.GenRand(1, room.height - 2) + 0.5f, -1);
        }

        rightLantern.transform.position = Vector3.MoveTowards(rightLantern.transform.position, rightMotion.destination, 5 * Time.deltaTime);
        if (Mathf.Abs(rightLantern.transform.position.y - rightMotion.destination.y) <= 0.1)
        {
            rightMotion.destination = new Vector3(room.worldOrigin.x + 23.5f, -room.worldOrigin.y - RNG.GenRand(1, room.height - 2) + 0.5f, -1);
        }
    }
    IEnumerator LanternBlast()
    {
        yield return new WaitForSeconds(5);

        windup = false;
        leftLantern.GetComponent<Animator>().Play("Lantern Blast Right");
        rightLantern.GetComponent<Animator>().Play("Lantern Blast Left");
        yield return new WaitForSeconds(1f);

        for(int i = 2; i < 24; i++)
        {
            leftLantern.GetComponent<BoxCollider2D>().offset = new Vector2((float)((i - 1) / 2), 0);
            leftLantern.GetComponent<BoxCollider2D>().size = new Vector2(i, 1);


            rightLantern.GetComponent<BoxCollider2D>().offset = new Vector2((float)((i - 1) / -2), 0);
            rightLantern.GetComponent<BoxCollider2D>().size = new Vector2(i, 1);

            yield return null;
        }

        yield return new WaitForSeconds(2);

        leftLantern.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
        leftLantern.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
        leftLantern.GetComponent<Animator>().Play("Lantern Right");

        rightLantern.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
        rightLantern.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
        rightLantern.GetComponent<Animator>().Play("Lantern Left");

        yield return null;
        leftLantern.SetActive(false);
        rightLantern.SetActive(false);
        ss1.SetActive(true);
        ss2.SetActive(true);
    }

    //Screensaver Activation
    public void ActivateBouncer()
    {
        ss1.SetActive(true);
        Coord loc = RNG.GenRandCoord(room);
        ss1.transform.position = new Vector3(loc.x + room.worldOrigin.x + 0.5f, -loc.y - room.worldOrigin.y + 0.5f, 0);
        
        ss2.SetActive(true);
        loc = RNG.GenRandCoord(room);
        ss2.transform.position = new Vector3(loc.x + room.worldOrigin.x + 0.5f, -loc.y - room.worldOrigin.y + 0.5f, 0);
    }
}
