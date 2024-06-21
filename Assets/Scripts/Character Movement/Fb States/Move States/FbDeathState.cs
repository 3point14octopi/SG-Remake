using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FbDeathState : FbBaseState
{
    public override void EnterState(FbStateManager fb){
        
        Debug.Log("DEATH TIME");
        fb.b.anim.Play("FrostbiteDeath");
        fb.b.rb.bodyType = RigidbodyType2D.Kinematic;
        fb.b.iFrame = true;
        fb.StartCoroutine(Death());
    }

    public override void UpdateState(FbStateManager fb){

    }

    public override void FixedUpdateState(FbStateManager fb){
        
    }
    
    public override void Collision(FbStateManager fb, Collision2D Collision2D){

    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(3.5f);
        GameObject.FindGameObjectWithTag("DDOL").GetComponent<DontDestroy>().win = false;
        SceneManager.LoadScene("EndGame");
    }
}
