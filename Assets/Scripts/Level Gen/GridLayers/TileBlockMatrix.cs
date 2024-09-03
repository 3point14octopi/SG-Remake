using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable] public struct TileBlockRow
{
    public TileBase[] elements;

    public TileBlockRow(int size = 1)
    {
        elements = new TileBase[size];
    }
}
[Serializable]public class TileBlockMatrix
{
    public int xDimension;
    public int yDimension;
    public TileBlockRow[] rows;

    public TileBlockMatrix(int x = 1, int y = 1)
    {
        rows = new TileBlockRow[y];
        
        for(int i = 0; i < y; i++)
        {
            rows[i] = new TileBlockRow(x);
        }

        xDimension = x;
        yDimension = y;
    }

    public void EditDimensions(Vector2Int dimensions)
    {
        TileBlockRow[] tRows = new TileBlockRow[dimensions.y];
        for (int i = 0; i < dimensions.y; tRows[i] = new TileBlockRow(dimensions.x), i++) ;

        int yIterations = (dimensions.y > yDimension) ? yDimension : dimensions.y;
        int xIterations = (dimensions.x > xDimension) ? xDimension : dimensions.x;

        for(int y = 0; y < yIterations; y++)
        {
            for(int x = 0; x < xIterations; x++)
            {
                tRows[y].elements[x] = rows[y].elements[x];
            }
        }

        rows = tRows;
        xDimension = dimensions.x;
        yDimension = dimensions.y;
    }
    
}