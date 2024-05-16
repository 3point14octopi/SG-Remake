using System;
using System.Collections.Generic;
using UnityEngine;
using JAFprocedural;

public class BasicEnemyRoom : RoomTemplate
{
    public bool ContainsEnemies { get; private set; } = true;
    public bool ContainsTraps { get; private set; } = false;
    public bool Cleared { get; private set; } = false;

    public Space2D roomLayout { get; private set; }
    private List<GameObject> roomEnemies;
    private int population;


    public void AssignMap(Space2D map)
    {
        roomLayout = map;
        //could do more complex stuff here if it's like a wisp room or something
    } 

    public void InitEnemyList()
    {

    }

    public void AddEnemy(GameObject enemy)
    {
        population++;
    }

    public void RemoveEnemy(int index)
    {
        if(index >= 0 && index < population)
        {
            roomEnemies.RemoveAt(index);
            population--;

            //TO IMPLEMENT
            //for(int i = 0; i < population; roomEnemies[i].GetComponent<Enemy>().roomIndex = i, i++);
        }

        if(population == 0)
        {
            Cleared = true;
        }
    }

    public int GetPopulation()
    {
        return population;
    }



    public void OnEnter()
    {
        //lock the cameras... deadass
        if (!Cleared)
        {
            //place enemies
        }
    }

    public void OnUpdate()
    {
        //this one doesn't do too much
    }
} 
