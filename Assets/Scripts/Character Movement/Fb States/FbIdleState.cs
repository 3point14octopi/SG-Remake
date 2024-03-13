using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbIdleState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        //debug
        Debug.Log("IDLE TIME");
    }

    public override void UpdateState(FbStateManager fb){

        //Transition for moving 
        if(fb.horizontalInput != 0f || fb.verticalInput != 0f){
            fb.SwitchState(fb.MoveState);
        }

        //Transition for shooting 
        else if(Input.GetKey(fb.shootUpKey) || Input.GetKey(fb.shootLeftKey) || Input.GetKey(fb.shootDownKey) || Input.GetKey(fb.shootRightKey)){
            fb.SwitchState(fb.ShootState);
            for (int i = 0; i <= fb.shootKeys.Length - 1; i++){
                if(Input.GetKeyDown(fb.shootKeys[i])){
                    fb.direction = i;
                    Debug.Log( fb.direction);
                }
            }
        }

        //Transition for ice block 
        else if(Input.GetKey(fb.iceBlockKey)){
            fb.SwitchState(fb.IceblockState);
        }
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){
        //checks for enemies or enemy bullets
        if (Collision2D.gameObject.tag == "Enemy" || (Collision2D.gameObject.tag == "Bullet" && Collision2D.gameObject.GetComponent<BulletBehaviour>().bPlayer == false))
        {
            //lowers your health based on how much damage you take
            fb.health = fb.health - Collision2D.gameObject.GetComponent<BulletBehaviour>().bDamage;
            if(fb.health <= 0){
                fb.SwitchState(fb.DeathState);
            }
        }
    }
}
