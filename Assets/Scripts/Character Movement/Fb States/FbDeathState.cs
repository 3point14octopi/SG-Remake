using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbDeathState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        
        Debug.Log("DEATH TIME");
    }

    public override void UpdateState(FbStateManager fb){

        Debug.Log("You died :((");
    }

    public override void FixedUpdateState(FbStateManager fb){
        
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){

    }
}
