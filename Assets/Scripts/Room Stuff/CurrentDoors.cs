using JAFprocedural;
using System;
using System.Collections.Generic;
using UnityEngine;


public enum ConditionTypes
{
    ROOM_CLEAR = 0,
    MINIONS_DEFEATED = 1
}

public interface DoorConditions
{
    ConditionTypes Condition { get; }
    bool HasBeenMet();
}

public class RoomClearCondition : DoorConditions
{
    public ConditionTypes Condition { get; set; } = ConditionTypes.ROOM_CLEAR;

    public bool HasBeenMet()
    {
        return (RoomPop.Instance.CurrentPopulation() == 0);
    }
}

public class BossUnlockCondition : DoorConditions
{
    public ConditionTypes Condition { get; set; } = ConditionTypes.MINIONS_DEFEATED;

    public bool HasBeenMet()
    {
        return (ERoomManager.Instance.IsBossUnlocked());
    }
}

[Serializable]public class Door
{
    public List<DoorConditions> conditionsToOpen = new List<DoorConditions>() { new RoomClearCondition() };

    public bool AllCleared()
    {
        Debug.Log(conditionsToOpen.Count.ToString() + " conditions");
        for(int i = 0; i < conditionsToOpen.Count; i++)
        {
            if (conditionsToOpen[i].HasBeenMet())
            {
                Debug.Log("condition met");
            }
            else
            {
                Debug.Log("failed condition " + i.ToString());
                return false;
            }
            
        }
        return true;
    }
}



public class CurrentDoors : MonoBehaviour
{

    public Door[] doors = new Door[4];
    private bool[] openDoors = new bool[] { false, false, false, false };
    private Coord[] doorLocations = new Coord[4];

    public Space2D floorMap;
    public Coord pos;
    public JAFGridLayer doorLayer;
    
    public void OnRoomEnter(Coord location)
    {
        doorLayer.Clear();
        openDoors = new bool[] { false, false, false, false };
        pos = location;

        doorLocations[0] = new Coord((pos.x * 25) + 12, (pos.y * -15));
        doorLocations[1] = new Coord((pos.x * 25) + 12, (pos.y * -15) - 14);
        doorLocations[2] = new Coord((pos.x * 25), (pos.y * -15) - 7);
        doorLocations[3] = new Coord((pos.x * 25) + 24, (pos.y * -15) - 7);


        //check north door
        if (pos.y > 0 && floorMap.GetCell(pos.x, pos.y - 1) != 0) InitDoor(0, floorMap.GetCell(pos.x, pos.y-1) == 7);
        //we automatically assume the door is "open" otherwise
        else openDoors[0] = true;

        //check south door
        if (pos.y < floorMap.height - 1 && floorMap.GetCell(pos.x, pos.y + 1) != 0) InitDoor(1, floorMap.GetCell(pos.x, pos.y + 1) == 7);
        else openDoors[1] = true;

        //check east door
        if (pos.x > 0 && floorMap.GetCell(pos.x - 1, pos.y) != 0) InitDoor(2, floorMap.GetCell(pos.x - 1, pos.y) == 7);
        else openDoors[2] = true;

        //check final door, you can probably guess which one it is
        if (pos.x < floorMap.width - 1 && floorMap.GetCell(pos.x + 1, pos.y) != 0) InitDoor(3, floorMap.GetCell(pos.x + 1, pos.y) == 7);
        else openDoors[3] = true;
    }

    public void UpdateDoorsInRoom()
    {
        for (int i = 0; i < 4; CheckDoor(i), i++) ;
    }




    private void InitDoor(int index, bool bossDoor = false)
    {
        doors[index] = new Door();
        if (bossDoor)
        {
            doors[index].conditionsToOpen.Add(new BossUnlockCondition());
        }

        if (!doors[index].AllCleared()) doorLayer.Draw(new Vector3Int(doorLocations[index].x, doorLocations[index].y, 0), ((bossDoor)?1:0));
        else openDoors[index] = true;
    }

    private void CheckDoor(int index)
    {
        if (!openDoors[index] && doors[index].AllCleared())
        {
            doorLayer.RemoveTile(new Vector3Int(doorLocations[index].x, doorLocations[index].y, 0));
            openDoors[index] = true;
        }
    }

}
