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
        anim.Play("VineSpawn");
        alive = true;
    }

    void OnCollisionEnter2D(Collision2D other){
        
        //damages the player if they walk into us
        if (other.gameObject.tag == "Player" && alive == true)
        {
            other.gameObject.GetComponent<FbStateManager>().TakeDamage(damage);
        }
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
