using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;





public class DoorCollider : MonoBehaviour
{
    public Vector3 nextRoomOffset;
    public Vector2Int shift;
    CamShift cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponentInParent<CamShift>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Frostbite")
        {
            collision.gameObject.transform.position += nextRoomOffset;

            cam.Shift(shift.x, shift.y);
        }
    }

}
