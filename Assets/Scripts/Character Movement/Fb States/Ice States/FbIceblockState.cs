using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbIceblockState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        fb.b.iFrame = true;
        fb.b.anim.SetBool("Iceblock", true);
        fb.b.anim.SetInteger("IceblockHits", fb.currentIceUses);
    }

    public override void UpdateState(FbStateManager fb){

        //Transition for idle 
        if(Input.GetKeyUp(fb.iceBlockKey)){
            fb.b.anim.SetBool("Iceblock", false);
            fb.SwitchState(fb.IdleState);
            fb.b.iFrame = false;
        }

        //Check for if iceblock cracks
        if(fb.currentIceUses <= 0){
            fb.b.anim.SetBool("Iceblock", false);
            fb.SwitchState(fb.IdleState);
            fb.b.iFrame = false;
        }
    }

    public override void FixedUpdateState(FbStateManager fb){

    }
    
    //If youa re hit by anything your iceblock with crack a little bit
    public override void Collision(FbStateManager fb, Collision2D Collision2D){
        if (fb.b.damageTags.Contains(Collision2D.gameObject.tag))
        {
            fb.currentIceUses--;
            fb.b.iceBar.GetComponent<FBIceBar>().IceBar(fb.currentIceUses);
            fb.b.anim.SetInteger("IceblockHits", fb.currentIceUses);
        }

    }
}
