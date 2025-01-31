
using System.Collections;
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
    public GameObject ratPrefab; //tree rat prefab
    private GameObject[] activeRats = new GameObject[25]; //stores references to the rats running across
    private int activeRatsIndex = -1; //index used to keep track of the next slot in the array

    [Tooltip("How many rats can be on screen at once?")]
    public int maxActive;
    public bool isConstant = false;
    private int activeCount = 0;

    //bools for waves
    private bool waveReady = false;
    public float waveTime;
    public int waveNum;


    private int ratIndex; 
    public List<RatLaunchStats> ratLaunchStats; //array of possible spawn locations
    private bool[] isActiveArray = new bool[30]; //array remembering what lanes are available

    public GameObject ratKing;
    private Brain bossBrain;


    private void Start()
    {
        bossBrain = ratKing.GetComponent<Brain>();
        StartCoroutine(WaveWait());
    }



    private void Update()
    {
        if (waveReady) WaveLaunch();
       if(isConstant) ConstantLaunch();

       
    }
    public void WaveLaunch()
    {
        for (int i = 0; i < waveNum; i++)
        {
            LaunchRat();
        }
        waveReady = false;
        StartCoroutine(WaveWait());

    }

    public void ConstantLaunch()
    {
        while (activeCount < maxActive && bossBrain.isAlive) LaunchRat();
    }
    public void LaunchRat()
    {
        
        
        activeCount++;
        do ratIndex = Random.Range(0, ratLaunchStats.Count);
        while (isActiveArray[ratIndex]);
        ToggleActive(ratIndex, true);
        if (activeRatsIndex == 24) activeRatsIndex = 0;
        else activeRatsIndex++;
        activeRats[activeRatsIndex] = (GameObject)Instantiate(ratPrefab, transform);
        activeRats[activeRatsIndex].GetComponentInChildren<TreeRatBehaviour>().CreateRat(ratLaunchStats[ratIndex], gameObject, ratIndex);
        

    }

    public IEnumerator WaveWait()
    {
        yield return new WaitForSeconds(waveTime);
        waveReady = true;
    }

    public void ToggleActive(int index, bool a)
    {
        int sideCases = 8;
        if(index < sideCases || index > isActiveArray.Length - sideCases)
        {
            isActiveArray[index] = a;
            isActiveArray[isActiveArray.Length - 1 - index] = a;
        }
        else isActiveArray[index] = a;
    }

    public void RemoveActive(int index)
    {
        ToggleActive(index, false);
        activeCount--;
    }

}
