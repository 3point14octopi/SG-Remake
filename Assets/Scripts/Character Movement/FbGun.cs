using EntityStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeStats;

public class FbGun : GunModule
{
    //[HideInInspector]
    public Animator anim;
    public int lastShot = 4;

    public List<KeyCode> shootKeys = new List<KeyCode>();//used for tracking the offsets, matches up with an array
    public Stack<int> keyHistory = new Stack<int>();
    public List<int> reflectedHistory = new List<int>();

    //[HideInInspector]
    public Transform[] launchOffset = new Transform[4]; //the offsets for each direction of shooting

    public bool testActive = true;

    public int testKeyAmount = 0;

    public float rawDamage;

    private void Start()
    {
        rawDamage = ammoList[0].bullet.bulletEffects[(int)EntityStat.Health].modifier;
    }
    private void Update()
    {
        for(int i = 0; i < shootKeys.Count; i++) 
        {
            if (Input.GetKeyDown(shootKeys[i])){
                keyHistory.Push(i);
                reflectedHistory.Add(keyHistory.Peek());
                break;
            }
        }
        

    }
    //Is called from states that can shoot, checks for key presses and shoots using the most recent key pressed
    public void CanShoot()
    {

        if (keyHistory.Count > 0 && active)
        {
            anim.SetBool("Throwing", true);

            StartCoroutine(Shooting());
        }
        else if(keyHistory.Count == 0)
        {
            lastShot = 4; 
            anim.SetBool("Throwing", false);
        }
    }

    //will end up called our Frostbite shooting function inside of gun module
    IEnumerator Shooting()
    {
        active = false;
        testActive = false;
        //0:Up 1:Left 2:Down 3:Right
        for (int i = 0; i < keyHistory.Count; ++i)
        {

            if (Input.GetKey(shootKeys[keyHistory.Peek()]))
            {
                if (lastShot != keyHistory.Peek()) GunAnimation(keyHistory.Peek());
                StartCoroutine(FbShoot(currentAmmo, launchOffset[keyHistory.Peek()].position, launchOffset[keyHistory.Peek()].rotation.eulerAngles));
                yield return new WaitForSeconds(currentAmmo.bullet.firerate);

            }
            else if(!Input.GetKey(shootKeys[keyHistory.Peek()]))
            {
                keyHistory.Pop();
                reflectedHistory.RemoveAt(reflectedHistory.Count - 1);
            }
        }
        active =true;
        testActive = true;
    }

    //Used by the ice drill state to launch the ice drill
    public void IceDrill(int i)
    {
        StartCoroutine(FbShoot(ammoList[1], launchOffset[i].position, launchOffset[i].rotation.eulerAngles));
    }

    //called by our upgrade manager, can upgrade most of the stats on our ammo type
    public void GunUpgrade(GunUpgrade upgrade)
    {
        switch (upgrade.gunUpgrade)
        {
            case GunUpgrades.Damage:

                rawDamage += upgrade.modifier;
                currentAmmo.bullet.bulletEffects[(int)EntityStat.Health] = new HitEffect(EntityStat.Health, currentAmmo.bullet.bulletEffects[(int)EntityStat.Health].modifier + upgrade.modifier);               
                break;
            

            case GunUpgrades.Speed:
            
                currentAmmo.bullet.speed += upgrade.modifier;
                break;
            

            case GunUpgrades.Size:
            
                currentAmmo.bullet.size += ((float)upgrade.modifier / 10);
                break;
            

            case GunUpgrades.Firerate:
            
                currentAmmo.bullet.firerate += upgrade.modifier;
                break;

            case GunUpgrades.Lifespan:

                currentAmmo.bullet.lifeSpan += upgrade.modifier;
                break;


            case GunUpgrades.Rebound:
                
                currentAmmo.bullet.rebound += upgrade.modifier;
                break;
                

            case GunUpgrades.Spread:
                
                currentAmmo.bullet.spreadNum += upgrade.modifier;
                CalculateShooting(currentAmmo.bullet);
                break;
                

            case GunUpgrades.Burst:
                
                currentAmmo.bullet.burstNum += upgrade.modifier;
                CalculateShooting(currentAmmo.bullet);
                break;

            case GunUpgrades.NoRiskNoReward:

                currentAmmo.bullet.bulletEffects[(int)EntityStat.Health] = new HitEffect(EntityStat.Health, currentAmmo.bullet.bulletEffects[(int)EntityStat.Health].modifier * 3);

                break;
        }
    }

    private void GunAnimation(int direction)
    {
        if (direction == 0) { anim.Play("FrostbiteThrowUp"); }
        else if (direction == 1) { anim.Play("FrostbiteThrowLeft"); }
        else if (direction == 2) { anim.Play("FrostbiteThrowDown"); }
        else if (direction == 3) { anim.Play("FrostbiteThrowRight"); }
        lastShot = direction;
    }

}
