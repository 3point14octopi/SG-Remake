using JAFprocedural;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;



public class LevelGenerator : MonoBehaviour
{
    public CurrentDoors doorManager;
    public CamShift camRef;
    public JAFGridLayer[] layers = new JAFGridLayer[3];
    private Space2D room;
    private SG_Level dungeon;
    // Start is called before the first frame update
    void Start()
    {
        dungeon = new SG_Level();

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

    private void PrintFullMap()
    {
        for(int i = 0; i < layers.Length; i++)
        {
            layers[i].Clear();
        }

        for(int y = 0; y < dungeon.megaMap.height; y++)
        {
            for(int x = dungeon.megaMap.width-1; x >= 0; x--)
            {
                int index = dungeon.megaMap.GetCell(x, y);
                if(index >=0 && index < layers.Length)
                {
                    layers[index].Draw(new Vector3Int(x, -y, 0));
                }
            }
        }


        Coord startRoom = new Coord();
        dungeon.minimap.FindFirstInstance(1, out startRoom);
        GameObject.Find("Frostbite").transform.position = new Vector2((startRoom.x * 25) + 12, (startRoom.y * -15) - 8);
        camRef.SetPosition(new Coord(startRoom.x, startRoom.y));

    }

    
}
