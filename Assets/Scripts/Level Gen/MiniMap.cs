using JAFprocedural;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField]public Image[] mapIcons = new Image[24];
    private List<int> knownIndexes = new List<int>();
    private List<int> startupSquares = new List<int>();
    public int currentLocation = -1;
    bool literallyAnythingElse = false;
    public Color yellowColour = new Color(230, 207, 161);
    public Color greenColour = new Color(112, 161, 143);
    public Color blackColour = new Color(66, 30, 66);
    private Coord bRoom = new Coord(-1, -1);
    // Start is called before the first frame update
    void Start()
    {
        foreach (Image icon in mapIcons) icon.color = blackColour;
    }

    private void Update()
    {
        if(!literallyAnythingElse && currentLocation != -1)
        {
            literallyAnythingElse = true;
            mapIcons[currentLocation].color = yellowColour;

            for (int i = 0; i < startupSquares.Count; mapIcons[startupSquares[i]].color = Color.white, i++) ;
            if(bRoom.x!= -1) mapIcons[bRoom.y * 6 + bRoom.x].color = Color.red;
        }
    }

    public void OnRoomEnter(Coord newLocation)
    {
        if(currentLocation != -1)mapIcons[currentLocation].color = greenColour;

        mapIcons[(newLocation.y * 6) + newLocation.x].color = yellowColour;
        currentLocation = newLocation.y * 6 + newLocation.x;

        knownIndexes.Add((newLocation.y * 6) + newLocation.x);
    }

    public void SetBossRoom(Coord bossRoomLoc)
    {
        bRoom = bossRoomLoc;
        knownIndexes.Add(bRoom.y * 6 + bRoom.x);
    }

    public void SetUnopenedDoor(Coord room)
    {
        if (!literallyAnythingElse)
        {
            startupSquares.Add(room.y * 6 + room.x);
        }

        int value = room.y * 6 + room.x;
        if(!knownIndexes.Contains(value))
        {
            mapIcons[value].color = Color.white;
            Debug.Log("i didn't know that");
        }
        else
        {
            Debug.Log("i already KNEW that: " + value.ToString());
        }
    }
}
