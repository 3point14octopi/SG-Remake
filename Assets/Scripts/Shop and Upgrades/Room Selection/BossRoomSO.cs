using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BossRoom", menuName = "ScriptableObjects/BossRooms")]
public class BossRoomSO : ScriptableObject
{
    public string bossName;
    public string sceneName;
    public int level;
    public Sprite signPic;
}
