using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbIceblockState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        //debug
        Debug.Log("Ice Block Time");
        fb.anim.SetBool("Iceblock", true);
    }

    public override void UpdateState(FbStateManager fb){

        //Transition for idle 
        if(Input.GetKeyUp(fb.iceBlockKey)){
            fb.anim.SetBool("Iceblock", false);
            fb.SwitchState(fb.IdleState);
        }
    }

    public override void FixedUpdateState(FbStateManager fb){

    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){
                //checks for enemies or enemy bullets
        if (Collision2D.gameObject.tag == "EnemyBullet")
        {
            //Since it is Ice block everything will be blocked
            Debug.Log("Blocked");
        }
        //checks for enemies
        else if (Collision2D.gameObject.tag == "Enemy"){
            //Since it is Ice block everything will be blocked
            Debug.Log("Blocked");
        }
    }
}
