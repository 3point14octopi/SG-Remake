using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelPhases : PhaseFrameWork
{
    public GameObject player;
    private GameObject Acorn;

    private bool chasingAcorn = false;
    public float speed; // Speed of the squirrel
    public Rigidbody2D rb;


    void Start()
    {
        speed = gameObject.GetComponent<Brain>().currentStats[1];
        rb = gameObject.GetComponent<Rigidbody2D>();
        // Fills our delegate array with our phase logic
        PhaseDelegateArray = new PhaseDelegate[]
        {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            PhaseFour,
            PhaseFive
        };
        currentPhaseDelegate = PhaseDelegateArray[currentPhase];
    }




    protected override void PhaseOne()
    {
        if (chasingAcorn)
        {
            rb.MovePosition(new Vector2(transform.position.x - Acorn.transform.position.x, transform.position.y - Acorn.transform.position.y) * Time.fixedDeltaTime);
        }
    }


    /// <summary>
    /// Used by bushes to tell the squirrel it has dropped a nut
    /// </summary>
    /// <param Acorn Reference="a"></param>
    public void GoToAcorn(GameObject a)
    {
        Acorn = a;
        chasingAcorn = true;
    }

}
