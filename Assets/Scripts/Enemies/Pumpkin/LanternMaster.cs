using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class LanternMaster : MonoBehaviour
{
    public GameObject lanternPrefab;
    public float speed = 1f;
    LanternController[] lanterns = new LanternController[2];
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 2; i++)
        {
            lanterns[i] = Instantiate(lanternPrefab, transform, false).GetComponent<LanternController>();
            lanterns[i].transform.Rotate(0, ((i % 2) * 180f), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SendToStartPositions();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Blast();
        }
    }

    private void SendToStartPositions()
    {
        for (int i = 0; i < lanterns.Length; lanterns[i].StartCoroutine(lanterns[i].GoToStart(6, speed)), i++) ;
    }

    private void Blast()
    {
        for (int i = 0; i < lanterns.Length; lanterns[i].StartCoroutine(lanterns[i].Blast()), i++) ;
    }
}
