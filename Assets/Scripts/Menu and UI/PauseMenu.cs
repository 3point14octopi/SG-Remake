using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused){
                Resume();
            } else {
                Pause();
            }
        }
    }

    void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }
    
    public void Resume(){
        Debug.Log("BOOP");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        paused = false;
    }


}
