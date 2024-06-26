using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemylessRoom : RoomTemplate
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            FollowCam.Instance.chaseTarget = true;
            FollowCam.Instance.ZoomIn();
        }
        
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FollowCam.Instance.chaseTarget = false;
        }
    }
}
