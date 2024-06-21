using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbIdleState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        fb.b.anim.SetBool("Throwing", false);
    }

    public override void UpdateState(FbStateManager fb){

        //Transition for moving 
        if(fb.b.movement != new Vector2(0 ,0)){
            fb.SwitchState(fb.MoveState);
        }

       
        //Transition for ice block 
        else if(Input.GetKey(fb.iceBlockKey) && fb.currentIceUses > 0){
            fb.SwitchState(fb.currentIceState);
        }
    }

    //CanShoot is only available from states Idle and Move
    public override void FixedUpdateState(FbStateManager fb){
        fb.g.CanShoot();
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){

    }
}
