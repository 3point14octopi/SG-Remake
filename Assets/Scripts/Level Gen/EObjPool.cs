using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EObjPool : MonoBehaviour
{
    

    // Start is called before the first frame update
    
    [Header("Prefabs")]
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public List<int> maximums = new List<int>();
    [Header("Pool")]
    public List<GameObject> enemyPool = new List<GameObject>();

    public static EObjPool Instance;

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


    void Start()
    {
        for (int i = 0; i < enemyPrefabs.Count; AddToPool(i, maximums[i]), i++) ;

        for (int i = 0; i < enemyPool.Count; enemyPool[i].SetActive(false), i++) ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddToPool(int enemyType, int amount)
    {
        for (int i = 0; i < amount; enemyPool.Add(GameObject.Instantiate(enemyPrefabs[enemyType], transform, true)), i++) ;
    }


}
