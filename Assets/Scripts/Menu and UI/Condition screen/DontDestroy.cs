using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DontDestroy : MonoBehaviour
{
public static DontDestroy instance;
public bool win;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null){
            Debug.Log("Destroying a do not destroy with health" + gameObject.GetComponent<IntraSceneStats>().health);
            gameObject.GetComponent<IntraSceneStats>().destroyed = true;
            Destroy(gameObject);
        }
        else {instance = this;} DontDestroyOnLoad(gameObject);
        
    }
}


