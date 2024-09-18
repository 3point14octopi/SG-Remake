using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbIceDrillState : FbBaseState
{
    FbBrain brain;
    FbGun gun;
    int lastShot;
    public override void EnterState(FbStateManager fb)
    {
        brain = fb.b;
        gun = fb.g;
    }

    public override void UpdateState(FbStateManager fb)
    {
        if (Input.GetKeyUp(fb.iceBlockKey)){
            if (lastShot == 0) { brain.anim.Play("FrostbiteWalkUp"); }
            else if (lastShot == 1) { brain.anim.Play("FrostbiteWalkLeft"); }
            else if (lastShot == 2) { brain.anim.Play("FrostbiteWalkDown"); }
            else if (lastShot == 3) { brain.anim.Play("FrostbiteWalkRight"); }
            fb.g.IceDrill(lastShot);
            fb.SwitchState(fb.IdleState);
        }

        IceDrillMoving();
        if (gun.keyHistory.Count > 0) { if (lastShot != gun.keyHistory.Peek()) IceDrillAnimation(gun.keyHistory.Peek())};
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
