using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JAFprocedural
{
    public class Node : IComparable<Node>
    {
        public Coord m_position;
        public int m_cost;
        public Node m_parent;
        public int m_G;
        public int m_heuristic;

        public int CompareTo(Node other)
        {
            return m_cost.CompareTo(other.m_cost);
        }
    }

    public class AStarCalculator
    {
        private Space2D grid;
        public int wall = 1000;
    
        public AStarCalculator(Space2D map, int valid, int barrier = 1000)
        {
            wall = barrier;
            SetNewGrid(map, valid);
        }
        public void SetNewGrid(Space2D newGrid, int valid)
        {
            grid = new Space2D(newGrid.width, newGrid.height);
            BasicBuilderFunctions.CopySpaceAToB(newGrid, grid, new List<Cell>(){ new Cell(valid)});
            BasicBuilderFunctions.FloodExcluding(grid, new Cell(1), new Cell(wall));
            UnityEngine.Debug.Log("unbelievable");
        }

        //is a little dumb but we can improve that later
        private int Heuristic(Coord current, Coord goal)
        {
            return Math.Abs(current.x - goal.x) + Math.Abs(current.y - goal.y);
        }
        //grabs any tile surrounding a given node that isn't a wall
        private List<Coord> GetNeighbours(Node node)
        {
            List<Coord> neighbours = new List<Coord>();
            int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };// down, up, right, left

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int x = node.m_position.x + directions[i, 0];
                int y = node.m_position.y + directions[i, 1];

                if((x >= 0 && x < grid.width) && (y >= 0 && y < grid.height))
                {
                    if(grid.GetCell(x, y) != wall)
                    {
                        neighbours.Add(new Coord(x, y));
                    }
                }
            }

            return neighbours;
        }

        //the A* algorithm
        public List<Coord> AStar(Coord start, Coord goal)
        {
            if (grid == null)
            {
                UnityEngine.Debug.Log("no");
                return null;
            }
            List<Node> openSet = new List<Node>();
            List<Coord> closedSet = new List<Coord>();
            Node startNode = new Node
                { m_position = start, m_cost = 0, m_G = 0, m_heuristic = Heuristic(start, goal) };
            openSet.Add(startNode);
            int iterations = 0;
            while(openSet.Count > 0 || iterations < 100)
            {
                iterations++;
                Node currentNode = openSet.Min<Node>();
                openSet.Remove(currentNode);

                if (currentNode.m_position.IsEqual(goal))
                {
                    List<Coord> path = new List<Coord>();
                    while(currentNode != null)
                    {
                        path.Add(currentNode.m_position);
                        currentNode = currentNode.m_parent;
                    }

                    path.Reverse();
                    return path;
                }

                foreach(Coord neighbourPos in GetNeighbours(currentNode))
                {
                    int neighbourG = grid.GetCell(neighbourPos);
                    Node neighbour = new Node { m_position = neighbourPos };
                    int tentativeG = currentNode.m_G + neighbourG;

                    if(!LHas(closedSet, neighbour.m_position))
                    {
                        if (!openSet.Contains(neighbour))
                        {
                            neighbour.m_G = tentativeG;
                            neighbour.m_heuristic = Heuristic(neighbourPos, goal);
                            neighbour.m_cost = neighbour.m_G + neighbour.m_heuristic;
                            neighbour.m_parent = currentNode;
                            openSet.Add(neighbour);
                        }

                    }
                    else
                    {
                        if(tentativeG < neighbour.m_G)
                        {
                            neighbour.m_G = tentativeG;
                            neighbour.m_heuristic = Heuristic(neighbourPos, goal);
                            neighbour.m_cost = neighbour.m_G + neighbour.m_heuristic;
                            neighbour.m_parent = currentNode;
                            openSet.Add(neighbour);

                            LRemove(closedSet, neighbour.m_position);
                        }
                    }
                }
                closedSet.Add(currentNode.m_position);
            }
            UnityEngine.Debug.Log("couldn't escape");
            return null;
        }
        

        //helpers because C# is sometimes too smart and wraps back around to being stupid
        private static bool LHas(List<Coord> list, Coord value)
        {
            for(int i = 0; i < list.Count; i++)
            {
                if (list[i].IsEqual(value)) return true;
            }
            return false;
        }
        private static void LRemove(List<Coord> list, Coord toRemove)
        {
            for(int i = 0; i < list.Count; i++)
            {
                if (list[i].IsEqual(toRemove))
                {
                    list.RemoveAt(i);
                    return;
                }
            }
        }

    }
}
