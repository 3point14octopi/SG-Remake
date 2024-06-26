using JAFprocedural;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WispManager : MonoBehaviour
{
    //public float speed;
    //public Vector2 maxSpeed;
    //public float avoidance = 0.75f;
    //public float obstacleAvoid = 1.5f;
    //public float targetRange = 1f;
    //public bool beFunny = false;

    //public Vector3 targetLocation = new Vector3();
    //public Vector3 startingCentre;
    ////public Transform targTransform;
    //public List<GameObject> wisps = new List<GameObject>();
    //public Space2D roomMap = new Space2D(0,0);
    //public bool roomOn = false;


    //public GameObject wanderer;
    //public Vector3 wTarg;
    ////public float wRot = 0f;
    //public bool rotating = false;
    //private Vector3 wVel = Vector3.zero;
    //public GameObject badCollider;
    //public List<GameObject> cList;
    //public LayerMask cLayermask;
    
    //private Vector3[] velocities = new Vector3[8];
    //private Vector3 startingVelocity;
    //int boidCount = 0;

    //private Space2D surroundingTerrain;

    //public static WispManager Instance;

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(this);
    //    }
    //    else
    //    {
    //        Instance = this;
    //    }
    //}
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    maxSpeed = new Vector2(speed, speed);
    //    if(roomOn && boidCount > 0)
    //    {
    //        CheckWander();
            
    //        //other wisps follow the leader
    //        for(int boid = boidCount-1; boid > 0; boid--)
    //        {
    //            //rotate towards wisp ahead 
    //            wisps[boid].transform.rotation = Quaternion.Euler(0, 0, angle(wisps[boid].transform.position, wisps[boid-1].transform.position));
    //            CheckForCollisions(boid);
    //            wisps[boid].transform.position = /*(beFunny)?wisps[boid-1].transform.position:*/Vector2.MoveTowards(wisps[boid].transform.position, wisps[boid - 1].transform.position, speed * Time.deltaTime);
    //        }

    //        //front wisp wanders
    //        wisps[0].GetComponent<Rigidbody2D>().AddForce(wisps[0].transform.up.normalized + CheckForCollisions(0) * Time.deltaTime *speed, ForceMode2D.Impulse);
    //        if (wisps[0].GetComponent<Rigidbody2D>().velocity.magnitude > speed)
    //        {
    //            wisps[0].GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(wisps[0].GetComponent<Rigidbody2D>().velocity, speed);
    //        }
    //    }
        
    //}

    ////wanderer functions
    //private void CheckWander()
    //{
    //    if(RNG.GenRand(1, 10) > 9)
    //    {
    //        float wRot = (RNG.GenRand(-1, 3) + RNG.GenRand(-1, 3))%2;
    //        wisps[0].transform.Rotate(0, 0, wRot * 5);
    //    }
        
    //}


    //private float angle(Vector3 boid, Vector3 target)
    //{
    //    return (Mathf.Atan2(target.y - boid.y, target.x - boid.x) * Mathf.Rad2Deg - 90f);
    //}

    //private void InitCentreOnUpdate()
    //{
    //    startingCentre = Vector3.zero;

    //    for(int boid = 0; boid < boidCount; startingCentre += wisps[boid].transform.position, boid++);
    //}
    //private void InitVelocityOnUpdate()
    //{
    //    startingVelocity = Vector3.zero;
    //    foreach (Vector3 boid in velocities) startingVelocity += boid;
    //}

    
    ////RULE 1: MOVE TOWARDS CENTRE OF MASS
    //private Vector3 BoidCentre(int boidIndex)
    //{
    //    Vector3 centre = startingCentre - wisps[boidIndex].transform.position;

    //    centre /= boidCount - 1;
    //    centre = ((centre - wisps[boidIndex].transform.position) / 200);

    //    return centre;
    //}


    //private Vector3 Avoid(Vector3 boid, Vector3 obstacle, bool isObstacle = false)
    //{
    //    Vector3 avoidVector = (obstacle - boid);
    //    float avoidVal = (isObstacle) ? obstacleAvoid : avoidance;
    //    if(avoidVector.magnitude < avoidVal)
    //    {
    //        //apply linear separation
    //        float intensity = (speed * 15) * (avoidVal - avoidVector.magnitude) / avoidVal;

    //        return avoidVector * intensity;
    //    }

    //    return Vector3.zero;
    //}
    ////RULE 2: AVOID OTHERS
    //private Vector3 AvoidOthers(int boidIndex)
    //{
    //    Vector3 result = Vector3.zero;

    //    for(int boid = 0; boid < boidCount; boid++)
    //    {
    //        //we don't need to separate from ourself
    //        if(boid != boidIndex)
    //        {
    //            result -= Avoid(wisps[boidIndex].transform.position, wisps[boid].transform.position);
    //        }
    //    }

    //    return result;
    //}
    //private Vector3 CheckForCollisions(int boid)
    //{
    //    bool centreHit = false;
    //    bool leftHit = false;
    //    bool rightHit = false;

    //    Vector3 avoidance = Vector3.zero;
    //    RaycastHit2D wallDetect = Physics2D.Raycast(wisps[boid].transform.position + wisps[boid].transform.up, wisps[boid].transform.up, 2f, cLayermask);
    //    if (wallDetect.collider != null)
    //    {
    //        Debug.Log("hit");
    //        Debug.Log("i just hit a " + wallDetect.collider.gameObject.tag.ToString());
    //        if (wallDetect.collider.gameObject.tag == "Barrier")
    //        {
    //            Debug.Log("wow, a thing");
    //            avoidance -= Avoid(wisps[boid].transform.position, wallDetect.collider.transform.position, true);
    //        }
    //    }

    //    //whiskers here
    //    if (avoidance != Vector3.zero)
    //    {
    //        centreHit = true;
    //        wisps[boid].transform.Rotate(0, 0, 45);
    //    }
    //    return avoidance;
    //}


    ////RULE 3: MATCH VELOCITY OF OTHERS
    //private Vector3 MatchVelocity(int boidIndex)
    //{
    //    Vector3 vel = startingVelocity - velocities[boidIndex];
        
    //    vel /= boidCount - 1;
    //    vel = ((vel - velocities[boidIndex]) / 10);

    //    return vel;
    //}


    ////travel towards a target
    //private Vector3 Target(int boidIndex)
    //{
    //    float dist = Vector3.Distance(wisps[boidIndex].transform.position, wanderer.transform.position);
    //    Vector3 val = Vector3.MoveTowards(wisps[boidIndex].transform.position, wanderer.transform.position, dist) - wisps[boidIndex].transform.position;
    //    return val * 0.6f;
    //}


    ////public functions
    //public void AssignMap(Space2D room)
    //{
    //    roomMap = room;
    //    roomOn = true;


    //    //PickTarget();
    //    //wanderer.transform.position = targetLocation;
    //    if(cList.Count == 0)StartCoroutine(PlaceBadColliders());

    //    velocities = new Vector3[boidCount];
    //}

    //IEnumerator PlaceBadColliders()
    //{
    //    for(int i = 0; i < roomMap.height; i++)
    //    {
    //        for(int j = 0; j < roomMap.width; j++)
    //        {
    //            if(roomMap.GetCell(j, i) != 1)
    //            {
    //                cList.Add(GameObject.Instantiate(badCollider, transform, true));
    //                cList[cList.Count - 1].transform.position = new Vector3(j + roomMap.worldOrigin.x + 0.5f, -i - roomMap.worldOrigin.y + 0.5f, 0.5f);
    //            }
    //        }
    //        yield return null;
    //    }
    //}

    //public void TurnOff()
    //{
    //    roomOn = false;
    //    cList.Clear();
    //}

    //public void RemoveWisp(int index)
    //{
    //    if(index > -1 && index < boidCount)
    //    {
    //        wisps.RemoveAt(index);
    //        boidCount--;
    //        for (int i = index; i < boidCount; wisps[i].GetComponent<WispTemp>().wispIndex = i, i++);
    //    }
    //}

    //public int AddWisp(GameObject boid)
    //{
    //    Debug.Log("added");
    //    wisps.Add(boid);
    //    boidCount++;
    //    return boidCount - 1;
    //}
}
