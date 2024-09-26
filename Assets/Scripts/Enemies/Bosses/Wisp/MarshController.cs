using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAFprocedural;

public class MarshController : MonoBehaviour
{
    public GameObject prefab;
    List<MarshInhabitant> inhabitants;
    Transform marshTransform;

    int activeBodyCount;
    Transform target;
    public float countdown;

    public Vector2 startingCentre;
    public float wispAvoidance = 1f;
    public float wispSpeed = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        marshTransform = transform;
        countdown = 6f;
        target = GameObject.Find("Frostbite").transform;
        
        StartCoroutine(SpawnWisps());
    }

    // Update is called once per frame
    void Update()
    {
        InitCentre();
        for(int boid = 0; boid < inhabitants.Count && inhabitants.Count > 1; boid++)
        {
            if (inhabitants[boid].activeInSwarm)
            {
                //             starting velocity          flock to centre                                avoidance                  stay relatively close to origin                     a bit of variance  
                Vector2 targ = inhabitants[boid].Vel() + (CentreOfMass(inhabitants[boid].Pos()) * 3f) + (AvoidOthers(boid) * 8f) + (GravitateToMarsh(inhabitants[boid].Pos()) * 5f) + (RandomOffshoot()*1.2f);
                inhabitants[boid].body.AddForce(targ.normalized * wispSpeed * Time.deltaTime, ForceMode2D.Impulse);

            
                //clamp speed
                inhabitants[boid].ClampSpeed(wispSpeed);
            }
            
        }

        countdown -= Time.deltaTime;
        if(countdown <= 0f && activeBodyCount > 1)
        {
            SelectHomingInhabitant();
            countdown = RNG.GenRand(3, 5);
        }
    }


    IEnumerator SpawnWisps()
    {
        for(int i = 0; i < 12; i++)
        {
            inhabitants.Add(Instantiate(prefab, marshTransform, false).GetComponent<MarshInhabitant>());
            inhabitants[i].MoveTo(inhabitants[i].Pos() + new Vector2(RNG.GenRand(-30, 60) / 10, RNG.GenRand(-30, 60) / 10));

            yield return new WaitForSecondsRealtime(1f);
        }
    }


    void InitCentre()
    {
        startingCentre = Vector2.zero;
        activeBodyCount = 0;
        Vector2 me = (Vector2)marshTransform.position;

        for (int boid = 0; boid < inhabitants.Count; boid++)
        {
            if (inhabitants[boid].activeInSwarm)
            {
                startingCentre += (inhabitants[boid].Pos() - me);
                activeBodyCount++;
            }
        }
            
    }

    Vector2 CentreOfMass(Vector2 myCentre)
    {
        Vector2 centre = startingCentre - ((Vector2)marshTransform.position - myCentre);
        centre /= activeBodyCount-1;
        return centre;
    }


    Vector2 Avoid(Vector2 boid, Vector2 obstacle)
    {
        float dist = Vector2.Distance(boid, obstacle);

        if(dist < wispAvoidance)
        {
            //linear separation
            float intensity = (wispSpeed * 5) * ((wispAvoidance - dist) / wispAvoidance);
            return (obstacle-boid) * intensity;
        }

        return Vector2.zero;
    }
    Vector2 AvoidOthers(int currentBoid)
    {
        Vector2 result = Vector2.zero;

        for(int boid = 0; boid < inhabitants.Count; boid++)
        {
            if(inhabitants[boid].activeInSwarm && boid != currentBoid)
            {
                result -= Avoid(inhabitants[currentBoid].Pos(), inhabitants[boid].Pos());
            }
        }

        return result;
    }

    Vector2 RandomOffshoot()
    {
        Vector2 rndm = new Vector2(RNG.GenRand(-10, 21), RNG.GenRand(-10, 21));
        return rndm.normalized;
    }

    Vector2 GravitateToMarsh(Vector2 position)
    {
        return (Vector2)marshTransform.position - position;
    }



    void SelectHomingInhabitant()
    {
        for (bool success = false; success != true;)
        {
            int index = RNG.GenRand(0, inhabitants.Count - 1);
            if (inhabitants[index].activeInSwarm)
            {
                inhabitants[index].SetHomingTarget(target.position, 4);
                success = true;
            }
        }
    }



    public void RemoveInhabitant(int index)
    {
        inhabitants.RemoveAt(index);
    }
    
}
