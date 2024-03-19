using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbMoveState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        //debug
        Debug.Log("Move TIME");
        fb.anim.SetBool("Throwing", false);
    }

    public override void UpdateState(FbStateManager fb){

        //Transition for idle
        if(fb.movement == new Vector2(0 ,0)){
            fb.SwitchState(fb.IdleState);
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
        else if(Input.GetKey(fb.iceBlockKey)){
            fb.SwitchState(fb.IceblockState);
        }

    }

    public override void FixedUpdateState(FbStateManager fb){ 
        Moving(fb);
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){
        //checks for enemies or enemy bullets
        if (Collision2D.gameObject.tag == "EnemyBullet")
        {
            //lowers your health based on how much damage you take
            fb.health = fb.health - Collision2D.gameObject.GetComponent<EnemyBulletBehaviour>().bDamage;
        }
        //checks for enemies
        else if (Collision2D.gameObject.tag == "Enemy"){

        }

        //Updates our health bar
        fb.healthbar.GetComponent<FbHealthBar>().HealthBar(fb.health);

        //Checks if we died
        if(fb.health <= 0){
            fb.SwitchState(fb.DeathState);
        }
    }

    public void Moving(FbStateManager fb){     
        //WASD movement || finds our vector then gives momentum in that direction
        fb.rb.MovePosition(fb.rb.position + fb.movement * fb.movementSpeed * Time.fixedDeltaTime);
        if(fb.movement.x == 1){fb.anim.Play("FrostbiteWalkRight");}
        else if(fb.movement.x == -1){fb.anim.Play("FrostbiteWalkLeft");}
        else if(fb.movement.x == 0 && fb.movement.y == 1){fb.anim.Play("FrostbiteWalkUp");}
        else if(fb.movement.x == 0 && fb.movement.y == -1){fb.anim.Play("FrostbiteWalkDown");}
    
    }
}
