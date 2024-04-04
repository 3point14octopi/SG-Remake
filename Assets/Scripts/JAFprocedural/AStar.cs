using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JAFprocedural
{
    public class Node : IComparable<Node>
    {
        public Coord Position { get; set; }
        public int Cost { get; set; }
        public Node Parent { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int CompareTo(Node other)
        {
            return Cost.CompareTo(other.Cost);
        }
    }

    public class AStarAlgorithm
    {
        public Coord WorldLocToCoord(Vector3 loc, Space2D roomMap)
        {
            return new Coord(Mathf.FloorToInt(loc.x - roomMap.worldOrigin.x - 0.5f), Mathf.FloorToInt(-loc.y - roomMap.worldOrigin.y - 0.5f));
        }


        private int Heuristic(Coord current, Coord goal)
        {
            return Math.Abs(current.x - goal.x) +
            Math.Abs(current.y - goal.y);
        }

        //this will PROBABLY be the source of problems
        private List<Coord> GetNeighbors(Node node, Space2D grid)
        {
            List<Coord> neighbors = new List<Coord>();
            int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
            // up, down, right, left
            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int dx = directions[i, 1];
                int dy = directions[i, 0];
                int x = node.Position.y + dx;
                int y = node.Position.x + dy;
                if (x >= 0 && x < grid.height && y >= 0 && y <
                grid.width)
                {
                    neighbors.Add(new Coord(y, x));
                }
            }
            return neighbors;
        }
        public List<Coord> AStar(Space2D grid, Coord start,
        Coord goal)
        {
            BasicBuilderFunctions.Flood(grid, new Cell(2), new Cell(1000));
            BasicBuilderFunctions.Flood(grid, new Cell(0), new Cell(1000));

            List<Node> openSet = new List<Node>();
            HashSet<Coord> closedSet = new HashSet<Coord>();
            Node startNode = new Node { Position = start, Cost = 0 };
            startNode.G = 0;
            startNode.H = Heuristic(start, goal);
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                Node currentNode = openSet.Min<Node>();
                openSet.Remove(currentNode);
                if (currentNode.Position == goal)
                {
                    List<Coord> path = new List<Coord>();
                    while (currentNode != null)
                    {
                        path.Add(currentNode.Position);
                        currentNode = currentNode.Parent;
                    }
                    path.Reverse();
                    return path;
                }
                foreach (Coord neighborPos in GetNeighbors(currentNode, grid))
                {
                    int Gneighbor = grid.GetCell(neighborPos.x, neighborPos.y);
                    Node neighbor = new Node { Position = neighborPos };
                    int TentativeG = currentNode.G + Gneighbor;
                    if (!closedSet.Contains(neighbor.Position))
                    {
                        if (!openSet.Contains(neighbor))
                        {
                            neighbor.G = TentativeG;
                            neighbor.H = Heuristic(neighborPos, goal);
                            neighbor.Cost = neighbor.G + neighbor.H;
                            neighbor.Parent = currentNode;
                            openSet.Add(neighbor);
                        }
                    }
                    if (closedSet.Contains(neighbor.Position))
                    {
                        if (TentativeG < neighbor.G)
                        {
                            neighbor.G = TentativeG;
                            neighbor.H = Heuristic(neighborPos, goal);
                            neighbor.Cost = neighbor.G + neighbor.H;
                            neighbor.Parent = currentNode;
                            openSet.Add(neighbor);
                            closedSet.Remove(neighbor.Position);
                        }
                    }
                }
                closedSet.Add(currentNode.Position);
            }
            return null;
        }
    }
}
