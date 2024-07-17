using System;
using System.Collections.Generic;
using JAFprocedural;
using UnityEngine;

public class BossRoomTemplate:RoomTemplate
{

    private void Start()
    {
        rType = 9;
        
        InitEnemyList();
    }

    public override void RemoveEnemy(int index) 
    {
        if (index >= 0 && index < population)
        {
            roomEnemies.RemoveAt(index);
            population--;
            for (int i = 0; i < population; roomEnemies[i].roomIndex = (i + 0), i++) ;
        }

        if (population == 0)
        {
            Cleared = true;
            DoorGridLayer.Instance.OpenShutDoors(true);

            //drama??? portal to next level?
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject.Find("Frostbite").transform.position = transform.position + new Vector3(6, 0, 0);
            FollowCam.Instance.ForceJump(transform.position);
        }
    }

    public override void OnUpdate() 
    {
        //???
    }
    public override void OnEnter() {
        //unlock all of the other entrances isaac-style
        foreach (DoorTileBehaviour dtb in doors) dtb.tileState = DoorStates.OPEN;
        
        base.OnEnter();


        //music? drama? cutscene?
    }


    public override void PlaceDoors(Coord origin)
    {
        DoorGridLayer.Instance.Draw(new Vector3Int(origin.x + 7, -origin.y + 1, 0));
        DoorGridLayer.Instance.Draw(new Vector3Int(origin.x -1, -origin.y -4, 0));
        DoorGridLayer.Instance.Draw(new Vector3Int(origin.x + 7, -origin.y -9, 0));
        DoorGridLayer.Instance.Draw(new Vector3Int(origin.x +15, -origin.y - 4, 0));

        for(int i = DoorGridLayer.Instance.instantiatedDoors.Count-1; i >= DoorGridLayer.Instance.instantiatedDoors.Count - 4; i--)
        {
            doors.Add(DoorGridLayer.Instance.instantiatedDoors[i]);
        }

        foreach (DoorTileBehaviour dtb in doors) dtb.tileState = DoorStates.LOCKED;
    } 
}
