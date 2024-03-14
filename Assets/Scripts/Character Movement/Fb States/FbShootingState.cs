using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbShootingState : FbBaseState
{
    public Stack<KeyCode> stack = new Stack<KeyCode>();
    public override void EnterState(FbStateManager fb){
        //debug
        Debug.Log("Shoot TIME");
    }

    public override void UpdateState(FbStateManager fb){

        //Transition for move
        if(Input.GetKey(fb.shootUpKey) == false && Input.GetKey(fb.shootLeftKey) == false && Input.GetKey(fb.shootDownKey) == false && Input.GetKey(fb.shootRightKey) == false){
            fb.SwitchState(fb.IdleState);
        }

        //Transition for move and shoot 
        else if(fb.movement != new Vector2(0 ,0)){
            fb.SwitchState(fb.MoveShootState);
        }

        //Transition for ice block 
        else if(Input.GetKey(fb.iceBlockKey)){
            fb.SwitchState(fb.IceblockState);
        }

        //establishes the direction for the bullet
        for (int i = 0; i <= fb.shootKeys.Length - 1; i++){
            if(Input.GetKey(fb.shootKeys[i])){
            fb.direction = i;
            Debug.Log( fb.direction);
            }
        }

        //Calls the shooting function
        if(fb.gunTimer <= 0){
            fb.Shooting();
            fb.gunTimer = fb.firerate;
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
