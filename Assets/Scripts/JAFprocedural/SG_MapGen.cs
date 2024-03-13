using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;

namespace JAFprocedural
{
    //static builder functions for SG rooms
    public static class SG_MapGen
    {
        public static Space2D MakeFloorplan()
        {
            Space2D floorPlan = new Space2D(5, 5);
            List<Coord> points = new List<Coord>();

            for(int i = 0; i < 3;)
            {
                Coord c = RNG.GenRandCoord(floorPlan, true);
                if (floorPlan.GetCell(c) == 0)
                {
                    floorPlan.SetCell(c, new Cell(i + 1));
                    points.Add(c);
                    i++;
                }
            }

            Space2D cover = new Space2D(5, 5);
            BasicBuilderFunctions.ConnectCoords(cover, points[0], points[1], new Cell(4));
            BasicBuilderFunctions.ConnectCoords(cover, points[0], points[2], new Cell(4));

            for(int i = 0; i < cover.height; i++)
            {
                for(int j = 0; j < cover.width; j++)
                {
                    if(cover.GetCell(j, i) == 4 && floorPlan.GetCell(j, i) == 0)
                    {
                        floorPlan.SetCell(new Coord(j, i), new Cell(4));
                    }
                }
            }


            return floorPlan;
        }

        public static Space2D MakeFloorplan2()
        {
            Space2D floorPlan = new Space2D(6, 4);
            List<Coord> points = new List<Coord>();

            for (int i = 0; i < 4;)
            {
                Coord c = RNG.GenRandCoord(floorPlan, true);
                if (floorPlan.GetCell(c) == 0)
                {
                    floorPlan.SetCell(c, new Cell(i + 1));
                    points.Add(c);
                    i++;
                }
            }

            Space2D cover = new Space2D(6, 4);
            BasicBuilderFunctions.Connect3(cover, points[0], points[1], points[2], new Cell(5));
            BasicBuilderFunctions.ConnectCoords(cover, points[2], points[3], new Cell(5));

            for (int i = 0; i < cover.height; i++)
            {
                for (int j = 0; j < cover.width; j++)
                {
                    if (cover.GetCell(j, i) == 5 && floorPlan.GetCell(j, i) == 0)
                    {
                        floorPlan.SetCell(new Coord(j, i), new Cell(5));
                    }
                }
            }


            return floorPlan;
        }

        public static Space2D MakeFloorplan3()
        {
            Space2D floorPlan = new Space2D(6, 4);
            List<Coord> points = new List<Coord>();

            for (int i = 0; i < 4;)
            {
                Coord c = new Coord();
                if(i == 0)
                {
                    c = new Coord(RNG.GenRand(2, 2), RNG.GenRand(1, 2));
                }
                else
                {
                    c = RNG.GenRandCoord(new Space2D(2, 4), true);
                    if (RNG.GenRand(0, 3) == 1) c.x += 4;
                }
                
                if (floorPlan.GetCell(c) == 0)
                {
                    floorPlan.SetCell(c, new Cell(i + 1));
                    points.Add(c);
                    i++;
                }
            }

            Space2D cover = new Space2D(6, 4);
            BasicBuilderFunctions.ConnectCoords(cover, points[0], points[1], new Cell(5));
            BasicBuilderFunctions.ConnectCoords(cover, points[0], points[2], new Cell(5));
            BasicBuilderFunctions.ConnectCoords(cover, points[0], points[3], new Cell(5));

            int extraCount = 0;

            for (int i = 0; i < cover.height; i++)
            {
                for (int j = 0; j < cover.width; j++)
                {
                    if (cover.GetCell(j, i) == 5 && floorPlan.GetCell(j, i) == 0)
                    {
                        floorPlan.SetCell(new Coord(j, i), new Cell(5));
                        extraCount++;
                    }
                }
            }

            while(extraCount < 3)
            {
                Coord addition = RNG.GenRandCoord(floorPlan, true);
                if(floorPlan.GetCell(addition) == 0)
                {
                    Coord test = BasicBuilderFunctions.CheckAdjacentCells(floorPlan, addition);
                    {
                        if((test.x + test.y) != 4)
                        {
                            floorPlan.SetCell(addition, new Cell(6));
                            extraCount++;
                        }
                        else
                        {
                            UnityEngine.Debug.Log("man");
                        }
                    }
                }
            }

            return floorPlan;
        }








        public static Space2D MakeRoom1()
        {
            Space2D room = new Space2D(25, 15);
            room = SetDefaultDoors(room);

            room = DownwardSpill(room, new Coord(12, 1));
            room = DownwardSpill(room, new Coord(12, 13), false);

            if(BasicBuilderFunctions.PercentageOf(room, new Cell(1)) > 0.55f)
            {
                room = LittleTimmy(room);
            }
            room = EnsureConnected(room);

            return room;
        }
        public static Space2D MakeRoom2()
        {
            Space2D room = new Space2D(25, 15);

            room = SetDefaultDoors(room);
            room = LittleJimmy(room);

            return room;
        }


        public static Space2D SetDefaultDoors(Space2D room)
        {
            Cell opening = new Cell(1);
            room.SetCell(new Coord(0, 7), opening);
            room.SetCell(new Coord(1, 7), opening);
            room.SetCell(new Coord(12, 0), opening);
            room.SetCell(new Coord(12, 1), opening);
            room.SetCell(new Coord(24, 7), opening);
            room.SetCell(new Coord(23, 7), opening);
            room.SetCell(new Coord(12, 13), opening);
            room.SetCell(new Coord(12, 14), opening);

            return room;
        }

