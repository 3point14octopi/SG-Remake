using JAFprocedural;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;



public class LevelGenerator : MonoBehaviour
{
    public CurrentDoors doorManager;
    public CamShift camRef;
    public JAFGridLayer[] layers = new JAFGridLayer[3];
    public int backgroundLayer = 1;
    private Space2D room;
    private SG_Level dungeon;
    [Header("Generation Parameters")]
    public bool useLazyLoading = true;
    public bool lazyLoadInBuild;
    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_EDITOR
        if(lazyLoadInBuild != null && lazyLoadInBuild){
            useLazyLoading = true;
        }else{
            useLazyLoading = false;
        }
#endif
        dungeon = new SG_Level(useLazyLoading);

        Debug.Log(dungeon.rooms.Count);
        doorManager.floorMap = dungeon.minimap;
        foreach(KeyValuePair<Coord, Space2D> kvp in dungeon.rooms)
        {
            ERoomManager.Instance.CreateDataForRoom(dungeon.minimap.GetCell(kvp.Key), kvp.Value, kvp.Key);
        }

        PrintFullMap();
    }

    // Update is called once per frame
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //teleport to the boss room
            Coord location = new Coord();
            if(dungeon.minimap.FindFirstInstance(7, out location))
            {
                GameObject.Find("Frostbite").transform.position = new Vector2((location.x * 25) + 12, (location.y * -15) - 13);
                camRef.SetPosition(location);
            }
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //teleport to the wisp room
            Coord location = new Coord();
            if (dungeon.minimap.FindFirstInstance(3, out location))
            {
                GameObject.Find("Frostbite").transform.position = new Vector2((location.x * 25) + 12, (location.y * -15) - 13);
                camRef.SetPosition(location);
            }
        }
    }
#endif

    private void PrintRoom(Space2D room)
    {
        for (int y = room.worldOrigin.y; y < room.height + room.worldOrigin.y; y++)
        {
            for (int x = room.worldOrigin.x + room.width - 1; x >= room.worldOrigin.x; x--)
            {
                layers[backgroundLayer].Draw(new Vector3Int(x, -y, 0));

                int index = room.GetCell(x-room.worldOrigin.x, y-room.worldOrigin.y);
                if((index >= 0 && index < layers.Length) && index != backgroundLayer)
                {
                    layers[index].Draw(new Vector3Int(x, -y, 0));
                }
            }
        }
    }

    private void PrintFullMap()
    {
        for(int i = 0; i < layers.Length; i++)
        {
            layers[i].Clear();
        }

        foreach(KeyValuePair<Coord, Space2D> room in dungeon.rooms)
        {
            PrintRoom(room.Value);
        }
        

        Coord startRoom = new Coord();
        dungeon.minimap.FindFirstInstance(1, out startRoom);
        GameObject.Find("Frostbite").transform.position = new Vector2((startRoom.x * 25) + 12, (startRoom.y * -15) - 8);
        camRef.SetPosition(new Coord(startRoom.x, startRoom.y));

        Coord bossRoom = new Coord();
        dungeon.minimap.FindFirstInstance(7, out bossRoom);
        camRef.mm.SetBossRoom(bossRoom);

    }

    
}
