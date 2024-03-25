using JAFprocedural;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public interface BossAttackManager
{
    void Init(BossRoomManager b);
    void LoadMap(BossRoomManager b, Space2D map);
    void Attack1(BossRoomManager b);
    void Attack2(BossRoomManager b);
    void Attack3(BossRoomManager b);
}
public class PumpkinPrinceAttacks : BossAttackManager
{
    public void Init(BossRoomManager b)
    {
        b.princeAttacks.OnStart();
    }

    public void LoadMap(BossRoomManager b, Space2D map)
    {
        b.princeAttacks.room = map;
    }

    public void Attack1(BossRoomManager b)
    {
        b.princeAttacks.FireRain();
    }

    public void Attack2(BossRoomManager b)
    {
        b.princeAttacks.VineWaves();
    }

    public void Attack3(BossRoomManager b)
    {
        b.princeAttacks.HorizontalLasers();
    }
}






public enum BossTypes
{
    PUMPKIN_PRINCE,
    KING_RAT
}







public class BossRoomManager : MonoBehaviour
{
    public static BossRoomManager Instance;

    public Space2D room;
    public PumpkinPrince_AttackManager princeAttacks;


    Prince_Attack currentAttack = Prince_Attack.NONE;
    private BossAttackManager[] behaviours = new BossAttackManager[] { new PumpkinPrinceAttacks() };
    private BossAttackManager currentBehaviour;
    public BossTypes bossType = BossTypes.PUMPKIN_PRINCE;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentBehaviour = behaviours[(int)bossType];
        currentBehaviour.Init(this);
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void LoadMap(Space2D floormap)
    {
        currentBehaviour.LoadMap(this, floormap);
    }

    public void Attack1()
    {
        currentBehaviour.Attack1(this);
    }

    public void Attack2()
    {
        currentBehaviour.Attack2(this);
    }

    public void Attack3()
    {
        currentBehaviour.Attack3(this);
    }
}
