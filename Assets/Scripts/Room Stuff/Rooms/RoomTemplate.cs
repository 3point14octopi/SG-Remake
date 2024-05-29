using System;
using System.Collections.Generic;
using JAFprocedural;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class RoomTemplate:MonoBehaviour
{
    public bool ContainsEnemies { get; protected set; }
    public bool ContainsTraps { get; protected set; }
    public bool Cleared { get; protected set; }
    public Space2D roomLayout { get; protected set; }
    protected List<Brain> roomEnemies;
    protected int population = 0;


    public virtual void AssignMap(Space2D map) 
    {
        roomLayout = map;
    }

    public virtual void InitEnemyList() 
    { 
        
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
        }
    }
    public virtual void AddEnemy(Brain enemy) 
    {
        
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
        }
    }

    protected virtual void SpawnEnemies()
    {
        
    }
}
