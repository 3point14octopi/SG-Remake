using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySubject : MonoBehaviour
{
    private List<EnemyObserver> observers = new List<EnemyObserver>(); //list of observers, we can also use this to reference how many enemies are alive currently
    public Tilemap doors; //so we can clear the doors

    //called when observers enable, they add themselves to the list
    public void AddObserver(EnemyObserver observer){
        observers.Add(observer);
    }

    //called when observers die
    public void RemoveObserver(EnemyObserver observer){
        observers.Remove(observer);

        //opens the doors after we run out of observers/enemies
        if(observers.Count == 0){
            doors.ClearAllTiles();
        }
    }

    //don't currently use this but it is an observer pattern standard, can be used for something like buffing the last 3 enemies
    protected void NotifyObservers(){
        

    }

    public void IdkMan(int index, Vector3 location)
    {
        observers[index].OnNotify(location);
    }

    public void AlertOfDeath(EnemyObserver observer)
    {
        int index = observers.IndexOf(observer);
        Debug.Log("enemy " + index.ToString() + " has died, lmao");
        //tell the object manager to turn it off
    }
}
