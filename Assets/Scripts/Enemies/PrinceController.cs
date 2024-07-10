using EntityStats;
using JAFprocedural;
using UnityEngine;
using System.Collections;

public class PrinceController : MonoBehaviour
{
    Brain princeBrain;
    GunModule shootyMcShootface;
    public float cooldown = 1f;

    // Start is called before the first frame update
    void Start()
    {
        princeBrain = GetComponent<Brain>();
        shootyMcShootface = GetComponent<GunModule>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;

        if(cooldown <= 0)
        {
            StartCoroutine(shootyMcShootface.PresetShoot(shootyMcShootface.ammoList[RNG.GenRand(0, 3)]));
            cooldown = 0.75f * RNG.GenRand(1, 3);
        }
    }

    /// <summary>
    /// EDIT THIS to be a state machine
    /// </summary>
    public void CheckHealthState()
    {
        Debug.Log("health is " + princeBrain.currentStats[(int)EntityStat.Health].ToString());
    }

    private IEnumerator FunnyFireAttack()
    {
        for(int i = 0; i < 3; i++)
        {
            shootyMcShootface.StartCoroutine(shootyMcShootface.PresetShoot(shootyMcShootface.ammoList[i]));
            yield return new WaitForSeconds(0.25f);
        }
    }
}
