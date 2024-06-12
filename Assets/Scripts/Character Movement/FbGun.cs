using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class FbGun : GunModule
{

    [HideInInspector]
    public KeyCode[] shootKeys = new KeyCode[4];//used for tracking the offsets, matches up with an array
    private Stack<int> keyHistory = new Stack<int>();

    [HideInInspector]
    public Transform[] launchOffset = new Transform[4]; //the offsets for each direction of shooting
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < shootKeys.Length; i++)
        {
            if (Input.GetKeyDown(shootKeys[i]))
            {
                keyHistory.Push(i);
            }
        }

        if(keyHistory.Count > 0 && active)
        {
            StartCoroutine(Shooting());
        }
    }

    IEnumerator Shooting()
    {
        active = false;
        //0:Up 1:Down 2:Left 3:Right
        for (int i = 1; i <= keyHistory.Count; ++i)
        {
            //Inits the bullet then sets a bunch of its variables 
            if (Input.GetKey(shootKeys[keyHistory.Peek()]))
            {
                StartCoroutine(FbShoot(currentAmmo, launchOffset[keyHistory.Peek()].position, launchOffset[keyHistory.Peek()].rotation.eulerAngles));

                //if (keyHistory.Peek() == 0) { anim.Play("FrostbiteThrowUp"); }
                //else if (keyHistory.Peek() == 1) { anim.Play("FrostbiteThrowDown"); }
                //else if (keyHistory.Peek() == 2) { anim.Play("FrostbiteThrowLeft"); }
                //else if (keyHistory.Peek() == 3) { anim.Play("FrostbiteThrowRight"); }
                 yield return new WaitForSeconds(currentAmmo.firerate);
                break;
            }
            else
            {
                keyHistory.Pop();
            }
        }
        active =true;
        
    }
}
