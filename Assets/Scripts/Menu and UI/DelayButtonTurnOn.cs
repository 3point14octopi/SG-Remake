using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DelayButtonTurnOn : MonoBehaviour
{ 
    public float delayTime;
    public GameObject button;
    private bool inProgress = false;

    void Update(){
        if(!button.activeSelf && !inProgress){
            StartCoroutine(TurnOnButton());
            inProgress = true;
            Debug.Log(button.activeSelf);
        }
    }

    IEnumerator TurnOnButton()
    {
        Debug.Log("YIPPEE");
        yield return new WaitForSeconds(delayTime);
        Debug.Log("BOOM");
        button.SetActive(true);
        inProgress = false;
    }
}
