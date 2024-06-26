using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyVinesBehaviour : MonoBehaviour
{
    private bool alive = false;
    public float damage = 1; //damage it deals to player
    public Animator anim;
    public float witherDuration;
    private Coroutine witherRoutine;

    // Start is called before the first frame update
    void OnEnable()
    {
        anim.Play("Vine Spawn");
        alive = true;
    }


    public IEnumerator WitherRoutine(){

        anim.Play("Vine Wither");
        alive = false;
        
        yield return new WaitForSeconds(witherDuration);

        gameObject.SetActive(false);
        witherRoutine = null;

    }

    public void Wither(){
        if(witherRoutine != null){
            StopCoroutine(witherRoutine);
        }

        witherRoutine = StartCoroutine(WitherRoutine());
    }



}
