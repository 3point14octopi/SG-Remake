using System;
using System.Collections.Generic;
using JAFprocedural;
using UnityEngine;

public class RoomTemplate:MonoBehaviour
{
    public bool ContainsEnemies { get; protected set; }
    public bool ContainsTraps { get; protected set; }
    public bool Cleared { get; protected set; }
    public Space2D roomLayout { get; protected set; }
    protected List<Brain> roomEnemies = new List<Brain>();
    protected List<Coord> enemySpawnPositions = new List<Coord>();
    public int population = 0;
    public int rType = 1;
    protected List<DoorTileBehaviour> doors = new List<DoorTileBehaviour>();

    private void Start()
    {
        //AddEnemy(EObjPool.Instance.enemyPool[0].GetComponent<Brain>());
        //enemySpawnPositions.Add(new Coord(7, 4));
        InitEnemyList();
    }

    public virtual void AssignMap(Space2D map) 
    {
        roomLayout = map;
    }

    public virtual void InitEnemyList() 
    { 
        foreach(RoomEnemy enemy in ERoomManager.Instance.Populate(roomLayout, rType))
        {
            Debug.Log(enemy.enemyID.ToString() + " at " + enemy.startingLocation.x.ToString() + ',' + enemy.startingLocation.y.ToString());
        }
    }
    public virtual void RemoveEnemy(int index) 
    {
        if (index >= 0 && index < population)
        {
            roomEnemies.RemoveAt(index);
            population--;
            //for (int i = 0; i < population; roomEnemies[i].roomID = (i + 0), i++) ;
        }

        if (population == 0)
        {
            Cleared = true;
            DoorGridLayer.Instance.OpenShutDoors(true);
        }
    }
    public virtual void AddEnemy(Brain enemy) 
    {
        roomEnemies.Add(enemy);
        population++;
    }
    public virtual int GetPopulation()
    {
        return population;
    }


    public virtual void OnUpdate() 
    {
        
    }
    public virtual void OnEnter() {
        if(population > 0)
        {
            SpawnEnemies();
            DoorGridLayer.Instance.OpenShutDoors(false);
        }

        Debug.Log("you entered a room");
    }

    protected virtual void SpawnEnemies()
    {
        for (int i = 0; i < roomEnemies.Count; i++)
        {
            roomEnemies[i].gameObject.SetActive(true);
            roomEnemies[i].roomIndex = i + 0;
            roomEnemies[i].mom = this;
            roomEnemies[i].transform.position = new Vector3(enemySpawnPositions[i].x + roomLayout.worldOrigin.x + 0.5f, -enemySpawnPositions[i].y - roomLayout.worldOrigin.y + 0.5f, 0);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            FollowCam.Instance.SlideToNew(transform.position + new Vector3(0, 0.5f, 0));
            OnEnter();
        }
        
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    public virtual void PlaceDoors(Coord origin)
    {
        DoorGridLayer.Instance.Draw(new Vector3Int(origin.x + 7, -origin.y + 1, 0));
        DoorGridLayer.Instance.Draw(new Vector3Int(origin.x -1, -origin.y -4, 0));
        DoorGridLayer.Instance.Draw(new Vector3Int(origin.x + 7, -origin.y -9, 0));
        DoorGridLayer.Instance.Draw(new Vector3Int(origin.x +15, -origin.y - 4, 0));

        for(int i = DoorGridLayer.Instance.instantiatedDoors.Count-1; i >= DoorGridLayer.Instance.instantiatedDoors.Count - 4; i--)
        {
            doors.Add(DoorGridLayer.Instance.instantiatedDoors[i]);
        }

        foreach (DoorTileBehaviour dtb in doors) dtb.tileState = DoorStates.OPEN;
    } 
}
