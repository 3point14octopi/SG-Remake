
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RatLaunchStats
{
    public Vector3 launchPosition;
    public int launchDirection;
}

public class RatLauncher : MonoBehaviour
{
    public GameObject ratPrefab;
    private GameObject activeRat;
    private int activeCount = 0;
    private int testCount = 0;

    private int ratIndex;
    public List<RatLaunchStats> ratLaunchStats;
    private bool[] isActiveArray = new bool[45];

    public GameObject ratKing;
    private Brain bossBrain;


    private void Start()
    {
        bossBrain = ratKing.GetComponent<Brain>();
    }



    private void Update()
    {
       if(bossBrain.isAlive) LaunchRat();
    }

    public void LaunchRat()
    {
        
        if (activeCount < 5) {
            activeCount++;
           do
           {
                ratIndex = Random.Range(0, ratLaunchStats.Count);
           } while (isActiveArray[ratIndex]);
            ToggleActive(ratIndex, true);
            activeRat = (GameObject)Instantiate(ratPrefab, transform);
            activeRat.GetComponent<TreeRatBehaviour>().CreateRat(ratLaunchStats[ratIndex], gameObject, ratIndex);
        }

    }
    public void ToggleActive(int index, bool a)
    {
        isActiveArray[index] = a;
    }

    public void RemoveActive(int index)
    {
        ToggleActive(index, false);
        activeCount--;
    }

}
