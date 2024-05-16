using JAFprocedural;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileSelectionStyle
{
    Shuffle, 
    Select
}


public abstract class JGridLayer:MonoBehaviour
{
    public abstract void Draw(Vector3Int p, int t = -1);
    public abstract void Clear();
    public abstract void RemoveTile(Vector3Int p);
}
