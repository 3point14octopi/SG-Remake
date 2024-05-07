using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace JAFprocedural
{
    public class SG_Level
    {
        public Space2D minimap;
        public Space2D megaMap;
        public Dictionary<Coord, Space2D> rooms = new Dictionary<Coord, Space2D>();

        public SG_Level(bool lazyLoad = true)
        {
            minimap = SG_MapGen.MakeFloorplan(lazyLoad);

            megaMap = new Space2D(150, 60);

            for(int i = 0; i < minimap.height; i++)
            {
                for(int j = 0; j < minimap.width; j++)
                {
                    if(minimap.GetCell(j, i) > 0)
                    {
                        Coord location = new Coord(j, i);
                        Space2D room = new Space2D(25, 15);
                        room = SG_MapGen.SetDoors(room, location, minimap);

                        if(minimap.GetCell(j, i) == 1)
                        {
                            room = SG_MapGen.MakeStartingRoom(room);
                        }
                        else
                        {
                            room = (minimap.GetCell(j, i) < 5) ? SG_MapGen.MakeRoom1(room) : 
                                (minimap.GetCell(j, i) == 7) ? SG_MapGen.MakeBossRoom(room) : SG_MapGen.MakeRoom2(room);
                        }
                        room.worldOrigin = new Coord(j * 25, i * 15);
                        room = SG_MapGen.DetectPerimeter(room);

                        rooms.Add(location, room);
                        BasicBuilderFunctions.CopySpaceAToB(room, megaMap, new List<Cell>());
                    }
                }
            }
        }
    }




    //static builder functions for SG rooms
    public static class SG_MapGen
    {
        private static bool lazyLevel = true;
        private static AStarCalculator astar = new AStarCalculator(new Space2D(), 1);
        public static Space2D MakeFloorplan(bool lazy = true)
        {
            lazyLevel = lazy;
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
                    if (RNG.GenRand(0, 2) == 1) c.x += 4;
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

            //places the connector rooms without overwriting the old ones
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

            //ensures that there are at least 7 rooms
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

            //places the boss room
            for(bool bossPlaced = false; !bossPlaced;)
            {
                Coord bossRoom = RNG.GenRandCoord(floorPlan, true);

                if(floorPlan.GetCell(bossRoom) == 0)
                {
                    Coord surrounding = BasicBuilderFunctions.CheckAdjacentCells(floorPlan, bossRoom);

                    if(surrounding.x + surrounding.y != 4)
                    {
                        floorPlan.SetCell(bossRoom, new Cell(7));
                        bossPlaced = true;
                    }
                }
            }

            return floorPlan;
        }



        public static Space2D MakeRoom1(Space2D room)
        {
            room = DownwardSpill(room, new Coord(12, 1));
            room = DownwardSpill(room, new Coord(12, 13), false);

            if(BasicBuilderFunctions.PercentageOf(room, new Cell(1)) > 0.55f)
            {
                room = LittleTimmy(room);
            }
            room = EnsureConnected(room);

            return room;
        }

        public static Space2D MakeRoom2(Space2D room)
        {
            if (lazyLevel)
            {
                room = LittleJimmy(room);
            }
            else
            {
                room = (RNG.GenRand(0, 2) == 1) ? LittleJimmy(room) : NewJimmy(room);
            }

            return room;
        }

        public static Space2D MakeStartingRoom(Space2D room)
        {
            BasicBuilderFunctions.Flood(room, new Cell(0), new Cell(1), 1, 1, room.width - 1, room.height - 1);
            BasicBuilderFunctions.Flood(room, new Cell(1), new Cell(0), 13, 0, 24, 7);

            room.SetCell(new Coord(13, 13), new Cell(0));
            BasicBuilderFunctions.HorizontalPath(room, new Cell(0), new Coord(13, 13), BasicBuilderFunctions.CalculateStride(new Coord(24, 13), new Coord(13, 13), true));
            room.SetCell(new Coord(23, 8), new Cell(0));
            BasicBuilderFunctions.VerticalPath(room, new Cell(0), new Coord(23, 8), BasicBuilderFunctions.CalculateStride(new Coord(23, 13), new Coord(23, 8), false));

            room.SetCell(new Coord(11, 13), new Cell(0));
            BasicBuilderFunctions.HorizontalPath(room, new Cell(0), new Coord(11, 13), BasicBuilderFunctions.CalculateStride(new Coord(0, 13), new Coord(11, 13), true));
            room.SetCell(new Coord(1, 8), new Cell(0));
            BasicBuilderFunctions.VerticalPath(room, new Cell(0), new Coord(1, 8), BasicBuilderFunctions.CalculateStride(new Coord(1, 13), new Coord(1, 8), false));

            BasicBuilderFunctions.Flood(room, new Cell(1), new Cell(0), 1, 1, 12, 3);
            BasicBuilderFunctions.HorizontalPath(room, new Cell(0), new Coord(8, 3), BasicBuilderFunctions.CalculateStride(new Coord(0, 3), new Coord(8, 3), true));
            BasicBuilderFunctions.HorizontalPath(room, new Cell(0), new Coord(6, 4), BasicBuilderFunctions.CalculateStride(new Coord(0, 4), new Coord(6, 4), true));
            BasicBuilderFunctions.HorizontalPath(room, new Cell(0), new Coord(4, 5), BasicBuilderFunctions.CalculateStride(new Coord(0, 5), new Coord(4, 5), true));
            room.SetCell(new Coord(1, 6), new Cell(0));

            if (room.GetCell(12, 14) == 0) room.SetCell(new Coord(12, 13), new Cell(0));

            room = DetectPerimeter(room);

            return room;
        }

        public static Space2D MakeBossRoom(Space2D room)
        {
            BasicBuilderFunctions.Flood(room, new Cell(0), new Cell(1), 1, 1, room.width-1, room.height-1);
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

        public static Space2D SetDoors(Space2D room, Coord roomLoc, Space2D layoutRef)
        {
            Cell opening = new Cell(1);

            //up
            if (roomLoc.y > 0 && layoutRef.GetCell(new Coord(roomLoc.x, roomLoc.y - 1)) != 0)
            {
                room.SetCell(new Coord(12, 0), opening);
                room.SetCell(new Coord(12, 1), opening);
            }

            //down
            if (roomLoc.y < layoutRef.height-1 && layoutRef.GetCell(new Coord(roomLoc.x, roomLoc.y+1)) != 0)
            {
                room.SetCell(new Coord(12, 13), opening);
                room.SetCell(new Coord(12, 14), opening);
            }

            //left
            if (roomLoc.x > 0 && layoutRef.GetCell(new Coord(roomLoc.x-1, roomLoc.y)) != 0)
            {
                room.SetCell(new Coord(0, 7), opening);
                room.SetCell(new Coord(1, 7), opening);
            }

            //right
            if (roomLoc.x < layoutRef.width-1 && layoutRef.GetCell(new Coord(roomLoc.x + 1, roomLoc.y)) != 0)
            {
                room.SetCell(new Coord(24, 7), opening);
                room.SetCell(new Coord(23, 7), opening);
            }

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
            int newLeftStart = (leftPoints.Count <= DetermineChokeValue(iteration)) ? -1:RNG.GenRand(0, leftPoints.Count);


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
            int newRightStart = (rightPoints.Count <= DetermineChokeValue(iteration))?-1:RNG.GenRand(0, rightPoints.Count);
            

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

        //turns out i'm a coward who uses A* and this function should be fixed
        public static Space2D EnsureConnected(Space2D room)
        {
            Cell fill = new Cell(1);
            Coord reunion;
            do { reunion = new Coord(RNG.GenRand(2, 20), RNG.GenRand(2, 10)); } while (room.GetCell(reunion) != 1);

            BasicBuilderFunctions.ConnectCoords(room, reunion, new Coord(12, 1), fill);
            BasicBuilderFunctions.ConnectCoords(room, reunion, new Coord(12, 13), fill);
            BasicBuilderFunctions.ConnectCoords(room, reunion, new Coord(1, 7), fill);
            BasicBuilderFunctions.ConnectCoords(room, reunion, new Coord(23, 7), fill);

            return room;
        }
        
        public static Space2D LittleTimmy(Space2D room)
        {
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


            return room;
        }

        public static Space2D LittleJimmy(Space2D room)
        {
            List<Coord> points = new List<Coord>();
            //can override this if you start being SMARTER about door locations
            if (room.GetCell(0, 7) != 0)  points.Add(new Coord(1, 7));
            if (room.GetCell(12, 0) != 0) points.Add(new Coord(12, 1));
            if (room.GetCell(12, 14) != 0)points.Add(new Coord(12, 13));
            if (room.GetCell(24, 7) != 0) points.Add(new Coord(23, 7));


            int iterations = RNG.GenRand(1, 10 - points.Count);
            for (int i = 0; i < iterations;)
            {
                Coord c = RNG.GenRandCoord(room);
                if(room.GetCell(c) != 0)
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

            BasicBuilderFunctions.Flood(room, new Cell(0), new Cell(2), 1, 1, room.width - 1, room.height - 1);
            room = UnderLine(room, 2, new Cell(1), new Cell(3));
            BasicBuilderFunctions.Flood(room, new Cell(2), new Cell(0));

            return room;
        }

        private static Space2D Clump(Space2D room, Coord start, Cell val)
        {
            room.SetCell(start, val);
            room.SetCell(new Coord(start.x + 1, start.y), val);
            room.SetCell(new Coord(start.x, start.y + 1), val);
            room.SetCell(new Coord(start.x + 1, start.y + 1), val);

            return room;
        }
        private static Space2D UnderLine(Space2D room, int valid, Cell toFind, Cell underlineVal, bool seamless = true)
        {
            for (int i = 0; i < room.height; i++)
            {
                for (int j = 0; j < room.width; j++)
                {
                    if (room.GetCell(j, i) == valid)
                    {
                        Coord data = BasicBuilderFunctions.CheckAdjacentCells(room, new Coord(j, i), true, toFind);
                        if (data.x == -1 || data.y == -1) room.SetCell(new Coord(j, i), underlineVal);
                    }
                }
            }

            if (seamless)BasicBuilderFunctions.Flood(room, underlineVal, toFind);

            return room;
        }
        public static Space2D NewJimmy(Space2D room)
        {
            List<Coord> points = new List<Coord>();
            //can override this if you start being SMARTER about door locations
            if (room.GetCell(0, 7) != 0) points.Add(new Coord(1, 7));
            if (room.GetCell(12, 0) != 0) points.Add(new Coord(12, 1));
            if (room.GetCell(12, 14) != 0) points.Add(new Coord(12, 13));
            if (room.GetCell(24, 7) != 0) points.Add(new Coord(23, 7));

            //fill room with valid space
            BasicBuilderFunctions.Flood(room, new Cell(0), new Cell(1), 1, 1, room.width - 1, room.height - 1);

            //"mine" room a bit so that there's less to sample and it maybe runs faster
            int mines = RNG.GenRand(3, 3);
            for(int i = 0; i < mines;)
            {
                Coord loc = new Coord(RNG.GenRand(2, room.width - 4), RNG.GenRand(2, room.height - 4));
                if(room.GetCell(loc) != 0)
                {
                    room = Clump(room, loc, new Cell(0));
                    i++;
                }
            }

            //send room to the A* calculator
            astar.SetNewGrid(room, 1);

            int iterations = RNG.GenRand(5-points.Count, 7-points.Count);
            UnityEngine.Debug.Log("chose " + iterations.ToString() + " points, " + points.Count.ToString() + " locations total");

            for (int i = 0; i < iterations;)
            {
                Coord c = RNG.GenRandCoord(room);

                if (room.GetCell(c) == 1)
                {
                    room.SetCell(c, new Cell(2));
                    points.Add(c);
                    i++;
                }
            }
            UnityEngine.Debug.Log("placed all points");

            for(iterations = 0, RNG.CircleSelect(points, 0); iterations < points.Count-1; iterations++)
            {
                Coord start = points[iterations];
                Coord end = RNG.CircleSelect(points, iterations + 1);
                UnityEngine.Debug.Log("creating a path between " + start.x.ToString() + ',' + start.y.ToString() + " and " + end.x.ToString() + ',' + end.y.ToString());
                List<Coord> path = astar.AStar(points[iterations], RNG.CircleSelect(points, iterations + 1));
                if (path != null)
                {
                    foreach (Coord node in path) room.SetCell(node, new Cell(2));
                }
                else
                {
                    UnityEngine.Debug.Log("had to be boring");
                    BasicBuilderFunctions.ConnectCoords(room, points[iterations], points[iterations + 1], new Cell(2));
                }
            }

            BasicBuilderFunctions.Flood(room, new Cell(0), new Cell(1), 1, 1, room.width - 1, room.height - 1);
            room = UnderLine(room, 1, new Cell(2), new Cell(3));
            BasicBuilderFunctions.Flood(room, new Cell(1), new Cell(0), 1, 1, room.width-1, room.height-1);
            BasicBuilderFunctions.Flood(room, new Cell(2), new Cell(1));


            return room;
        }
        public static Space2D DetectPerimeter(Space2D room)
        {
            for(int i = 0; i < room.height; i++)
            {
                for(int j = 0; j < room.width; j++)
                {
                    if(room.GetCell(j, i) == 0)
                    {
                        Coord data = BasicBuilderFunctions.CheckAdjacentCells(room, new Coord(j, i), true, new Cell(1));
                        if (data.x + data.y != 4)
                        {
                            room.SetCell(new Coord(j, i), new Cell(2));
                        }
                        else
                        {
                            data = BasicBuilderFunctions.CheckDiagonalCells(room, new Coord(j, i), true, new Cell(1));
                            if(data.x + data.y != 4)
                            {
                                room.SetCell(new Coord(j, i), new Cell(2));
                            }
                        }
                    }
                }
            }
            return room;
        }
    }

    

   
}
