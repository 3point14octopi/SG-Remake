using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using JAFprocedural;

public class DungeonGenerator : MonoBehaviour
{
    Space2D map = new Space2D();
    Space2D room = new Space2D();
    public Text output;
    public Text roomOutput;
    // Start is called before the first frame update
    void Start()
    {
        GenThing();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenThing();
        }
    }

    void GenThing()
    {
        map = SG_MapGen.MakeFloorplan();
        PrintThing(output, map);
        //room = SG_MapGen.MakeRoom2();
        //PrintThing(roomOutput, room, true);
    }

    void PrintThing(Text t, Space2D s, bool showZeroes = false)
    {
        char zeros = (showZeroes) ? '#' : ' ';
        char altOthers = ' ';
        string txt = "";
        for(int i = 0; i < s.height; txt += '\n', i++)
        {
            for(int j = 0; j < s.width; j++)
            {
                int c = s.GetCell(j, i);
                txt += (c == 0)?zeros: ((showZeroes)?altOthers:c.ToString());
            }
        }
        t.text = txt;
    }
}
