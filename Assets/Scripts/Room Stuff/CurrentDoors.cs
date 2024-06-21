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
    bool HasBeenMet(RoomTemplate room);
}

public class RoomClearCondition : DoorConditions
{
    public ConditionTypes Condition { get; set; } = ConditionTypes.ROOM_CLEAR;

    public bool HasBeenMet(RoomTemplate room)
    {
        return room.Cleared;
    }
}

public class BossUnlockCondition : DoorConditions
{
    public ConditionTypes Condition { get; set; } = ConditionTypes.MINIONS_DEFEATED;

    public bool HasBeenMet(RoomTemplate room)
    {
        return (ERoomManager.Instance.IsBossUnlocked());
    }
}

[Serializable]public class Door
{
    public List<DoorConditions> conditionsToOpen = new List<DoorConditions>() { new RoomClearCondition() };

    public bool AllCleared()
    {
        for(int i = 0; i < conditionsToOpen.Count; i++)
        {
            if (!conditionsToOpen[i].HasBeenMet(new RoomTemplate()))
            {
               return false;
            }
            
        }
        return true;
    }
}



public class CurrentDoors : MonoBehaviour
{
    public MiniMap mm;

    public Door[] doors = new Door[4];
    private bool[] openDoors = new bool[] { false, false, false, false };
    private Coord[] doorLocations = new Coord[4];

    public Space2D floorMap;
    public Coord pos;
    public SimpleGridLayer doorLayer;

    public void OnRoomEnter(Coord location)
    {
        doorLayer.Clear();
        doors = new Door[4];
        openDoors = new bool[] { false, false, false, false };
        pos = location;

        doorLocations[0] = new Coord((pos.x * 25) + 12, (pos.y * -15));
        doorLocations[1] = new Coord((pos.x * 25) + 12, (pos.y * -15) - 14);
        doorLocations[2] = new Coord((pos.x * 25), (pos.y * -15) - 7);
        doorLocations[3] = new Coord((pos.x * 25) + 24, (pos.y * -15) - 7);

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

    public void UpdateDoorsInRoom()
    {

    }

}
