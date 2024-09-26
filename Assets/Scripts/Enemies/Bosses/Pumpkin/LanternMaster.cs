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

        if (Input.GetKeyDown(KeyCode.X))
        {
            for(int i = 0;i < lanterns.Length; i++)
            {
                //sync lantern arrival time by messing with their speeds
                float syncSpeed = Vector2.Distance(lanterns[i].transform.position, transform.position) / 3;

                lanterns[i].StartCoroutine(lanterns[i].SpinBlast(syncSpeed));
            }
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

    IEnumerator SpinLantern(GameObject lantern)
    {
        yield return Shmoove(lantern);
        //optimize
        lantern.GetComponent<LanternController>().StartCoroutine(lantern.GetComponent<LanternController>().Blast());
        yield return Shmrotate(lantern);
    }

    IEnumerator Shmoove(GameObject gob)
    {
        Vector3 target = gob.transform.parent.position;
        target.z = gob.transform.position.z;

        while(gob.transform.position != target)
        {
            gob.transform.position = Vector3.MoveTowards(gob.transform.position, target, speed * Time.deltaTime);
            yield return new WaitForSeconds(0);
        }
        Debug.Log("done moving");
    }

    IEnumerator Shmrotate(GameObject gob)
    {
        Vector3 gobRot = new Vector3(0, 0, 2);
        for(int i = 0; i < 120; i++)
        {
            gob.transform.Rotate(gobRot);
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(1.5f);
        gob.transform.Rotate(new Vector3(0, 0, 120));
    }
}
