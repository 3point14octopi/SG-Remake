using JAFprocedural;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomPop : MonoBehaviour
{
    public CurrentDoors doorManager;
    public BossRoomManager bossStuff;

    public RoomWithEnemies currentRoom;

    public Space2D floorPlan;
    public Coord position;

    public static RoomPop Instance;

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

        if (currentRoom.isBossRoom)
        {
            Debug.Log("why do i hear boss music?");
            bossStuff.LoadMap(ERoomManager.Instance.RequestRoom(position));
            bossStuff.Attack1();
        }else if(currentRoom.enemies.Count > 0 && currentRoom.enemies[0].enemyID == 15)//wisp
        {
            Debug.Log("not these bitches again!");
            WispManager.Instance.AssignMap(ERoomManager.Instance.RequestRoom(position));
        }


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
        return currentRoom.Population();
    }
}
