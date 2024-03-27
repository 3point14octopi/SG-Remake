using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbIdleState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        Debug.Log("IDLE TIME");
        fb.anim.SetBool("Throwing", false);
    }

    public override void UpdateState(FbStateManager fb){

        //Transition for moving 
        if(fb.movement != new Vector2(0 ,0)){
            fb.SwitchState(fb.MoveState);
        }

        //Transition for shooting up
        else if(Input.GetKey(fb.shootUpKey)){
            fb.keyHistory.Push(0);
            fb.SwitchState(fb.ShootState);
        }

        //Transition for shooting down
        else if(Input.GetKey(fb.shootDownKey)){
            fb.keyHistory.Push(1);
            fb.SwitchState(fb.ShootState);
        }

        //Transition for shooting left
        else if(Input.GetKey(fb.shootLeftKey)){
            fb.keyHistory.Push(2);
            fb.SwitchState(fb.ShootState);
        }

        //Transition for shooting right
        else if(Input.GetKey(fb.shootRightKey)){
            fb.keyHistory.Push(3);
            fb.SwitchState(fb.ShootState);
        }

        //Transition for ice block 
        else if(Input.GetKey(fb.iceBlockKey) && fb.iceBlockHP > 0){
            fb.SwitchState(fb.IceblockState);
        }
    }

    public override void FixedUpdateState(FbStateManager fb){
        
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){

    }
}
