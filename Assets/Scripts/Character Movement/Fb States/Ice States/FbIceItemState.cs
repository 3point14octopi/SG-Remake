using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class FbIceItemState : FbBaseState
{
    private int direction = 0;
    public override void EnterState(FbStateManager fb)
    {
        fb.indicator.SetActive(true);
    }

    public override void UpdateState(FbStateManager fb)
    {
        //calculates the direction so we can put the indicator in the right spot
        for (int i = 0; i < fb.g.shootKeys.Count; i++)
        {
            if (Input.GetKey(fb.g.shootKeys[i]))
            {
                direction = i;
            }
        }

        //transit back to idle and place the block
        if (Input.GetKeyUp(fb.iceBlockKey))
        {
            fb.PlaceItem();
            fb.indicator.SetActive(false);
            fb.SwitchState(fb.IdleState);
        }
    }

    //Preplace manages the indicator
    public override void FixedUpdateState(FbStateManager fb)
    {
        fb.Preplace(direction);
        fb.b.Moving();
    }

    public override void Collision(FbStateManager fb, Collision2D Collision2D)
    {

    }

}
