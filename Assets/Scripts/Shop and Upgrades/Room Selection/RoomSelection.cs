using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSelection : MonoBehaviour
{
    //a list of each 
    public int bossLevel;
    public List<BossRoomSO> level1Bosses;
    public List<BossRoomSO> level2Bosses;
    public List<BossRoomSO> level3Bosses;
    public List<BossRoomSO> level4Bosses;
    public List<BossRoomSO> level5Bosses;

    public GameObject leftDoor;
    public GameObject rightDoor;

    void Start()
    {

        DecideBosses();
    }
    
    
    public void DecideBosses()
    {
        List<BossRoomSO> selectedBosses = new List<BossRoomSO>();
        List<BossRoomSO> bossList = GetBossListByLevel(bossLevel);

        if (bossList != null && bossList.Count >= 2)
        {
            int firstIndex = Random.Range(0, bossList.Count);
            int secondIndex;
            do
            {
                secondIndex = Random.Range(0, bossList.Count);
            } while (secondIndex == firstIndex);

            leftDoor.GetComponent<DoorBehaviour>().InstantiateBossDoor(bossList[firstIndex]);
            rightDoor.GetComponent<DoorBehaviour>().InstantiateBossDoor(bossList[secondIndex]);
        }
    }

    private List<BossRoomSO> GetBossListByLevel(int level)
    {
        switch (level)
        {
            case 1:
                return level1Bosses;
            case 2:
                return level2Bosses;
            case 3:
                return level3Bosses;
            case 4:
                return level4Bosses;
            case 5:
                return level5Bosses;
            default:
                return null;
        }
    }
}
