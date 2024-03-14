using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySubject : MonoBehaviour
{
    private List<EnemyObserver> observers = new List<EnemyObserver>();
    public Tilemap doors;

     void Update(){
        if(observers.Count == 0){
            Debug.Log("Level Cleared");
            doors.ClearAllTiles();
        }
    }
    public void AddObserver(EnemyObserver observer){
        observers.Add(observer);
        Debug.Log("Added observer");
    }

    public void RemoveObserver(EnemyObserver observer){
        observers.Remove(observer);
        Debug.Log("Deleted observer");
    }

    protected void NotifyObservers(){
        observers.ForEach((observers) => {
            observers.OnNotify(); 
        });

    }
}
