using JAFprocedural;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField]public Image[] mapIcons = new Image[24];
    public int currentLocation = -1;
    bool literallyAnythingElse = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Image icon in mapIcons) icon.color = Color.black;
    }

    private void Update()
    {
        if(!literallyAnythingElse && currentLocation != -1)
        {
            literallyAnythingElse = true;
            mapIcons[currentLocation].color = Color.yellow;
        }
    }

    public void OnRoomEnter(Coord newLocation)
    {
        if(currentLocation != -1)mapIcons[currentLocation].color = Color.green;

        Debug.Log("you are in " + newLocation.x.ToString() + ',' + newLocation.y.ToString());
        Debug.Log(((newLocation.y * 6) + newLocation.x).ToString());
        mapIcons[(newLocation.y * 6) + newLocation.x].color = Color.yellow;
        currentLocation = newLocation.y * 6 + newLocation.x;

        //if (!literallyAnythingElse)
        //{
        //    if (newLocation.x + newLocation.y != 0) literallyAnythingElse = true;
        //}

        //if(currentLocation.x != -1){
        //    mapIcons[currentLocation.y * 6 + currentLocation.x].color = Color.green;
        //    Debug.Log("greemn at " + currentLocation.x.ToString() + ',' + currentLocation.y.ToString());

        //}

        

        //if (literallyAnythingElse)
        //{
        //    currentLocation = newLocation;
        //}

        

        
    }
}
