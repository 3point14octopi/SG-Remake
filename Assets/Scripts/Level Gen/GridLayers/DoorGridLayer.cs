using System;
using UnityEngine;
using JAFprocedural;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class DoorGridLayer : JGridLayer
{
    bool flipflop = false;
    public Dictionary<Vector3Int, DoorStates> stateList = new Dictionary<Vector3Int, DoorStates>();
    public Tilemap renderGrid;
    public List<DoorTile> tileList = new List<DoorTile>();
    public List<GameObject> doorObj = new List<GameObject>();
    public List<DoorTileBehaviour> instantiatedDoors = new List<DoorTileBehaviour>();
    public DoorTile exposedTile;

    public static DoorGridLayer Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public override void Clear()
    {
        renderGrid.ClearAllTiles();
    }
    public override void RemoveTile(Vector3Int position)
    {
        renderGrid.SetTile(position, null);
    }
    public override void Refresh()
    {
        renderGrid.RefreshAllTiles();
    }

    public override void Draw(Vector3Int p, int t = -1)
    {
        int index = (t < 0 || t > tileList.Count) ? 0 : t;
        
        renderGrid.SetTile(p, tileList[index]);
        instantiatedDoors.Add((transform.GetChild(transform.childCount - 1)).gameObject.GetComponent<DoorTileBehaviour>());

    }

    

    public void OpenShutDoors(bool open)
    {
        foreach (DoorTileBehaviour door in instantiatedDoors) door.tileState += (open) ? -1 : 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OpenShutDoors(flipflop);
            Refresh();
            flipflop = !flipflop;
            Debug.Log("fwip");
        }
      
    }
}
