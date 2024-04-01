using JAFprocedural;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CamShift : MonoBehaviour
{
    private Coord currentPosition = new Coord();
    public MiniMap mm;

    public void SetPosition(Coord position)
    {
        transform.position = new Vector3((position.x * 25) + 12.65f, (position.y * -15) -5.85f, -5);
        Debug.Log("teleported");

        currentPosition = position;

        EnemiesAndDoors(currentPosition);
    }

    public void Shift(int xOffset, int yOffset)
    {
        currentPosition.x += xOffset;
        currentPosition.y += yOffset;

        transform.position = new Vector3((currentPosition.x * 25) + 12.65f, (currentPosition.y * -15) - 5.85f, -5);
        Debug.Log("shifted to " + currentPosition.x.ToString() + ',' + currentPosition.y.ToString());

        EnemiesAndDoors(currentPosition);
    }

    public void EnemiesAndDoors(Coord room)
    {
        ERoomManager.Instance.OnRoomEnter(room);
        mm.OnRoomEnter(room);
    }
}
