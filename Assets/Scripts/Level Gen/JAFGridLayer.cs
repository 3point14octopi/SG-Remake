using JAFprocedural;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum TileSelectionStyle
{
    Shuffle, 
    Variant
}



public class JAFGridLayer : MonoBehaviour
{
    public Tilemap rendermap;
    public bool isAnimated = false;
    public List<Tile> staticTiles = new List<Tile>();
    public List<AnimatedTile> animatedTiles = new List<AnimatedTile>();

    public TileSelectionStyle selectionStyle = TileSelectionStyle.Shuffle;


    public void Draw(Vector3Int position, int tileType = -1)
    {
        if (isAnimated)
        {
            int index = (animatedTiles.Count > 1 && selectionStyle == TileSelectionStyle.Shuffle) 
                ? ((RNG.GenRand(0, 10) > 8) ? RNG.GenRand(1, animatedTiles.Count - 1) : 0) 
                : ((tileType > -1 && tileType < animatedTiles.Count)?tileType:0);
            rendermap.SetTile(position, animatedTiles[index]);
        }
        else
        {
            int index = (staticTiles.Count > 1 && selectionStyle == TileSelectionStyle.Shuffle) 
                ? ((RNG.GenRand(0, 10) > 8) ? RNG.GenRand(1, staticTiles.Count - 1) : 0) 
                : ((tileType > -1 && tileType < staticTiles.Count)? tileType: 0);
            rendermap.SetTile(position, staticTiles[index]);
        }

    }

    public void Clear()
    {
        rendermap.ClearAllTiles();
    }

    public void RemoveTile(Vector3Int position)
    {
        rendermap.SetTile(position, null);
    }
}
