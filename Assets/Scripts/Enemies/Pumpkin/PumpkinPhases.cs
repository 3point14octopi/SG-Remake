using EntityStats;
using JAFprocedural;
using System;
using UnityEngine;

public interface PrincePhase
{
    //called when the phase starts
    void OnPhaseStart(PrinceController pcon);
    //called once per frame while the phase is active
    void OnPhaseUpdate();
    //validate the current phase 
    void CheckPhaseStatus();
    //cleanup and calling of next phase
    void OnPhaseEnd();
}

public class FirePhase : PrincePhase
{
    PrinceController pRef;

    public void OnPhaseStart(PrinceController pcon)
    {
        pRef = pcon;
        pcon.StartCoroutine(pcon.FunnyFireAttack(true));
    }

    public void OnPhaseUpdate()
    {
        //this one doesn't have a whole lot happening in the update loop lol
    }

    public void CheckPhaseStatus()
    {
        if (pRef.princeBrain.currentStats[(int)EntityStat.Health] <= 400)
        {
            OnPhaseEnd();
        }
    }

    public void OnPhaseEnd()
    {
        pRef.StopAllCoroutines();
        pRef.attackPhase = new VinePhase();
        pRef.attackPhase.OnPhaseStart(pRef);
    }
}

public class VinePhase : PrincePhase
{
    PrinceController pRef;
    float countdown;

    public void OnPhaseStart(PrinceController pCon)
    {
        pRef = pCon;
        pRef.ToggleCollider(false);
        pRef.StartCoroutine(pRef.VineAttack());
        countdown = RNG.GenRand(400, 400) / 100;
    }

    public void OnPhaseUpdate()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0)
        {
            pRef.SingleShot(2);
            countdown = RNG.GenRand(90, 400) / 100;
        }
    }

    public void CheckPhaseStatus()
    {
        if (pRef.princeBrain.currentStats[(int)EntityStat.Health] <= 200)
        {
            OnPhaseEnd();
        }
    }

    public void OnPhaseEnd()
    {
        pRef.ToggleCollider(true);
        pRef.attackPhase = new LanternPhase();
        pRef.attackPhase.OnPhaseStart(pRef);
    }
}

public class LanternPhase : PrincePhase
{
    PrinceController pRef;
    float countdown;
    float warningShot;
    bool doSpin;

    public void OnPhaseStart(PrinceController pcon)
    {
        pRef = pcon;
        pRef.InstantiateLanterns();
        pRef.SendToStartPosition();

        countdown = RNG.GenRand(500, 300) / 100;
        warningShot = RNG.GenRand(100, 1000) / 100;
        doSpin = false;
    }

    public void OnPhaseUpdate()
    {
        countdown -= Time.deltaTime;
        warningShot -= Time.deltaTime;
        if (warningShot <= 0)
        {
            pRef.SingleShot(2);
            warningShot = RNG.GenRand(100, 1000) / 100;
        }

        if (countdown <= 0)
        {
            if(doSpin && RNG.GenRand(0, 2) > 0)
            {
                pRef.SpinBlast();
                countdown = RNG.GenRand(700, 300) / 100;
            }
            else
            {
                pRef.Blast();
                countdown = RNG.GenRand(500, 300) / 100;
            }
            
        }
    }

    public void CheckPhaseStatus()
    {
        float health = pRef.princeBrain.currentStats[(int)EntityStat.Health];
        if (health <= 100 && !doSpin)
        {
            doSpin = true;
        }else if(health <= 0)
        {
            OnPhaseEnd();
        }
    }

    public void OnPhaseEnd()
    {
        pRef.StopAllCoroutines();
    }
}