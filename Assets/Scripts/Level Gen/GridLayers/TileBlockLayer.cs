using JAFprocedural;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum MatrixAnchor
{
    TopLeft,
    BottomLeft
}

public class TileBlockLayer:SimpleGridLayer
{
    public Vector2Int originCoords = new Vector2Int(0,0);
    public MatrixAnchor anchor = MatrixAnchor.TopLeft;
    public TileBlockMatrix matrix = new TileBlockMatrix(2,2);
    public Vector2Int matrixDim = new Vector2Int(2,2);

    public override void Draw(Vector3Int position, int tileType = -1)
    {
        int xValue = Mathf.Abs((position.x - originCoords.x) % matrix.xDimension);
        int yValue = Mathf.Abs((position.y - originCoords.y) % matrix.yDimension);

        if(anchor == MatrixAnchor.BottomLeft)
        {
            yValue = (matrix.yDimension - 1) - yValue;
        }

        renderGrid.SetTile(position, matrix.rows[yValue].elements[xValue]);
    }
}