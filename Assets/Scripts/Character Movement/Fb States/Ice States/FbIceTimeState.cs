using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbIceTimeState : FbBaseState
{
    public override void EnterState(FbStateManager fb)
    {
        if (fb.currentIceUses <= 1) fb.SwitchState(fb.IdleState);
        else
        {
            Time.timeScale = 0.5f;
            fb.b.currentStats[1] *= 1.5f;
            fb.freezeScreen.SetActive(true);
        }
    }

    public override void UpdateState(FbStateManager fb)
    {
        fb.currentIceUses -= Time.deltaTime;

        //Transition for idle 
        if (Input.GetKeyUp(fb.iceBlockKey))
        {
            Time.timeScale = 1f;
            fb.freezeScreen.SetActive(false);
            fb.SwitchState(fb.IdleState);
            fb.b.currentStats[1] *= 2/3f;
        }

        //Check for if iceblock cracks
        if (fb.currentIceUses <= 0)
        {
            Time.timeScale = 1f;
            fb.freezeScreen.SetActive(false);
            fb.SwitchState(fb.IdleState);
            fb.b.currentStats[1] *= 2 / 3f;
        }
    }

    public override void FixedUpdateState(FbStateManager fb)
    {
        fb.b.Moving();
        fb.g.CanShoot();
    }

    //If youa re hit by anything your iceblock with crack a little bit
    public override void Collision(FbStateManager fb, Collision2D Collision2D)
    {

    }
}
