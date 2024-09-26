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
    private bool isAlive = true;


    private int currentPhase = 0; //tracks our current phase

    private delegate void PhaseDelegate(); //our delegate variable type
    private PhaseDelegate currentPhaseDelegate; //our current phase is stored in this delegate
    private PhaseDelegate[] PhaseDelegateArray = new PhaseDelegate[5]; //all 5 phases are stored in this array

    void Start()
    {
      // Fills our delegate array with our phase logic
      PhaseDelegateArray = new PhaseDelegate[]
      {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            PhaseFour,
            PhaseFive
      };

    }

    //executes the code of the current phase
    void FixedUpdate()
    {
       if(isAlive) currentPhaseDelegate();
    }

    // Holds the logic for each phase of the boss and the death
    protected abstract void PhaseOne();

    protected abstract void PhaseTwo();

    protected abstract void PhaseThree();

    protected abstract void PhaseFour();

    protected abstract void PhaseFive();

    protected abstract void Death();

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
