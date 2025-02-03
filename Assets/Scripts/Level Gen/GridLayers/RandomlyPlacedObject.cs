using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an example script that showcases the "find an empty spot" features
/// The important thing here is you want to call FindUnoccupiedTile before LastLocated
/// and do so in a coroutine because FindUnoccupied needs to finish executing
/// </summary>
public class RandomlyPlacedObject : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(DropObject());
        }
    }

    public IEnumerator DropObject()
    {
        yield return AstarDebugLayer.Instance.FindUnoccupiedTile();

        Vector2 hold = AstarDebugLayer.Instance.LastLocatedUnoccupied();
        transform.position = new Vector3(hold.x, hold.y, transform.position.z);
        
        yield return null;
    }
}
