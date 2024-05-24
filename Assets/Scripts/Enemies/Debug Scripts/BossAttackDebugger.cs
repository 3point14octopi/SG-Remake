using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAFprocedural;

public class BossAttackDebugger : MonoBehaviour
{
    private Space2D harryRoom;
    public GameObject pumpkinPrince;

    // Start is called before the first frame update
    void Start()
    {
        harryRoom = new Space2D(25, 15);
        harryRoom.worldOrigin =  new Coord(-12, 7);
        BossRoomManager.Instance.LoadMap(harryRoom);

    }

    // Update is called once per frame
    void Update()
    {
        //Calls Vines
        if(Input.GetKeyDown(KeyCode.I)){
            BossRoomManager.Instance.Attack2();
        }

        //Calls Fire
        if(Input.GetKeyDown(KeyCode.O)){
            BossRoomManager.Instance.Attack1();            
        }
        
        //Calls Lantern
        if(Input.GetKeyDown(KeyCode.P)){            
            BossRoomManager.Instance.Attack3();            
        }
    }
}
