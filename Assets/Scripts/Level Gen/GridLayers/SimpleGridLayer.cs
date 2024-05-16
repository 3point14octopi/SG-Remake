using JAFprocedural;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;






public class SimpleGridLayer : JGridLayer
{
    public Tilemap renderGrid;
    public TileSelectionStyle selectionStyle = TileSelectionStyle.Select;
    public List<TileBase> tileList = new List<TileBase>();
    

    public uint tileRange = 10; [Range(0, 10)] 
    public int[] odds = new int[] { 1 };




    int[] indexes;

    public override void Draw(Vector3Int position, int tileType = -1)
    {
        int index = (tileList.Count > 1 && selectionStyle == TileSelectionStyle.Shuffle)
            ? RNGindex()
            : ((tileType > -1 && tileType < tileList.Count) ? tileType : 0);
        renderGrid.SetTile(position, tileList[index]);
    }

    public override void Clear()
    {
        renderGrid.ClearAllTiles();
    }

    public override void RemoveTile(Vector3Int position)
    {
        renderGrid.SetTile(position, null);
    }


    private void Start()
    {
        if(selectionStyle == TileSelectionStyle.Shuffle && tileList.Count > 1)
        {
            //this is not very good 
            indexes = new int[tileRange];
            int total = 0;
            for (int i = 0; i < odds.Length; i++)
            {
                for(int j = 0; j < odds[i]; j++)
                {
                    int x = i;
                    indexes[total] = x;
                    total++;
                }
            }

            int remainder = (int)tileRange - total;
            if (remainder > 0) for (int i = remainder; i > 0; indexes[total] = 0, total++, i--) ;
        }
        
    }
    private int RNGindex()
    {
        return indexes[RNG.GenRand(0, indexes.Length)];
    }

}
