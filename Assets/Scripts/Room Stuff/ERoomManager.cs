using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JAFprocedural;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomEnemy
{
    public int enemyID;
    public Coord startingLocation;

    public RoomEnemy(int id, Coord location)
    {
        enemyID = id;
        startingLocation = location;
    }
}

public class RoomWithEnemies
{
    public List<RoomEnemy> enemies = new List<RoomEnemy>();
    int aliveCount = 0;
    public bool isBossRoom = false;

    public RoomWithEnemies(bool boss = false)
    {
        aliveCount = 0;
        isBossRoom = boss;
    }
    public void Add(RoomEnemy enemy)
    {
        enemies.Add(enemy);
        aliveCount++;
    }

    //this is really, really bad
    public void KillAnEnemy()
    {
        if (aliveCount > 0) aliveCount--;
    }

    public void DropLast()
    {
        if(enemies.Count > 0)
        {
            enemies.RemoveAt(enemies.Count - 1);
            aliveCount--;
        }
        
    }

    public int Population()
    {
        return aliveCount;
    }
    public bool IsClear()
    {
        return (aliveCount <= 0);
    }
}

public class ERoomManager : MonoBehaviour
{
    public static ERoomManager Instance;

    //private Dictionary<Coord, RoomWithEnemies> enemyLists = new Dictionary<Coord, RoomWithEnemies>();
    private RoomWithEnemies[,] enemyLists = new RoomWithEnemies[4, 6];
    private Space2D[,] layouts = new Space2D[4, 6];
    private Space2D bossRoom = null;
    List<Coord> usedRooms = new List<Coord>();

    public Space2D RequestRoom(Coord id)
    {
        return layouts[id.y, id.x];
    }

