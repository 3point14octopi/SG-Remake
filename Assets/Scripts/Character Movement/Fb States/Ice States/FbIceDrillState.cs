using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbIceDrillState : FbBaseState
{
    FbBrain brain;
    FbGun gun;
    int lastShot;
    bool directionCalc = false;
    public override void EnterState(FbStateManager fb)
    {
        brain = fb.b;
        gun = fb.g;
        brain.anim.SetBool("IceDrill", true);
        if (gun.keyHistory.Count > 0){
            if (lastShot != gun.keyHistory.Peek()) IceDrillAnimation(gun.keyHistory.Peek());
        }
        else lastShot = brain.direction;

    }

    public override void UpdateState(FbStateManager fb)
    {
        //if (!directionCalc)
        //{
        //    if (brain.anim.GetCurrentAnimatorStateInfo(0).IsName("FrostbiteIceDrillUp")) lastShot = 0;
        //    else if (brain.anim.GetCurrentAnimatorStateInfo(0).IsName("FrostbiteIceDrillLeft")) lastShot = 1;
        //    else if (brain.anim.GetCurrentAnimatorStateInfo(0).IsName("FrostbiteIceDrillDown")) lastShot = 2;
        //    else if (brain.anim.GetCurrentAnimatorStateInfo(0).IsName("FrostbiteIceDrillRight")) lastShot = 3;
        //    else Debug.Log("Animation hasnt triggered yet");
        //    directionCalc = true;

        //}
        if (Input.GetKeyUp(fb.iceBlockKey)){
            brain.anim.SetBool("IceDrill", false);
            fb.g.IceDrill(lastShot);
            directionCalc = false;
            fb.SwitchState(fb.IdleState);
        }

        IceDrillMoving();
        //loop looks gross but if we use an and gate the .peek could throw errors
        if (gun.keyHistory.Count > 0) {
            if (lastShot != gun.keyHistory.Peek()) IceDrillAnimation(gun.keyHistory.Peek());

        }
    }

    public override void FixedUpdateState(FbStateManager fb)
    {

    }

    //If youa re hit by anything your iceblock with crack a little bit
    public override void Collision(FbStateManager fb, Collision2D Collision2D)
    {

    }

    public void IceDrillMoving()
    {
        //If we are moving in both x and y (on a diagonal) we move at a reduced speed
        if (brain.movement.x != 0 && brain.movement.y != 0) brain.rb.MovePosition(brain.rb.position + brain.movement * brain.currentStats[1] * Time.fixedDeltaTime * 0.72f);
        //if not on a diagonal we can move at full speed
        else brain.rb.MovePosition(brain.rb.position + brain.movement * brain.currentStats[1] * Time.fixedDeltaTime);
    }
    private void IceDrillAnimation(int direction)
    {
        if (direction == 0) { brain.anim.Play("FrostbiteIceDrillUp"); }
        else if (direction == 1) { brain.anim.Play("FrostbiteIceDrillLeft"); }
        else if (direction == 2) { brain.anim.Play("FrostbiteIceDrillDown"); }
        else if (direction == 3) { brain.anim.Play("FrostbiteIceDrillRight"); }
        lastShot = direction;
    }
}
