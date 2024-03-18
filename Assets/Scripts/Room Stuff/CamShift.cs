using JAFprocedural;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CamShift : MonoBehaviour
{
    private Coord currentPosition = new Coord();
    [SerializeField]public Vector3[] tpPositions = new Vector3[24];

    

    public void SetPosition(Coord position)
    {
        transform.position = new Vector3((position.x * 25) + 8, (position.y * -15) -2, -5);
        Debug.Log("teleported");

        currentPosition = position;
    }

    public void Shift(int xOffset, int yOffset)
    {
        currentPosition.x += xOffset;
        currentPosition.y += yOffset;

        transform.position = new Vector3((currentPosition.x * 25) + 8, (currentPosition.y * -15) - 2, -5);
        Debug.Log("shifted");
    }
}
