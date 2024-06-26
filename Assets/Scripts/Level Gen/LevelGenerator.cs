using JAFprocedural;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class LevelGenerator : MonoBehaviour
{
    public GameObject roomBlob;

    [SerializeField]public JGridLayer[] layers = new JGridLayer[2];
    public int backgroundLayerIndex = 1;
    private SG_Level dungeon;
    // Start is called before the first frame update
    void Start()
    {
        dungeon = new SG_Level(false);

        PrintFullMap();
        AstarDebugLayer.Instance.SetRoomMap(dungeon.megaMap);
    }


    private void PrintRoom(Space2D room)
    {
        for (int y = room.worldOrigin.y; y < room.height + room.worldOrigin.y; y++)
        {
            for (int x = room.worldOrigin.x + room.width - 1; x >= room.worldOrigin.x; x--)
            {
                layers[backgroundLayerIndex].Draw(new Vector3Int(x, -y, 0));

                int index = (room.GetCell(x-room.worldOrigin.x, y-room.worldOrigin.y));
                if((index >= 0 && index < layers.Length) && index != backgroundLayerIndex)
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

            RoomTemplate r = Instantiate(roomBlob, new Vector2((room.Value.worldOrigin.x) + 7.5f, (-room.Value.worldOrigin.y) - 3.5f), Quaternion.identity).GetComponent<RoomTemplate>();

            r.PlaceDoors(room.Value.worldOrigin);
            r.AssignMap(room.Value);
            r.rType = dungeon.minimap.GetCell(room.Key);
        }
        Debug.Log("done room instantiation");

        Coord startRoom = new Coord();
        dungeon.minimap.FindFirstInstance(1, out startRoom);
        GameObject.Find("Frostbite").transform.position = new Vector2((startRoom.x * 15) + 7.5f, (startRoom.y * -9) - 4.5f);
        FollowCam.Instance.ForceJump(new Vector3((startRoom.x * 15) + 7.5f, (startRoom.y * -9) -4.5f, 0));

    }

    
}
