using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbMoveState : FbBaseState
{

    public override void EnterState(FbStateManager fb){

    }

    public override void UpdateState(FbStateManager fb){

        //Transition for idle
        if (fb.b.movement == new Vector2(0, 0)){
            fb.SwitchState(fb.IdleState);
        }


        //Transition for ice block 
        else if(Input.GetKey(fb.iceBlockKey) && fb.currentIceUses > 0){
            fb.SwitchState(fb.currentIceState);
        }

    }
    //CanShoot is only available from states Idle and Move
    public override void FixedUpdateState(FbStateManager fb){ 
        fb.b.Moving();
        fb.g.CanShoot();
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){
   
    }

}
