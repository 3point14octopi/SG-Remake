using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhaseFrameWork : MonoBehaviour
{
    [Tooltip("Access to the bosses brain so we can change and reference stats")]
    public Brain brain;

    [Tooltip("Reference to the bosses animator")]
    public Animator anim;

    [Range(1, 5)]
    [Tooltip("Number of phases this boss fight has")]
    public int totalPhases;

    [Tooltip("The amount of health the boss will have left when a phase is over")]
    public float[] PhaseHealth = new float[4];
    protected bool isAlive = true;


    protected int currentPhase = 0; //tracks our current phase

    protected delegate void PhaseDelegate(); //our delegate variable type
    protected PhaseDelegate currentPhaseDelegate; //our current phase is stored in this delegate
    protected PhaseDelegate[] PhaseDelegateArray = new PhaseDelegate[5]; //all 5 phases are stored in this array


    //executes the code of the current phase
    void FixedUpdate()
    {
       if(isAlive) currentPhaseDelegate();
    }

    


    // Holds the logic for each phase of the boss and the death
    protected virtual void PhaseOne()
    {

    }

    protected virtual void PhaseTwo()
    {

    }

    protected virtual void PhaseThree()
    {

    }

    protected virtual void PhaseFour()
    {

    }

    protected virtual void PhaseFive()
    {

    }

    /// <summary>
    /// you MUST implement an override for each boss
    /// </summary>
    protected virtual void Death()
    {
        Debug.Log("this idiot didn't implement a death function!");
    }

    //called as a reaction from the brain
    public void OnDeath()
    {
        currentPhaseDelegate = null;
        isAlive = false;
        Death();
    }

    //called by the brain damage reaction to see if we need to change phases
    public void PhaseGate(float health)
    {
        if(health < PhaseHealth[currentPhase] && currentPhase < totalPhases -1)
        {
            currentPhase++;
            currentPhaseDelegate = PhaseDelegateArray[currentPhase];

        }
    }
}
