using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Place(Vector3 position)
    {
        gameObject.transform.position = position;
    }
}
