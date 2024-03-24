using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbDeathState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        
        Debug.Log("DEATH TIME");
        fb.anim.Play("FrostbiteDeath");
        fb.rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public override void UpdateState(FbStateManager fb){

    }

    public override void FixedUpdateState(FbStateManager fb){
        
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){

    }
}
