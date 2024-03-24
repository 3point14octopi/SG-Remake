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
    void Update()
    {

    }

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

        for(int i = 0; i < dungeon.minimap.height; i++)
        {
            for(int j = 0; j < dungeon.minimap.width; j++)
            {
                if(dungeon.minimap.GetCell(j, i) == 1)
                {
                    camRef.SetPosition(new Coord(j, i));
                    GameObject.Find("Frostbite").transform.position = new Vector2((j * 25) + 12, (i * -15) - 8);
                }
            }
        }
    }

    
}
