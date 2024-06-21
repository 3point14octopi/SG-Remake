using System;
using System.Collections.Generic;
using System.Linq;

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

            megaMap = new Space2D(90, 36);

            for(int i = 0; i < minimap.height; i++)
            {
                for(int j = 0; j < minimap.width; j++)
                {
                    if(minimap.GetCell(j, i) > 0)
                    {
                        Coord location = new Coord(j, i);
                        Space2D room = new Space2D(15, 9);
                        room = SG_MapGen.SetDoors(room, location, minimap);

                        if(minimap.GetCell(j, i) == 1)
                        {
                            room = SG_MapGen.MakeStartingRoom(room);
                        }
                        else
                        {
                            room = (minimap.GetCell(j, i) < 8) ? SG_MapGen.MakeRoom1(room) : 
                                (minimap.GetCell(j, i) == 9) ? SG_MapGen.MakeBossRoom(room) : SG_MapGen.MakeRoom2(room);
                        }
                        room.worldOrigin = new Coord(j * 15, i * 9);

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

            for (int i = 0; i < 6;)
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
            for(int i = 0; i < points.Count-1; i++)
            {
                BasicBuilderFunctions.ConnectCoords(cover, points[0], points[i+1], new Cell(8));
            }

            int extraCount = 0;

            //places the connector rooms without overwriting the old ones
            for (int i = 0; i < cover.height; i++)
            {
                for (int j = 0; j < cover.width; j++)
                {
                    if (cover.GetCell(j, i) == 8 && floorPlan.GetCell(j, i) == 0)
                    {
                        floorPlan.SetCell(new Coord(j, i), new Cell(8));
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
                        floorPlan.SetCell(bossRoom, new Cell(9));
                        bossPlaced = true;
                    }
                }
            }

            return floorPlan;
        }



        public static Space2D MakeRoom1(Space2D room)
        {
            room = DownwardSpill(room, new Coord(7, 1));
            room = DownwardSpill(room, new Coord(7, 7), false);

            if(BasicBuilderFunctions.PercentageOf(room, new Cell(1)) > 0.55f)
            {
                room = LittleTimmy(room);
            }
            room = EnsureConnected(room);

            return room;
        }

        public static Space2D MakeRoom2(Space2D room)
        {
            room = NewJimmy(room);

            return room;
        }

        public static Space2D MakeStartingRoom(Space2D room)
        {
            int[,] preset = new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
                {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
                {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0}
            };                    
            BasicBuilderFunctions.Flood(room, new Cell(0), new Cell(1), 1, 1, room.width - 1, room.height - 1);

            Space2D copy = new Space2D(preset);
            BasicBuilderFunctions.CopySpaceAToB(copy, room, new List<Cell> { new Cell(0) });

            return room;
        }

        public static Space2D MakeBossRoom(Space2D room)
        {
            BasicBuilderFunctions.Flood(room, new Cell(0), new Cell(1), 1, 1, room.width-1, room.height-1);
            return room;
        }


        public static Space2D SetDoors(Space2D room, Coord roomLoc, Space2D layoutRef)
        {
            Cell opening = new Cell(1);

            //up
            if (roomLoc.y > 0 && layoutRef.GetCell(new Coord(roomLoc.x, roomLoc.y - 1)) != 0)
            {
                room.SetCell(new Coord(7, 0), opening);
                room.SetCell(new Coord(7, 1), opening);
            }

            //down
            if (roomLoc.y < layoutRef.height-1 && layoutRef.GetCell(new Coord(roomLoc.x, roomLoc.y+1)) != 0)
            {
                room.SetCell(new Coord(7, 7), opening);
                room.SetCell(new Coord(7, 8), opening);
            }

            //left
            if (roomLoc.x > 0 && layoutRef.GetCell(new Coord(roomLoc.x-1, roomLoc.y)) != 0)
            {
                room.SetCell(new Coord(0, 4), opening);
                room.SetCell(new Coord(1, 4), opening);
            }

            //right
            if (roomLoc.x < layoutRef.width-1 && layoutRef.GetCell(new Coord(roomLoc.x + 1, roomLoc.y)) != 0)
            {
                room.SetCell(new Coord(14, 4), opening);
                room.SetCell(new Coord(13, 4), opening);
            }

            return room;
        }

        public static int DetermineChokeValue(int iterationNum)
        {
            return (iterationNum == 1)? 0 : RNG.GenRand(1, 2)-1;
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
            int lLimit = (leftStride < 3) ? leftStride : 3;
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

            //right
            int rLimit = (rightStride < 3) ? rightStride : 3;
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
            

            if(iteration <= 1 || RNG.GenRand(1, 4) > 1)
            {
                if (newLeftStart > -1 && iteration < (int)(room.width/2))
                {
                    Coord lC = new Coord(leftPoints[newLeftStart].x, leftPoints[newLeftStart].y + ((downward) ? 1 : -1));
                    room = DownwardSpill(room, lC, downward, iteration, fill);
                }
            }
            
            if(iteration <= 1 || RNG.GenRand(1, 4) > 1)
            {
                if (newRightStart > -1 && iteration < (int)(room.width/2))
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
            do { reunion = new Coord(RNG.GenRand(2, room.width-5), RNG.GenRand(2, room.height-5)); } while (room.GetCell(reunion) != 1);

            BasicBuilderFunctions.ConnectCoords(room, reunion, new Coord(7,  1), fill);
            BasicBuilderFunctions.ConnectCoords(room, reunion, new Coord(7, 7), fill);
            BasicBuilderFunctions.ConnectCoords(room, reunion, new Coord(1,  4), fill);
            BasicBuilderFunctions.ConnectCoords(room, reunion, new Coord(13, 4), fill);

            return room;
        }
        
        public static Space2D LittleTimmy(Space2D room)
        {
            int timmyPlaced = 0;
            Cell bingChillin = new Cell(0);

            for(int i = 0; i < 20 && timmyPlaced < 10; i++)
            {
                Coord c = new Coord(RNG.GenRand(2, room.width-5), RNG.GenRand(2, room.height-5));
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
            if (room.GetCell(0, 4) != 0)  points.Add(new Coord(1, 4));
            if (room.GetCell(7, 0) != 0) points.Add(new Coord(7, 1));
            if (room.GetCell(7, 8) != 0)points.Add(new Coord(7, 7));
            if (room.GetCell(14, 4) != 0) points.Add(new Coord(13, 4));


            int iterations = RNG.GenRand(1, 8 - points.Count);
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
        public static Space2D UnderLine(Space2D room, int valid, Cell toFind, Cell underlineVal, bool seamless = true)
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



        //recursively finds an easy distance to travel to
        private static Coord Target(Coord start, Coord dest)
        {
            int x = dest.x - start.x;
            int y = dest.y - start.y;

            if (MathF.Sqrt(MathF.Pow(x, 2) + MathF.Pow(y, 2)) < 10)
            {
                return dest;
            }
            else
            {
                Coord between = new Coord();
                between.x = (int)((dest.x - start.x) / 2) + start.x;
                between.y = (int)((dest.y - start.y) / 2) + start.y;

                return Target(start, between);
            }
        }

        /// <summary>
        /// Does A* in short bursts on the map
        /// needs pre-loading
        /// (is mostly just an "as the crow flies" across empty space, use different method if obstacles are in the way)
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="start"></param>
        /// <param name="map"></param>
        /// <param name="val"></param>
        private static Space2D StaggeredAStar(Coord finalDestination, Coord start, Space2D map, Cell pathValue)
        {
            Coord curDest;
            Coord prev = start;

            do
            {
                curDest = Target(prev, finalDestination);
                List<Coord> path = astar.AStar(prev, curDest);

                if (path != null)
                {
                    foreach (Coord node in path) map.SetCell(node, pathValue);
                }

                prev = new Coord(curDest.x + 0, curDest.y + 0);
            } while (MathF.Abs(curDest.x - finalDestination.x) > 3 || MathF.Abs(curDest.y - finalDestination.y) > 3);

            return map;
        }

        public static Space2D NewJimmy(Space2D room)
        {
            List<Coord> points = new List<Coord>();
            //can override this if you start being SMARTER about door locations
            if (room.GetCell(0, 4) != 0) points.Add(new Coord(1, 4));
            if (room.GetCell(7, 0) != 0) points.Add(new Coord(7, 1));
            if (room.GetCell(7, 8) != 0) points.Add(new Coord(7, 7));
            if (room.GetCell(14, 4) != 0) points.Add(new Coord(13, 4));

            //fill room with valid space
            BasicBuilderFunctions.Flood(room, new Cell(0), new Cell(1), 1, 1, room.width - 1, room.height - 1);

            //send room to the A* calculator
            astar.SetNewGrid(room, 1);

            //can throw in a couple of extra points if you really care
            int iterations = RNG.GenRand(0, 5-points.Count);
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

            //connect them with staggered a* (works because the room is otherwise empty)
            for (iterations = 0, RNG.CircleSelect(points, 0);
                iterations < points.Count - 1;
                room = StaggeredAStar(RNG.CircleSelect(points, iterations + 1), points[iterations], room, new Cell(2)), iterations++) ;

            //widen paths and flood
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

    

    //public class SG_LevelVariant
    //{
    //    public Space2D megaMap;
    //    public List<Coord> roomCentres;
    //    Space2D copy;
    //    AStarCalculator aStar;
    //    Space2D urMom;
    //    public SG_LevelVariant()
    //    {
    //        megaMap = new Space2D(125, 80);
    //        roomCentres = new List<Coord>();
    //        copy = new Space2D(125, 80);
    //        BasicBuilderFunctions.Flood(copy, new Cell(0), new Cell(1), 1, 1, copy.width - 1, copy.height - 1);
    //        aStar = new AStarCalculator(copy, 1);



    //        for (int i = 0; i < 10; i++)
    //        {
    //            Coord start = new Coord();

    //            for(bool succeeded = false; succeeded != true;)
    //            {
    //                start = new Coord(RNG.GenRand(1, 100), RNG.GenRand(1, 65));
    //                bool tooClose = false;
    //                for(int j = 0; j < roomCentres.Count && !tooClose; j++)
    //                {
    //                    int xDist = Math.Abs((start.x + 12) - (roomCentres[j].x));
    //                    int yDist = Math.Abs((start.y + 7) - (roomCentres[j].y));
    //                    if(xDist <= 20 && yDist <= 12){
    //                        tooClose = true;
    //                    }
    //                }

    //                succeeded = !tooClose;

    //            }


    //            roomCentres.Add(new Coord(start.x + 9, start.y + 6));

    //            Space2D room = new Space2D(17, 12);
    //            //
    //            //if(i == 9)
    //            //{
    //            //    //SG_MapGen.MakeStartingRoom(room);
    //            //}
    //            //else
    //            //{
    //                SG_MapGen.DownwardSpill(room, new Coord(9, 1));
    //                SG_MapGen.DownwardSpill(room, new Coord(9, 10), false);
    //            //}
                
                
    //            room.worldOrigin = start;
    //            BasicBuilderFunctions.CopySpaceAToB(room, megaMap, new List<Cell> { new Cell(1) });
    //        }

    //        AltConnection();
    //        //Slowwww();
    //    }





    //    private Coord Target(Coord start, Coord dest)
    //    {
    //        int x = dest.x - start.x;
    //        int y = dest.y - start.y;

    //        if(MathF.Sqrt(MathF.Pow(x, 2) + MathF.Pow(y, 2)) < 10)
    //        {
    //            return dest;
    //        }
    //        else
    //        {
    //            Coord between = new Coord();
    //            between.x = (int)((dest.x - start.x) / 2) + start.x;
    //            between.y = (int)((dest.y - start.y) / 2) + start.y;

    //            return Target(start, between);
    //        }
    //    }

    //    private void Slowwww()
    //    {
    //        for (int i = 0; i < roomCentres.Count - 1; i++)
    //        {
    //            megaMap = StaggeredAStar(roomCentres[i + 1], roomCentres[i], megaMap, new Cell(2));
    //        }

    //        megaMap = SG_MapGen.UnderLine(megaMap, 0, new Cell(2), new Cell(3));
    //        BasicBuilderFunctions.Flood(megaMap, new Cell(2), new Cell(1));
    //    }

    //    private int SmallestIndex(List<float> numbers)
    //    {
    //        float min = float.MaxValue;
    //        int index = -1;

    //        for(int i = 0; i < numbers.Count; i++)
    //        {
    //            if (numbers[i] < min)
    //            {
    //                min = numbers[i];
    //                index = (i + 0);
    //            }
    //        }
    //        return index;
    //    }

    //    private float CoordDist(Coord a, Coord b)
    //    {
    //        int x = b.x - a.x;
    //        int y = b.y - a.y;

    //        return MathF.Sqrt(MathF.Pow(x, 2) + MathF.Pow(y, 2));
    //    }

    //    /// <summary>
    //    /// Does A* in short bursts on the map
    //    /// needs pre-loading
    //    /// (is mostly just an "as the crow flies" across empty space, use different method if obstacles are in the way)
    //    /// </summary>
    //    /// <param name="goal"></param>
    //    /// <param name="start"></param>
    //    /// <param name="map"></param>
    //    /// <param name="val"></param>
    //    private Space2D StaggeredAStar(Coord finalDestination, Coord start, Space2D map, Cell pathValue)
    //    {
    //        Coord curDest;
    //        Coord prev = start;
    //        bool notDone;

    //        do
    //        {
    //            curDest = Target(prev, finalDestination);
    //            List<Coord> path = aStar.AStar(prev, curDest);

    //            if (path != null)
    //            {
    //                foreach (Coord node in path) map.SetCell(node, pathValue);
    //            }

    //            prev = new Coord(curDest.x + 0, curDest.y + 0);
    //            notDone = MathF.Abs(curDest.x - finalDestination.x) > 3 || MathF.Abs(curDest.y - finalDestination.y) > 3;
    //            if(notDone && RNG.GenRand(1,5) > 4)
    //            {
    //                urMom.worldOrigin = new Coord(curDest.x - 1, curDest.y - 1);
    //                BasicBuilderFunctions.CopySpaceAToB(urMom, map, new List<Cell> { new Cell(1) });
    //            }
    //        } while (notDone);

    //        return map;
    //    }


    //    private void AltConnection()
    //    {
    //        Space2D copy = new Space2D(125, 80);
    //        BasicBuilderFunctions.Flood(copy, new Cell(0), new Cell(1), 1, 1, copy.width - 1, copy.height - 1);
    //        urMom = new Space2D(3, 3);
    //        BasicBuilderFunctions.Flood(urMom, new Cell(0), new Cell(1));


    //        List<List<int>> connections = new List<List<int>>();
    //        for (int i = 0; i < roomCentres.Count; connections.Add(new List<int>()), i++) ;
            
    //        List<float> distances;

    //        for(int i = 0; i < roomCentres.Count; i++)
    //        {
    //            distances = new List<float>();
    //            for(int j = 0; j < roomCentres.Count; j++)
    //            {
    //                distances.Add((i == j)?float.MaxValue:CoordDist(roomCentres[i], roomCentres[j]));
    //            }

    //            int iterations;
    //            int cIteration;
    //            for (iterations = 0, cIteration = 0; cIteration < 2; iterations++)
    //            {
    //                int current = SmallestIndex(distances);
    //                distances[current] = float.MaxValue;

    //                if (!connections[i].Contains(current))
    //                {
    //                    connections[i].Add(current);
    //                    int x = i + 0;
    //                    connections[current].Add(x);
    //                    BasicBuilderFunctions.ConnectCoords(megaMap, roomCentres[current], roomCentres[i], new Cell(2));
    //                    cIteration++;
    //                }
    //                else if (cIteration > 0)
    //                {
    //                    cIteration++;
    //                }
    //                else if (iterations > roomCentres.Count - 2)
    //                {
    //                    cIteration = 2;
    //                }
    //            }


    //        }

    //        //megaMap = SG_MapGen.UnderLine(megaMap, 0, new Cell(2), new Cell(3));
    //        BasicBuilderFunctions.Flood(megaMap, new Cell(2), new Cell(1));
    //        BasicBuilderFunctions.Flood(megaMap, new Cell(3), new Cell(2));
    //    }
    //}
   
}
