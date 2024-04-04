using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayButtonTurnOn : MonoBehaviour
{ 
    public float delayTime;
    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TurnOnButton());
    }

    IEnumerator TurnOnButton()
    {
        yield return new WaitForSeconds(delayTime);
        button.SetActive(true);
    }
}
