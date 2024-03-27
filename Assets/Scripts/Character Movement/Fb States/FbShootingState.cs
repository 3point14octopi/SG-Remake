using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbShootingState : FbBaseState
{
    public Stack<KeyCode> stack = new Stack<KeyCode>();
    public override void EnterState(FbStateManager fb){
        //debug
        Debug.Log("Shoot TIME");
        fb.anim.SetBool("Throwing", true);
    }

    public override void UpdateState(FbStateManager fb){

        //Transition for move
        if(fb.keyHistory.Count == 0){
            fb.SwitchState(fb.IdleState);
        }

        //Transition for move and shoot 
        else if(fb.movement != new Vector2(0 ,0)){
            fb.SwitchState(fb.MoveShootState);
        }

        //Transition for ice block 
        else if(Input.GetKeyDown(fb.iceBlockKey) && fb.iceBlockHP > 0){
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
        
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){
<<<<<<< Updated upstream
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
=======
>>>>>>> Stashed changes

    }

}
