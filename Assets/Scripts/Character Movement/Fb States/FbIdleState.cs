using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbIdleState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        Debug.Log("IDLE TIME");
        fb.b.anim.SetBool("Throwing", false);
    }

    public override void UpdateState(FbStateManager fb){

        //Transition for moving 
        if(fb.b.movement != new Vector2(0 ,0)){
            fb.SwitchState(fb.MoveState);
        }

       
        //Transition for ice block 
        else if(Input.GetKey(fb.iceBlockKey) && fb.iceBlockHP > 0){
            fb.SwitchState(fb.currentIceState);
        }
    }

    public override void FixedUpdateState(FbStateManager fb){
        
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){

    }
}
