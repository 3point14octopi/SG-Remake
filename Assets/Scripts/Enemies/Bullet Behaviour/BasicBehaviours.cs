using System;
using UnityEngine;

public interface BBhaviour
{
    void BulletUpdate(BulletBehaviour bullet);
}

public class Straight : BBhaviour
{
    public void BulletUpdate(BulletBehaviour bullet)
    {
        bullet.transform.position += bullet.transform.up * Time.deltaTime * bullet.bSpeed;
    }
}

public class Homing : BBhaviour
{
    public void BulletUpdate(BulletBehaviour bullet)
    {
        bullet.transform.up = bullet.target.transform.position - bullet.transform.position;
        bullet.transform.position += bullet.transform.up * Time.deltaTime * bullet.bSpeed;
    }
}

public class Twirl : BBhaviour
{
    public void BulletUpdate(BulletBehaviour bullet)
    {
        bullet.transform.Rotate(0, 0, bullet.bArc * Time.deltaTime);
        bullet.transform.position += bullet.transform.up * Time.deltaTime * bullet.bSpeed + (bullet.outward * Time.deltaTime * bullet.bSpeed); 
    }
}

public class Arc : BBhaviour
{
    protected float ogRot = 0f;
    protected bool first = true;
    protected float lifetime = 0f;

    protected void Movement(BulletBehaviour bullet)
    {
        if (first)
        {
            ogRot = bullet.transform.eulerAngles.z + 0f;
            first = false;
        }

        bullet.transform.eulerAngles = new Vector3(bullet.transform.rotation.x, bullet.transform.rotation.y, ogRot + lifetime);
        lifetime += (Mathf.Sin(Time.deltaTime * (float)Math.PI) * 65);
        bullet.transform.position += bullet.transform.up * Time.deltaTime * bullet.bSpeed;
    }

    public virtual void BulletUpdate(BulletBehaviour bullet)
    {
        Movement(bullet);
    }
}


