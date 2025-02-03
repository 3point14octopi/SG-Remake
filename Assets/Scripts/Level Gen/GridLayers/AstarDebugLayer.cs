using JAFprocedural;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum AstarTileTypes
{
    Node,
    Start,
    Goal
}

public class AstarDebugLayer : SimpleGridLayer
{
    public ETSelect ets;
    private bool finder;
    private Vector2 foundV;

    public bool enableDebuggingVisualizers = true;
    private AStarCalculator aStar;
    private Coord worldOrigin = new Coord(0, 0);
    private Space2D myMap;

    public static AstarDebugLayer Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetRoomMap(Space2D map)
    {
        if (aStar == null)
        {
            aStar = new AStarCalculator(map, 1);
        }
        else
        {
            aStar.SetNewGrid(map, 1);
        }
        worldOrigin = map.worldOrigin;
        SetNewGrid(map);
    }

    private Queue<Vector2> CoordListToQueue(List<Coord> nodes, Vector3Int start)
    {
        if (enableDebuggingVisualizers) Clear();
        
        Queue<Vector2> queue = new Queue<Vector2>();
        for(int i = 0; i < nodes.Count; i++)
        {
            Coord node = nodes[i];
            queue.Enqueue(new Vector2(node.x + worldOrigin.x + 0.5f, -node.y - worldOrigin.y + 0.5f));
            if (enableDebuggingVisualizers) Draw(new Vector3Int(node.x + worldOrigin.x, -node.y - worldOrigin.y, 0), (int)AstarTileTypes.Node);
        }
        if (enableDebuggingVisualizers)
        {
            Draw(new Vector3Int((int)queue.Peek().x, (int)(queue.Peek().y-0.5f), 0), (int)AstarTileTypes.Start);
            Draw(new Vector3Int((int)(nodes[nodes.Count - 1].x + worldOrigin.x + 0.5f), -(int)(nodes[nodes.Count - 1].y - worldOrigin.y+0.5f), 0), (int)AstarTileTypes.Goal);
        }
        return queue;
    }

    public Queue<Vector2> AstarPath(Vector3 start, Vector3 goal)
    {
        Queue<Vector2> path = new Queue<Vector2>();
        Vector3Int loc = renderGrid.WorldToCell(start);
        Vector3Int target = renderGrid.WorldToCell(goal);

        List<Coord> nodes = aStar.AStar(new Coord(loc.x - worldOrigin.x, -(loc.y) - worldOrigin.y), new Coord(target.x-worldOrigin.x, -(target.y) - worldOrigin.y), 3500);
        if (nodes != null) path = CoordListToQueue(nodes, loc);

        return path;
    }
     

    public void ToggleSomethingAwful(bool on = true, Space2D room = null)
    {
        if(on && enableDebuggingVisualizers)
        {
            for (int y = 0; y < room.height; y++)
            {
                for (int x = room.worldOrigin.x + room.width - 1; x >= room.worldOrigin.x; x--)
                {
                    int index = (room.GetCell(x - room.worldOrigin.x, y));
                    if (index == 1)
                    {
                        Draw(new Vector3Int(x, y, 0), (int)AstarTileTypes.Node);
                        Debug.Log("bloop");
                    }
                    
                }
            }
            
        }else if (enableDebuggingVisualizers)
        {
            Clear();
        } 
    }

    public IEnumerator FindUnoccupiedTile()
    {
        foundV = new Vector2();
        for(bool found = false; !found; ets.gameObject.SetActive(false))
        {
            Coord loc = RNG.GenRandCoord(myMap);
            if(myMap.GetCell(loc) == 1)
            {
                ets.gameObject.SetActive(true);
                ets.SendTo(new Vector2(loc.x + worldOrigin.x + 0.5f, worldOrigin.y - loc.y + 0.5f));
                yield return new WaitForFixedUpdate();

                if (!ets.activated) { 
                    foundV = new Vector2 (loc.x + worldOrigin.x + 0.5f, worldOrigin.y - loc.y + 0.5f);
                    found = true;
                }
                else
                {
                    myMap.SetCellVal(loc, 99);
                }
            }
            yield return null;
        } 

        BasicBuilderFunctions.Flood(myMap, new Cell(99), new Cell(1));
    }

    public Vector2 LastLocatedUnoccupied()
    {
        return (foundV);

    }













    private void SetNewGrid(Space2D newGrid)
    {
        Coord worldOriginPreserve = new Coord(newGrid.worldOrigin.x, newGrid.worldOrigin.y);
        newGrid.worldOrigin = new Coord();
        myMap = new Space2D(newGrid.width, newGrid.height);
        BasicBuilderFunctions.CopySpaceAToB(newGrid, myMap, new List<Cell>() {});
        newGrid.worldOrigin = worldOriginPreserve;
    }
}
