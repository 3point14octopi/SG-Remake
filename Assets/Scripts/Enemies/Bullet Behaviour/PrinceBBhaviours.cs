using System;
using UnityEngine;

public class PrinceArc : Arc
{
    public override void BulletUpdate(BulletBehaviour bullet)
    {
        Movement(bullet);

        if(lifetime >= 810)
        {
            bullet.transform.eulerAngles = new Vector3(bullet.transform.eulerAngles.x, bullet.transform.eulerAngles.y, (Mathf.Atan2(bullet.target.transform.position.y - bullet.gameObject.transform.position.y, bullet.target.transform.position.x - bullet.gameObject.transform.position.x) * Mathf.Rad2Deg - 90f));
            bullet.AssignBehaviour(BulletStyles.Straight);
        }
    }
}

public class PrinceFakeout : Arc
{
    public override void BulletUpdate(BulletBehaviour bullet)
    {
        Movement(bullet);
        
        if (lifetime >= 720)
        {
            bullet.AssignBehaviour(BulletStyles.Spinning);
            bullet.bSpeed *= 1.25f;
        }
    }
}

public class RippleArc : Arc
{
    int ripples = 0;
    public override void BulletUpdate(BulletBehaviour bullet)
    {
        Movement(bullet);
        if(lifetime >= 360)
        {
            ogRot += 135;
            float x = lifetime / (float)(65 * bullet.bSpeed);
            x *= (float)Math.PI;
            x *= 2;
            Debug.Log(x.ToString());
            lifetime = 0;

            ripples++;

            if (ripples > 8) bullet.Kill();
        }
    }
}