        public static int DetermineChokeValue(int iterationNum)
        {
            if (iterationNum == 1) return 0;
            return (RNG.GenRand(1, 2) - 1);
        }
        //what if i hated myself
        public static Space2D DownwardSpill(Space2D room, Coord start, bool downward = true, int iteration = 0, Cell fill = null)
        {
            fill = (fill == null) ? new Cell(1) : fill;
            room.SetCell(start, fill);
            
            List<Coord> leftPoints = new List<Coord>();
            List<Coord> rightPoints = new List<Coord>();

            iteration++;
            int leftStride = BasicBuilderFunctions.CalculateStride(start, new Coord(0, start.y), true) - 2;
            int rightStride = BasicBuilderFunctions.CalculateStride(new Coord(room.width-1, start.y), start, true) - 2;

            //left
            int lLimit = (leftStride < 7) ? leftStride : 7;
            Coord leftTarget = new Coord();
            if (lLimit > 0)
            {
                if(lLimit > 1)
                {
                    leftTarget = new Coord(start.x - RNG.GenRand(1, lLimit), start.y);
                }
                else
                {
                    leftTarget = new Coord(start.x - 1, start.y);
                }
                
                for(int i = start.x-1; i >= leftTarget.x; i--)
                {
                    leftPoints.Add(new Coord(i, start.y));
                    room.SetCell(leftPoints.Last<Coord>(), fill);
                }
            }
            int newLeftStart = (leftPoints.Count <= DetermineChokeValue(iteration)) ? -1:RNG.GenRand(0, leftPoints.Count + 1);


            int rLimit = (rightStride < 7) ? rightStride : 7;
            Coord rightTarget = new Coord();
            if (rLimit > 0)
            {
                if (rLimit > 1)
                {
                    rightTarget = new Coord(start.x + RNG.GenRand(1, rLimit), start.y);
                }
                else
                {
                    rightTarget = new Coord(start.x + 1, start.y);
                }

                for (int i = start.x + 1; i <= rightTarget.x; i++)
                {
                    rightPoints.Add(new Coord(i, start.y));
                    room.SetCell(rightPoints.Last<Coord>(), fill);
                }
            }
            int newRightStart = (rightPoints.Count <= DetermineChokeValue(iteration))?-1:RNG.GenRand(0, rightPoints.Count + 1);
            

            if(iteration <= 2 || RNG.GenRand(1, 4) > 1)
            {
                if (newLeftStart > -1 && iteration < 8)
                {
                    Coord lC = new Coord(leftPoints[newLeftStart].x, leftPoints[newLeftStart].y + ((downward) ? 1 : -1));
                    room = DownwardSpill(room, lC, downward, iteration, fill);
                }
            }
            
            if(iteration <= 2 || RNG.GenRand(1, 4) > 1)
            {
                if (newRightStart > -1 && iteration < 8)
                {
                    Coord rC = new Coord(rightPoints[newRightStart].x, rightPoints[newRightStart].y + ((downward)?1:-1));
                    room = DownwardSpill(room, rC, downward, iteration, fill);
                }
            }
            

            return room;
        }

        //a* is for goddamn cowards
        public static Space2D EnsureConnected(Space2D room)
        {
            Cell fill = new Cell(1);
            Coord reunion;
            do { reunion = new Coord(RNG.GenRand(2, 20), RNG.GenRand(2, 10)); } while (room.GetCell(reunion) != 1);

            //can override this if you start being SMARTER about door locations
            BasicBuilderFunctions.ConnectCoords(room, reunion, new Coord(12, 1), fill);
            BasicBuilderFunctions.ConnectCoords(room, reunion, new Coord(12, 13), fill);
            BasicBuilderFunctions.ConnectCoords(room, reunion, new Coord(1, 8), fill);
            BasicBuilderFunctions.ConnectCoords(room, reunion, new Coord(23, 8), fill);

            return room;
        }
        
        public static Space2D LittleTimmy(Space2D room)
        {
            UnityEngine.Debug.Log("little timmy to the rescue!");
            int timmyPlaced = 0;
            Cell bingChillin = new Cell(0);

            for(int i = 0; i < 20 && timmyPlaced < 10; i++)
            {
                Coord c = new Coord(RNG.GenRand(2, 20), RNG.GenRand(2, 10));
                if(room.GetCell(c) == 1)
                {
                    Coord tV = BasicBuilderFunctions.CheckAdjacentCells(room, c, true, new Cell(0));
                    if(tV.x != 0 && tV.y  != 0)
                    {
                        room.SetCell(c, bingChillin);
                        i++;
                        timmyPlaced++;
                    }
                }
            }


            UnityEngine.Debug.Log("little timmy placed " + timmyPlaced.ToString());
            return room;
        }

        public static Space2D LittleJimmy(Space2D room)
        {
            List<Coord> points = new List<Coord>();
            //can override this if you start being SMARTER about door locations
            points.Add(new Coord(1, 7));
            points.Add(new Coord(12, 1));
            points.Add(new Coord(23, 7));
            points.Add(new Coord(12, 13));


            int iterations = 10 - points.Count;
            for (int i = 0; i < iterations;)
            {
                Coord c = RNG.GenRandCoord(room);
                if(room.GetCell(c) == 0)
                {
                    room.SetCell(c, new Cell(1));
                    points.Add(c);
                    i++;
                }
            }


            for (iterations = 0, RNG.CircleSelect(points, 0); 
                iterations < points.Count - 1; 
                BasicBuilderFunctions.Connect3(room, points[iterations], RNG.GenRandCoord(room), RNG.CircleSelect(points, iterations + 1), new Cell(1)), 
                iterations++) ;

            return room;
        }

    }

   
}
