using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorBehaviour : MonoBehaviour
{
    private BossRoomSO boss;
    public SpriteRenderer sign;
    
    public void InstantiateBossDoor(BossRoomSO a)
    {
        boss = a;
        sign.sprite = boss.signPic;
        Debug.Log(name + " was given boss " + boss.bossName);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") SceneManager.LoadScene(boss.sceneName);
    }

}
