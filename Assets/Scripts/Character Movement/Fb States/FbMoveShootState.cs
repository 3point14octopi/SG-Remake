using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbMoveShootState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        //debug
        Debug.Log("Move & Shoot TIME");
        fb.anim.SetBool("Throwing", true);
    }

    public override void UpdateState(FbStateManager fb){

        //Transition for shoot 
        if(fb.movement == new Vector2(0 ,0)){
            fb.SwitchState(fb.ShootState);
        }

        //Transition for move
        else if(Input.GetKey(fb.shootUpKey) == false && Input.GetKey(fb.shootLeftKey) == false && Input.GetKey(fb.shootDownKey) == false && Input.GetKey(fb.shootRightKey) == false){
            fb.SwitchState(fb.MoveState);
        }

        //Transition for ice block 
        else if(Input.GetKey(fb.iceBlockKey) && fb.iceBlockHP > 0){
            fb.SwitchState(fb.IceblockState);
        }
              
          //Transition for shooting up
        else if(Input.GetKeyDown(fb.shootUpKey)){
            fb.keyHistory.Push(0);
        }

        //Transition for shooting down
        else if(Input.GetKeyDown(fb.shootDownKey)){
            fb.keyHistory.Push(1);
        }

        //Transition for shooting left
        else if(Input.GetKeyDown(fb.shootLeftKey)){
            fb.keyHistory.Push(2);
        }

        //Transition for shooting right
        else if(Input.GetKeyDown(fb.shootRightKey)){
            fb.keyHistory.Push(3);
        }

        //Calls the shooting function
        if(fb.gunTimer <= 0){
            fb.Shooting();
            fb.gunTimer = fb.firerate;
        }
    }

    public override void FixedUpdateState(FbStateManager fb){
        Moving(fb);
    }

    public void Moving(FbStateManager fb){     
        //WASD movement || finds our vector then gives momentum in that direction
        fb.rb.MovePosition(fb.rb.position + fb.movement * fb.movementSpeed * Time.fixedDeltaTime);
    
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){
        //checks for enemies or enemy bullets
        if (Collision2D.gameObject.tag == "EnemyBullet")
        {
            //lowers your health based on how much damage you take
            fb.health = fb.health - Collision2D.gameObject.GetComponent<EnemyBulletBehaviour>().bDamage;
            fb.Flash();
        }
        //checks for enemies
        else if (Collision2D.gameObject.tag == "Enemy"){
            fb.Flash();
        }

        //Updates our health bar
        fb.healthbar.GetComponent<FbHealthBar>().HealthBar(fb.health);

        //Checks if we died
        if(fb.health <= 0){
            fb.SwitchState(fb.DeathState);
        }
    }
}
