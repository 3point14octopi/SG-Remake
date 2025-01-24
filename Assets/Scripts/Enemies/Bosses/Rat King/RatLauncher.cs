using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatLauncher : MonoBehaviour
{
    public GameObject ratPrefab;
    private GameObject activeRat;
    private bool isActive = false;

    public int direction;

    private void Update()
    {
        direction = Random.Range(0, 2);
        LaunchRat();
    }

    public void LaunchRat()
    {
        if (!isActive) {
            //isActive = true;
            activeRat = (GameObject)Instantiate(ratPrefab, transform);
            activeRat.GetComponent<TreeRatBehaviour>().CreateRat(direction, gameObject);
        }

    }

    public void RemoveActive()
    {
        isActive = false;
    }
}
