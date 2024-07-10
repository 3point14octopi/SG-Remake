using System.Collections.Generic;
using System.Linq;
using JAFprocedural;
using UnityEngine;

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

    public RoomWithEnemies()
    {
        aliveCount = 0;
    }
    public void Add(RoomEnemy enemy)
    {
        enemies.Add(enemy);
        aliveCount++;
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
}

public class ERoomManager : MonoBehaviour
{
    public static ERoomManager Instance;




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
        int amount = RNG.GenRand(0, 2) + RNG.GenRand(0, 2);
        if (min > amount) amount = min;

        for(int i = 0; i < amount;)
        {
            Coord startPos = new Coord(RNG.GenRand(4, room.width - 8), RNG.GenRand(3, room.height - 6));
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
            Coord location = new Coord(RNG.GenRand(4, room.width - 8), RNG.GenRand(3, room.height - 6));

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

        for(int i = 0; i < RNG.GenRand(1, 3);)
        {
            Coord startPos = new Coord(RNG.GenRand(4, room.width - 8), RNG.GenRand(3, room.height - 6));

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
            Coord startPos = new Coord(RNG.GenRand(4, room.width - 8), RNG.GenRand(3, room.height - 6));
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

        BasicBuilderFunctions.Flood(room, new Cell(99), new Cell(1));
        return eList;
    }

    private RoomWithEnemies WispRoomGen(Space2D room)
    {
        RoomWithEnemies eList = new RoomWithEnemies();

        Coord[] options = new Coord[8];


        for(int i = 0; i < 8;)
        {
            Coord location = new Coord(RNG.GenRand(5, 5), RNG.GenRand(3, 3));
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


        return eList;
    }

    private RoomWithEnemies BossRoom(Space2D room, int bossID = 0)
    {
        RoomWithEnemies r = new RoomWithEnemies();

        r.Add(new RoomEnemy(25 + bossID, new Coord(7 + room.worldOrigin.x, 4 + room.worldOrigin.y)));

        return r;
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

        if(roomType != 1)
        {
            if(roomType < 8)
            {
                r = (roomType == 3) ? WispRoomGen(roomMap) : BasicEnemyRoom(roomMap);
            }else if(roomType == 9)
            {
                r = BossRoom(roomMap);
            }
        }

        BasicBuilderFunctions.Flood(roomMap, new Cell(99), new Cell(1));
        return r.enemies;
    }
}
