using JAFprocedural;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JAFGridLayer : MonoBehaviour
{
    public Tilemap rendermap;
    public bool isAnimated = false;
    public List<Tile> staticTiles = new List<Tile>();
    public List<AnimatedTile> animatedTiles = new List<AnimatedTile>();

    public void Draw(Vector3Int position)
    {
        if (isAnimated)
        {
            int index = (animatedTiles.Count > 1) ? ((RNG.GenRand(0, 10) > 8) ? RNG.GenRand(1, animatedTiles.Count - 1) : 0) : 0;
            if (index > 0) Debug.Log(index.ToString());
            rendermap.SetTile(position, animatedTiles[index]);
        }
        else
        {
            int index = (staticTiles.Count > 1) ? ((RNG.GenRand(0, 10) > 6) ? RNG.GenRand(1, staticTiles.Count - 1) : 0) : 0;
            if (index > 0) Debug.Log(index.ToString());
            rendermap.SetTile(position, staticTiles[index]);
        }

    }

    public void Clear()
    {
        rendermap.ClearAllTiles();
    }
}
