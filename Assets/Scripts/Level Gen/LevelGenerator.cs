using JAFprocedural;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;



public class LevelGenerator : MonoBehaviour
{
    public JAFGridLayer[] layers = new JAFGridLayer[3];
   
    private Space2D room;
    // Start is called before the first frame update
    void Start()
    { 
        room = SG_MapGen.MakeMasterFloor(room);
        PrintRoom();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space)){
            room = SG_MapGen.MakeMasterFloor(room);
            PrintRoom();
        }
#endif
    }

    private void PrintRoom()
    {
        for(int i = 0; i < layers.Length; i++)
        {
            layers[i].Clear();
        }

        for(int y = 0; y < room.height; y++)
        {
            for(int x = room.width-1; x >= 0; x--)
            {
                int index = room.GetCell(x, y);
                if(index >=0 && index < layers.Length)
                {
                    layers[index].Draw(new Vector3Int(room.width - x - 1, -y, 0));
                }
            }
        }
    }
}
