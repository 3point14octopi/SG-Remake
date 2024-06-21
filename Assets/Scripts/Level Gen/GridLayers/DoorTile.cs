using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum DoorStates
{
    OPEN,
    ENEMY_CLOSED, 
    LOCKED,
    ENEMY_LOCKED
}

[Serializable] public struct DoorTileElement
{
    public Sprite tileSprite;
    public Tile.ColliderType tileCollider;
    
    public DoorTileElement(Sprite s, Tile.ColliderType t = Tile.ColliderType.None)
    {
        tileSprite = s;
        tileCollider = t;
    }
}


[CreateAssetMenu(fileName = "DoorTile_", menuName = "2D/Tiles/Door Tile")]
[Serializable]public class DoorTile:Tile 
{
    public DoorStates currentState = DoorStates.OPEN;
    public DoorTileElement[] elements = new DoorTileElement[4];

    public void Set(DoorStates state)
    {
        currentState = state;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        int index;
        if(DoorGridLayer.Instance == null)
        {
            index = 0;
        }
        else
        {
            index = (DoorGridLayer.Instance.stateList.ContainsKey(position)) ? (int)DoorGridLayer.Instance.stateList[position] : 0;
        }
        tileData.sprite = elements[index].tileSprite;
        tileData.colliderType = elements[index].tileCollider;
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (DoorGridLayer.Instance == null) return false;
        go.transform.position = new Vector3(position.x + 0.5f, position.y + 0.5f, position.z);
        go.GetComponent<DoorTileBehaviour>().tileLocation = position;
        
        return base.StartUp(position, tilemap, go);
    }

}

