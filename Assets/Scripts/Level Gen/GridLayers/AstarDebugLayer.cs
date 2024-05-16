using JAFprocedural;
using System.Collections.Generic;
using UnityEngine;

public enum AstarTileTypes
{
    Node,
    Start,
    Goal
}

public class AstarDebugLayer : SimpleGridLayer
{
    public bool enableDebuggingVisualizers = true;

    private AStarCalculator aStar;
    private Coord worldOrigin = new Coord(0, 0);

    public static AstarDebugLayer Instance = new AstarDebugLayer();

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
    }

    private Queue<Vector2> CoordListToQueue(List<Coord> nodes)
    {
        if (enableDebuggingVisualizers) Clear();

        Queue<Vector2> queue = new Queue<Vector2>();
        foreach (Coord node in nodes)
        {
            queue.Enqueue(new Vector2(node.x + worldOrigin.x + 0.5f, -node.y - worldOrigin.y + 0.5f));
            if (enableDebuggingVisualizers) Draw(new Vector3Int(node.x + worldOrigin.x, -node.y - worldOrigin.y, 0), (int)AstarTileTypes.Node);
        }
        if (enableDebuggingVisualizers)
        {
            Draw(new Vector3Int((int)queue.Peek().x, (int)queue.Peek().y, 0), (int)AstarTileTypes.Start);
            Draw(new Vector3Int((int)nodes[nodes.Count - 1].x + worldOrigin.x, -(int)nodes[nodes.Count - 1].y - worldOrigin.y, 0), (int)AstarTileTypes.Goal);
        }
        return queue;
    }

    public Queue<Vector2> AstarPath(Vector3 start, Vector3 goal)
    {
        Queue<Vector2> path = new Queue<Vector2>();
        Vector3Int loc = renderGrid.WorldToCell(start);
        Vector3Int target = renderGrid.WorldToCell(goal);

        List<Coord> nodes = aStar.AStar(new Coord(loc.x - worldOrigin.x, -(loc.y) - worldOrigin.y), new Coord(target.x-worldOrigin.x, -(target.y) - worldOrigin.y), 3500);
        if (nodes != null) path = CoordListToQueue(nodes);

        return path;
    }

}
