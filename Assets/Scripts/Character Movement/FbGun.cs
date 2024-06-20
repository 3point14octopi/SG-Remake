using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UpgradeStats;

public class FbGun : GunModule
{
    [HideInInspector]
    public KeyCode[] shootKeys = new KeyCode[4];//used for tracking the offsets, matches up with an array
    public Stack<int> keyHistory = new Stack<int>();

    [HideInInspector]
    public Transform[] launchOffset = new Transform[4]; //the offsets for each direction of shooting


    //Is called from states that can shoot, checks for key presses and shoots using the most recent key pressed
    public void CanShoot()
    {
        for (int i = 0; i < shootKeys.Length; i++)
        {
            if (Input.GetKeyDown(shootKeys[i]))
            {
                keyHistory.Push(i);
            }
        }

        if (keyHistory.Count > 0 && active)
        {
            StartCoroutine(Shooting());
        }
    }

    //will end up called our Frostbite shooting function inside of gun module
    IEnumerator Shooting()
    {
        active = false;
        //0:Up 1:Left 2:Down 3:Right
        for (int i = 1; i <= keyHistory.Count; ++i)
        {

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
    //called by our upgrade manager, can upgrade most of the stats on our ammo type
    public void GunUpgrade(GunUpgrade upgrade)
    {
        switch (upgrade.gunUpgrade)
        {
            case GunUpgrades.Damage:
            {
                currentAmmo.damage += upgrade.modifier;
                break;
            }

            case GunUpgrades.Speed:
            {
                    currentAmmo.damage += upgrade.modifier;
                    break;
            }

            case GunUpgrades.Size:
                {
                    Debug.Log("No size upgrade available");
                    //currentAmmo.size += upgrade.modifier;
                    break;
                }

            case GunUpgrades.Firerate:
                {
                    currentAmmo.firerate += upgrade.modifier;
                    break;
                }

            case GunUpgrades.Rebound:
                {
                    currentAmmo.rebound += upgrade.modifier;
                    break;
                }

            case GunUpgrades.Spread:
                {
                    currentAmmo.spreadNum += upgrade.modifier;
                    break;
                }

            case GunUpgrades.Burst:
                {
                    currentAmmo.burstNum += upgrade.modifier;
                    break;
                }


        }
    }

}
