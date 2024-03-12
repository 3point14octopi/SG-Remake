using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbMoveState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        //debug
        Debug.Log("Move TIME");
    }

    public override void UpdateState(FbStateManager fb){

        //Transition for idle
        if(fb.horizontalInput == 0f && fb.verticalInput == 0f){
            fb.SwitchState(fb.IdleState);
        }

        //Transition for move and shoot 
        else if(Input.GetKey(fb.shootUpKey) || Input.GetKey(fb.shootLeftKey) || Input.GetKey(fb.shootDownKey) || Input.GetKey(fb.shootRightKey)){
        for (int i = 0; i <= fb.shootKeys.Length - 1; i++){
            if(Input.GetKeyDown(fb.shootKeys[i])){
                fb.direction = i;
                Debug.Log( fb.direction);
            }
        }
            fb.SwitchState(fb.MoveShootState);
        }

        //Transition for ice block 
        else if(Input.GetKey(fb.iceBlockKey)){
            fb.SwitchState(fb.IceblockState);
        }

        Moving(fb);
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){

    }

    public void Moving(FbStateManager fb){     
        //WASD movement || finds our vector then gives momentum in that direction
        fb.rb.velocity = new Vector2 (fb.horizontalInput*fb.movementSpeed, fb.verticalInput*fb.movementSpeed);
    
    }
}
