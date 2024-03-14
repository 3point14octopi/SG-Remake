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
        if(fb.movement != new Vector2(0 ,0)){
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

    public override void FixedUpdateState(FbStateManager fb){
        
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){
        //checks for enemies or enemy bullets
        if (Collision2D.gameObject.tag == "EnemyBullet")
        {
            //lowers your health based on how much damage you take
            fb.health = fb.health - Collision2D.gameObject.GetComponent<EnemyBulletBehaviour>().bDamage;
            if(fb.health <= 0){
                fb.SwitchState(fb.DeathState);
            }
        }
        //checks for enemies
        else if (Collision2D.gameObject.tag == "Enemy"){

        }
    }
}
