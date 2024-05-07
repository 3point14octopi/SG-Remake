using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbIceblockState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        //debug
        Debug.Log("Ice Block Time");
        fb.iFrame = true;
        fb.anim.SetBool("Iceblock", true);
        fb.anim.SetInteger("IceblockHits", fb.iceBlockHP);
    }

    public override void UpdateState(FbStateManager fb){

        //Transition for idle 
        if(Input.GetKeyUp(fb.iceBlockKey)){
            fb.anim.SetBool("Iceblock", false);
            fb.SwitchState(fb.IdleState);
            fb.iFrame = false;
        }

        //Check for if iceblock cracks
        if(fb.iceBlockHP <= 0){
            fb.anim.SetBool("Iceblock", false);
            fb.SwitchState(fb.IdleState);
            fb.iFrame = false;
        }
    }

    public override void FixedUpdateState(FbStateManager fb){

    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){

        fb.iceBlockHP--;
        fb.iceBar.GetComponent<FBIceBar>().IceBar(fb.iceBlockHP);
        fb.anim.SetInteger("IceblockHits", fb.iceBlockHP);
    }
}
