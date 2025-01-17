using EntityStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornBush : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    public string DropAcornAnimation;
    private int bushHits = 0;
    private bool acornIsDropped = false;

    public GameObject Acorn;
    public GameObject Squirrel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("hit collider");
        if (collision.gameObject.tag == "PlayerBullet" && acornIsDropped == false)
        {

            //Debug.Log("Identified as bullet");
            if (bushHits < 4)
            {
                anim.Play("Bush_Shake");
                bushHits++;
            }
            else
            {
                anim.Play(DropAcornAnimation);
                Squirrel.GetComponent<SquirrelPhases>().GoToAcorn(Acorn);
            }

        }
    }
}
