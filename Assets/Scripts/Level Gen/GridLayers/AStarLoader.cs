using JAFprocedural;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarLoader : MonoBehaviour
{
    public Vector2Int dimensions;
    public bool printDebug = false;

    Space2D scannedMap;
    Tilemap grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Tilemap>();
        grid.CompressBounds();
        Scan();

        if (printDebug) DebugScannedMap();
    }

    private void Scan()
    {
        scannedMap = new Space2D(dimensions.x, dimensions.y);
        TileBase[] tiles = grid.GetTilesBlock(grid.cellBounds);

        Vector2Int org = CalcOrigin(tiles);
        scannedMap.worldOrigin = new Coord(org.x, org.y);

        for (int i = 0; i < dimensions.y; i++)
        {
            for(int j = 0; j < dimensions.x; j++)
            {
                int cVal = ((tiles[(i * dimensions.x) + j] == null)?1:0);
                scannedMap.SetCellVal(new Coord(j, dimensions.y-i-1), cVal);
            }
        }

        AstarDebugLayer.Instance.SetRoomMap(scannedMap);


        
    }

    Vector2Int CalcOrigin(TileBase[] tiles)
    {
        Vector2Int org = new Vector2Int(Int32.MaxValue, Int32.MinValue);

        BoundsInt.PositionEnumerator b = grid.cellBounds.allPositionsWithin;
        int iteration = 0;
        foreach(var point in grid.cellBounds.allPositionsWithin)
        {
            if(tiles[iteration] != null)
            {
                if (point.x < org.x) org.x = point.x;
                if (point.y > org.y) org.y = point.y;
            }
        }

        return org;
    }

    private void DebugScannedMap()
    {
        string toPrint = "";
        for(int i = 0; i < scannedMap.height; i++)
        {
            for(int j = 0; j < scannedMap.width; j++)
            {
                toPrint += scannedMap.GetCell(j, i).ToString();
            }
            toPrint += '\n';
        }
        Debug.Log(toPrint);
    }
}
