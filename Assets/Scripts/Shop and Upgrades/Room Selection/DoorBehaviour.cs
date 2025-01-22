using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorBehaviour : MonoBehaviour
{
    private BossRoomSO boss;
    public SpriteRenderer sign;
    public bool shopPortal = false;
    
    public void InstantiateBossDoor(BossRoomSO a)
    {
        boss = a;
        sign.sprite = boss.signPic;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!shopPortal) SceneManager.LoadScene(boss.sceneName);
            else SceneManager.LoadScene("Shop");
        }
    }

}
