using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshInhabitant : MonoBehaviour
{
    public Transform tform;
    public Rigidbody2D body;

    public bool activeInSwarm = true;
    bool targetReached = false;
    Vector2 target;
    float homingSpeed = 1f;

    void Awake()
    {
        tform = transform;
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Homing();
        if (!activeInSwarm)
        {
            tform.position = Vector2.MoveTowards(Pos(), target, homingSpeed * Time.deltaTime);
        }
    }


    public Vector2 Pos()
    {
        return tform.position;
    }

    public void MoveTo(Vector3 pos)
    {
        tform.position = pos;
    }

    public Vector2 Vel()
    {
        return body.velocity;
    }

    public void ClampSpeed(float speed)
    {
        if(Vel().magnitude > speed)
        {
            body.velocity = Vector2.ClampMagnitude(Vel(), speed);
        }
    }

    public void SetHomingTarget(Vector2 targ, float speed = 1f)
    {
        target = targ;
        activeInSwarm = false;
        homingSpeed = speed;
    }

    private void Homing()
    {
        if (!activeInSwarm && Vector2.Distance(Pos(), target) < 0.4f)
        {
            if (!targetReached)
            {
                targetReached = true;
                target = transform.parent.position;
            }
            else
            {
                activeInSwarm = true;
                targetReached = false;
            }
        }
    }

}