    public bool IsFloorCleared()
    {
        foreach (Coord room in usedRooms)
        {
            if (enemyLists[room.y, room.x].enemies.Count > 0 && enemyLists[room.y, room.x].Population() != 0)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsBossUnlocked()
    {
        foreach (Coord room in usedRooms)
        {
            if (enemyLists[room.y, room.x].enemies.Count > 0 && enemyLists[room.y, room.x].Population() != 0)
            {
                if (enemyLists[room.y, room.x].isBossRoom == false)
                {
                    Debug.Log("boss room inaccessible");
                    return false;
                }
            }
        }
        Debug.Log("boss room accessible");
        return true;
    }


    private const int EMPTY = 2;
    private const int TIMMY = 4;
    private const int WISP = 5;
    private const int JIMMY = 7;
    private const int BOSS = 9;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    

    private RoomWithEnemies AddBeans(Space2D room, RoomWithEnemies list, int min = 0)
    {
        int amount = RNG.GenRand(0, 4) + RNG.GenRand(0, 4);
        if (min > amount) amount = min;

        for(int i = 0; i < amount;)
        {
            Coord startPos = new Coord(RNG.GenRand(2, room.width - 4), RNG.GenRand(2, room.height - 4));
            if (room.GetCell(startPos) == 1)
            {
                Coord around = BasicBuilderFunctions.CheckAdjacentCells(room, startPos);
                if (around.x + around.y != 4)
                {
                    room.SetCell(startPos, new Cell(99));
                    startPos.x += room.worldOrigin.x;
                    startPos.y += room.worldOrigin.y;
                    int temp = i;
                    RoomEnemy enemy = new RoomEnemy(temp, startPos);
                    list.Add(enemy);
                    i++;
                }

            }
        }

        return list;
    }
    private RoomWithEnemies AddKnights(Space2D room, RoomWithEnemies list, int overShoot = 0)
    {
        int quantity = 5 - overShoot;
        List<int> rowsSelected = new List<int>();

        for(int i = 0; i < RNG.GenRand(0, quantity);)
        {
            Coord location = new Coord(RNG.GenRand(2, room.width - 4), RNG.GenRand(2, room.height - 4));

            if(!rowsSelected.Contains(location.y) && room.GetCell(location) == 1)
            {
                int tempRow = location.y;
                rowsSelected.Add(tempRow);

                int tempID = i;
                location.x += room.worldOrigin.x;
                location.y += room.worldOrigin.y;

                RoomEnemy r = new RoomEnemy(tempID + 6, location);
                list.Add(r);
                i++;
            }
            

        }

        return list;
    }
    private RoomWithEnemies AddSpectres(Space2D room, RoomWithEnemies list)
    {

        for(int i = 0; i < RNG.GenRand(0, 5);)
        {
            Coord startPos = new Coord(RNG.GenRand(2, room.width - 4), RNG.GenRand(2, room.height - 4));

            if(room.GetCell(startPos) == 1)
            {
                room.SetCell(startPos, new Cell(99));
                startPos.x += room.worldOrigin.x;
                startPos.y += room.worldOrigin.y;

                int temp = i;

                RoomEnemy r = new RoomEnemy(temp + 11, startPos);
                list.Add(r);
                i++;
            }
        }

        return list;
    }

    private RoomWithEnemies TurkeyRoomGen(Space2D room)
    {
        RoomWithEnemies eList = new RoomWithEnemies();
        int iterations = RNG.GenRand(0, 3);
        for(int i = 0; i < iterations;)
        {
            Coord startPos = new Coord(RNG.GenRand(3, room.width - 6), RNG.GenRand(3, room.height - 6));
            if (room.GetCell(startPos) == 1)
            {
                room.SetCell(startPos, new Cell(99));
                startPos.x += room.worldOrigin.x;
                startPos.y += room.worldOrigin.y;
                int temp = i;
                RoomEnemy enemy = new RoomEnemy(23 + temp, startPos);
                eList.Add(enemy);
                i++;
            }
        }

        int min = (eList.Population() < 2) ? ((eList.Population() == 0) ? 2 : RNG.GenRand(1, 2)) : 0;

        eList = AddBeans(room, eList, min);

        Debug.Log(eList.enemies.Count.ToString() + " enemies in room");

        return eList;
    }

    //"basic" my ass, this one is the worst
    private RoomWithEnemies BasicEnemyRoom(Space2D room)
    {
        RoomWithEnemies eList = new RoomWithEnemies();

        switch (RNG.GenRand(1, 2))
        {
            case (1):
                eList = AddBeans(room, eList, 2);
                eList = (RNG.GenRand(0, 2) == 0) ? AddKnights(room, eList, (eList.Population() == 6) ? 1 : 0) : AddSpectres(room, eList);
                break;
            case (2):
                eList = AddKnights(room, eList);
                eList = AddSpectres(room, eList);
                if (eList.Population() < 2) eList = AddBeans(room, eList, 2);
                break;
            default:
                break;
        }

        return eList;
    }

    private RoomWithEnemies WispRoomGen(Space2D room)
    {
        RoomWithEnemies eList = new RoomWithEnemies();

        Coord[] options = new Coord[8];


        for(int i = 0; i < 8;)
        {
            Coord location = new Coord(RNG.GenRand(10, 5), RNG.GenRand(5, 5));
            if(room.GetCell(location) == 1)
            {
                Coord sur = BasicBuilderFunctions.CheckAdjacentCells(room, location, true, new Cell(1));
                if(sur.x + sur.y != 4 && !options.Contains<Coord>(location))
                {
                    options[i] = location;
                    i++;
                }
            }
        }

        foreach (Coord place in options)
        {
            place.x += room.worldOrigin.x;
            place.y += room.worldOrigin.y;
        }

        for (int i = 0; i < 8; i++)
        {

            int temp = i;
            RoomEnemy r = new RoomEnemy(15 + temp, options[i]);
            eList.Add(r);
        }

        eList = AddKnights(room, eList, 3);

        return eList;
    }

    private RoomWithEnemies BossRoom(Space2D room, int bossID = 0)
    {
        RoomWithEnemies r = new RoomWithEnemies(true);

        r.Add(new RoomEnemy(25 + bossID, new Coord(12 + room.worldOrigin.x, 7 + room.worldOrigin.y)));

        return r;
    }

    public void CreateDataForRoom(int roomValue, Space2D room, Coord location)
    {
        if (roomValue < EMPTY)
        {
            enemyLists[location.y, location.x] = new RoomWithEnemies();
            usedRooms.Add(location);
        }
        else if (roomValue < TIMMY)
        {
            enemyLists[location.y, location.x] = BasicEnemyRoom(room);
            usedRooms.Add(location);
        }
        else if (roomValue < WISP)
        {
            enemyLists[location.y, location.x] = WispRoomGen(room);
            usedRooms.Add(location);
        }
        else if (roomValue < JIMMY)
        {
            
            enemyLists[location.y, location.x] = new RoomWithEnemies();
            usedRooms.Add(location);
            Debug.Log("empty room at " + location.x.ToString() + ',' + location.y.ToString() + " (room value: " + roomValue.ToString() + ')');
        }else if(roomValue < BOSS)
        {
            enemyLists[location.y, location.x] = BossRoom(room);
        }

        BasicBuilderFunctions.Flood(room, new Cell(99), new Cell(1));
        layouts[location.y, location.x] = room;
    }

    public void OnRoomEnter(Coord roomLoc)
    {
        RoomPop.Instance.LoadRoom(enemyLists[roomLoc.y, roomLoc.x], roomLoc);
    }


    /// <summary>
    /// NEEDS EDITING
    /// </summary>
    /// <param name="roomMap"></param>
    /// <param name="roomType"></param>
    /// <returns></returns>
    public List<RoomEnemy> Populate(Space2D roomMap, int roomType)
    {
        RoomWithEnemies r = new RoomWithEnemies();
        //touch

        //if (roomType == EMPTY)
        //{
        //    //do nothing
        //}
        //else if (roomType == WISP)
        //{
        //    r = WispRoomGen(roomMap);
        //}
        //else if (roomType < JIMMY)
        //{
        r = BasicEnemyRoom(roomMap);
        //}
        //else if (roomType == BOSS)
        //{
        //    r = BossRoom(roomMap);

        //}

        return r.enemies;
    }
}
