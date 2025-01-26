
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RatLaunchStats
{
    public Vector3 launchPosition;
    public int launchDirection;
    public bool isActive;

    public void ToggleActive(bool a)
    {
        isActive = a;
    }
}

public class RatLauncher : MonoBehaviour
{
    public GameObject ratPrefab;
    private GameObject activeRat;
    private int activeCount = 0;

    private int ratIndex;
    public List<RatLaunchStats> ratLaunchStats;
   

    private void Start()
    {
        //foreach (RatLaunchStats a in ratLaunchStats) a.ToggleActive(false);
    }

    private void Update()
    {
        
       LaunchRat();
    }

    public void LaunchRat()
    {
        
        if (activeCount < 5) {
            activeCount++;
           do
           {
                ratIndex = Random.Range(0, ratLaunchStats.Count);
           } while (ratLaunchStats[ratIndex].isActive);
            ratLaunchStats[ratIndex].ToggleActive(true);

            activeRat = (GameObject)Instantiate(ratPrefab, transform);
            activeRat.GetComponent<TreeRatBehaviour>().CreateRat(ratLaunchStats[ratIndex], gameObject);
        }

    }

    public void RemoveActive()
    {
        activeCount--;
    }
}
