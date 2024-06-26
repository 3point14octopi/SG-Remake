using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanShooterController : MonoBehaviour
{
    Animator anim;
    Brain beanBrain;
    GunModule gun;

    private void OnEnable()
    {
        if(anim != null)
        {
            anim.SetBool("Death", false);
            gun.ToggleAutomatic(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        beanBrain = GetComponent<Brain>();
        gun = GetComponent<GunModule>();
    }


    private void Update()
    {
        if (beanBrain.isAlive)
        {
            if (gun.automaticShotThisFrame)
            {
                anim.Play("Bean Shooter Shoot");
                //can also do audio here if you want
                gun.automaticShotThisFrame = false;
            }
        }
    }
}
