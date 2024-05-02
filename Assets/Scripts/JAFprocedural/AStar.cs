using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JAFprocedural
{
    public struct AScoord
    {
        public int x;
        public int y;
        public int z;
    }

    public class Node : IComparable<Node>
    {
        public AScoord m_position;
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
        }

        //Average of hypotenuse (i don't think it's actually possible, we don't have perfect diagonals)
        //and the (x + y) dist
        //x+y tends to run slower? i think? slight undersells are more helpful when doing little jimmy than pathfinding
        private int Heuristic(AScoord current, AScoord goal)
        {
            int a = (int)(Math.Pow(current.x - goal.x, 2f));
            int b = (int)(Math.Pow(current.y - goal.y, 2f));
            int hyp = (int)Math.Sqrt(a + b);

            int sum = Math.Abs(current.x - goal.x) + Math.Abs(current.y - goal.y);

            return (int)(hyp + sum) / 2;
        }
        //grabs any tile surrounding a given node that isn't a wall
        private List<AScoord> GetNeighbours(Node node)
        {
            List<AScoord> neighbours = new List<AScoord>();
            int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };// down, up, right, left

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int x = node.m_position.x + directions[i, 0];
                int y = node.m_position.y + directions[i, 1];

                if((x >= 0 && x < grid.width) && (y >= 0 && y < grid.height))
                {
                    if(grid.GetCell(x, y) != wall)
                    {
                        neighbours.Add(new AScoord { x = x, y = y});//REALLY bad code btw
                    }
                }
            }

            return neighbours;
        }

        //the A* algorithm
        public List<Coord> AStar(Coord start, Coord goal)
        {
            AScoord start_ = new AScoord { x = start.x, y = start.y };
            AScoord goal_ = new AScoord { x = goal.x, y = goal.y };

            if (grid == null)
            {
                UnityEngine.Debug.Log("no");
                return null;
            }
            List<Node> openSet = new List<Node>();
            HashSet<AScoord> closedSet = new HashSet<AScoord>();
            Node startNode = new Node
                { m_position = start_, m_cost = 0, m_G = 0, m_heuristic = Heuristic(start_, goal_) };
            openSet.Add(startNode);
            int iterations = 0;
            while(openSet.Count > 0 && iterations < 20000)
            {
                iterations++;
                Node currentNode = openSet.Min<Node>();
                openSet.Remove(currentNode);

                if (currentNode.m_position.x == goal_.x && currentNode.m_position.y == goal_.y)
                {
                    List<Coord> path = new List<Coord>();
                    while(currentNode != null)
                    {
                        path.Add(new Coord(currentNode.m_position.x, currentNode.m_position.y));
                        currentNode = currentNode.m_parent;
                    }

                    path.Reverse();
                    return path;
                }

                foreach (AScoord neighbourPos in GetNeighbours(currentNode))
                {
                    int neighbourG = grid.GetCell(neighbourPos.x, neighbourPos.y);
                    Node neighbour = new Node { m_position = neighbourPos };
                    int tentativeG = currentNode.m_G + neighbourG;

                    if(!closedSet.Contains(neighbour.m_position))
                    {
                        if (!openSet.Contains(neighbour))
                        {
                            neighbour.m_G = tentativeG;
                            neighbour.m_heuristic = Heuristic(neighbourPos, goal_);
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
                            neighbour.m_heuristic = Heuristic(neighbourPos, goal_);
                            neighbour.m_cost = neighbour.m_G + neighbour.m_heuristic;
                            neighbour.m_parent = currentNode;
                            openSet.Add(neighbour);

                            closedSet.Remove(neighbour.m_position);
                        }
                    }
                }
                closedSet.Add(currentNode.m_position);
            }
            UnityEngine.Debug.Log("no path found");
            return null;
        }
        

    }
}
