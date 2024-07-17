using EntityStats;
using UnityEngine;
using System.Collections;
using System;
using JAFprocedural;

public class PrinceController : MonoBehaviour
{
    public AudioClip badjoke;
    Brain princeBrain;
    GunModule shootyMcShootface;
    bool phase1 = true;

    private void OnEnable()
    {
        //if(shootyMcShootface != null)
        //{
        //    GetComponent<AudioSource>().PlayOneShot(badjoke);
        //    cooldown = 5f; 
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        princeBrain = GetComponent<Brain>();
        shootyMcShootface = GetComponent<GunModule>();

        Debug.Log("funny fire attack from start");
        GetComponent<AudioSource>().PlayOneShot(badjoke);
        StartCoroutine(FunnyFireAttack(true));
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// EDIT THIS to be a state machine
    /// </summary>
    public void CheckHealthState()
    {
        if (princeBrain.currentStats[(int)EntityStat.Health] <= 500) phase1 = false;
    }

    IEnumerator FunnyFireAttack(bool startpause = false)
    {
        if(startpause)yield return new WaitForSeconds(2.5f);
        while(phase1)
        {
            int index = (RNG.GenRand(0, 5) > 3) ? 1 : 0;
            yield return shootyMcShootface.PresetShoot(shootyMcShootface.ammoList[index]);
            yield return new WaitForSeconds(5);
        }
        Debug.Log("done funny fire attacking");
    }

    IEnumerator Ripple()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return shootyMcShootface.PresetShoot(shootyMcShootface.ammoList[1]);
            yield return new WaitForSeconds((float)(Math.PI/2));
        }

        
    }
}
