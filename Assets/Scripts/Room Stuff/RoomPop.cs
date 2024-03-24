using JAFprocedural;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPop : MonoBehaviour
{
    public CurrentDoors doorManager;

    public RoomWithEnemies currentRoom;

    public Space2D floorPlan;
    public Coord position;

    public static RoomPop Instance = new RoomPop();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public void LoadRoom(RoomWithEnemies list, Coord currentPos)
    {
        currentRoom = list;
        Debug.Log(list.enemies.Count);
        if(list.Population() > 0)
        {
            for(int i = 0; i < list.enemies.Count; i++)
            {
                EObjPool.Instance.enemyPool[list.enemies[i].enemyID].SetActive(true);
                EObjPool.Instance.enemyPool[list.enemies[i].enemyID].transform.position = new Vector3(list.enemies[i].startingLocation.x +0.5f, -list.enemies[i].startingLocation.y+0.5f, 0);
            }
        }

        position = currentPos;
        InitDoors();
    }

    public void EnemyKilled()
    {
        currentRoom.KillAnEnemy();
        UpdateDoors();

        if(currentRoom.Population() == 0)
        {
            if (ERoomManager.Instance.IsFloorCleared())
            {
                Debug.Log("wowie");
            }
        }
    }

    private void InitDoors()
    {
        doorManager.OnRoomEnter(position);
    }

    public void UpdateDoors()
    {
        doorManager.UpdateDoorsInRoom();
    }

    public int CurrentPopulation()
    {
        Debug.Log(currentRoom.Population().ToString() + " remaining");
        return currentRoom.Population();
    }
}
