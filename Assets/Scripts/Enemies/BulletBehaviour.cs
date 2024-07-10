using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAFprocedural;
using Unity.VisualScripting;
using static UnityEngine.RuleTile.TilingRuleOutput;
using EntityStats;
using System;
using static UnityEngine.GraphicsBuffer;


public class BulletBehaviour : MonoBehaviour
{
    //all 3 of these things are updated by the person that calls them
    public float bSpeed; //bullet speed
    public int bRebound;
    public float bArc;

    public BulletStyles style;
    private Vector3 wallCenter;
    private GameObject player;
    
    public List<string> ignoreTags = new List<string>();

    private Vector3 outward; //original direction it was fired

    //can remove if this sucks
    private float lifetime = 0f;
    private float ogRot = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (style)
        {
            case BulletStyles.Straight:
                StraightMovement();
                break;
            case BulletStyles.Tracking:
                TrackingMovement();
                break;
            case BulletStyles.Arcing:
                ArchingMovement();
                break;
            case BulletStyles.Spinning:
                SpinningMovement();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ignoreTags.Contains(other.gameObject.tag) || other.gameObject.tag == "PlayerBullet" || other.gameObject.tag == "EnemyBullet"){
            
        }
  
        else{
            if (bRebound > 0){
                bRebound--;
                transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
            }

            else if(bRebound == 0){Destroy(gameObject);}
        }   
    }
    
    //Called by gun when a bullet is instantiated
    public void SetBullet(Bullet bullet)
    {

        bSpeed = bullet.speed;
        bRebound = bullet.rebound;
        bArc = bullet.arcAngle;
        style = bullet.style;
        outward = transform.up;


        ogRot = transform.rotation.z;

        //transfers on hit data from the ammo type to the bullet object
        for (int i = 0; i < EntityStat.GetNames(typeof(EntityStat)).Length; i++) //transfers on hit data from the ammo type to the bullet object
        {
            gameObject.GetComponent<OnHit>().effects.Add(bullet.bulletEffects[i]);
        }
    }

    private void StraightMovement()
    {
        //moves the bullet continously in same direction it was fired
        transform.position += transform.up * Time.deltaTime * bSpeed;

    }

    private void TrackingMovement()
    {
        transform.up = player.transform.position - transform.position;
        transform.position += transform.up * Time.deltaTime * bSpeed;
    }

    private void SpinningMovement()
    {
        transform.Rotate(0, 0, bArc);
        transform.position += transform.up * Time.deltaTime * bSpeed + outward * Time.deltaTime * bSpeed;
    }

    private void ArchingMovement()
    {
        if (lifetime == 0)
        {
            ogRot = transform.eulerAngles.z + 0f;
        }
        
        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, ogRot + lifetime);
        lifetime += (Mathf.Sin(Time.deltaTime * (float)Math.PI) * 20);
        transform.position += transform.up * Time.deltaTime * bSpeed;
    }
}
