using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbIceblockState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        //debug
        Debug.Log("Ice Block Time");
    }

    public override void UpdateState(FbStateManager fb){

        //Transition for idle 
        if(Input.GetKeyUp(fb.iceBlockKey)){
            fb.SwitchState(fb.IdleState);
        }
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){
        //checks for enemies or enemy bullets
        if (Collision2D.gameObject.tag == "Enemy" || (Collision2D.gameObject.tag == "Bullet" && Collision2D.gameObject.GetComponent<BulletBehaviour>().bPlayer == false))
        {
            //Since it is Ice block everything will be blocked
            Debug.Log("Blocked");
        }
    }
}
