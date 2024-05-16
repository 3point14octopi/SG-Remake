using System;
using System.Collections.Generic;
using JAFprocedural;
using UnityEngine;

public interface RoomTemplate
{
    bool ContainsEnemies { get;}
    bool ContainsTraps { get;}
    bool Cleared { get; }

    void AssignMap(Space2D map);
    void InitEnemyList();
    void RemoveEnemy(int index);
    void AddEnemy(GameObject enemy);

    void OnUpdate();
    void OnEnter();

    int GetPopulation();

}
