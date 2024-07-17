using System.Collections.Generic;
using UnityEngine;
using EntityStats;
using System;


public class BulletBehaviour : MonoBehaviour
{
    //all 3 of these things are updated by the person that calls them
    public float bSpeed; //bullet speed
    public int bRebound;
    public float bArc;

    public BulletStyles style;
    private BBhaviour movement;
    private Vector3 wallCenter;
    [HideInInspector]public GameObject target;
    
    public List<string> ignoreTags = new List<string>();

    [HideInInspector]public Vector3 outward; //original direction it was fired

    //can remove if this sucks
    public float lifetime = 0f;
    [HideInInspector] float ogRot = 0f;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement.BulletUpdate(this);
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
        AssignBehaviour(bullet.style);
        outward = transform.up;


        ogRot = transform.rotation.z;

        //transfers on hit data from the ammo type to the bullet object
        for (int i = 0; i < EntityStat.GetNames(typeof(EntityStat)).Length; i++) //transfers on hit data from the ammo type to the bullet object
        {
            gameObject.GetComponent<OnHit>().effects.Add(bullet.bulletEffects[i]);
        }
    }

    public void AssignBehaviour(BulletStyles newStyle)
    {
        switch (newStyle)
        {
            case BulletStyles.Straight:
                movement = new Straight();
                break;
            case BulletStyles.Tracking:
                movement = new Homing();
                break;
            case BulletStyles.Arcing:
                movement = new Arc();
                break;
            case BulletStyles.Spinning:
                movement = new Twirl();
                break;
            case BulletStyles.PrinceArcing:
                movement = new PrinceArc();
                break;
            case BulletStyles.PrinceFakeout:
                movement = new PrinceFakeout();
                break;
            case BulletStyles.Ripple:
                movement = new RippleArc();
                break;
            default:
                break;
        }

        style = newStyle;
    }

    private void StraightMovement()
    {
        //moves the bullet continously in same direction it was fired
        transform.position += transform.up * Time.deltaTime * bSpeed;

    }

    private void TrackingMovement()
    {
        transform.up = target.transform.position - transform.position;
        transform.position += transform.up * Time.deltaTime * bSpeed;
    }

    private void SpinningMovement()
    {
        transform.Rotate(0, 0, bArc);
        transform.position += transform.up * Time.deltaTime * bSpeed + outward * Time.deltaTime * bSpeed;
    }

    private void ArchingMovement()
    {
        //if (first)
        //{
        //    ogRot = transform.eulerAngles.z + 0f;
        //    first = false;
        //}
        
        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, ogRot + lifetime);
        lifetime += (Mathf.Sin(Time.deltaTime * (float)Math.PI) * 65);
        if (lifetime >= 1170)
        {
            Debug.Log("full loop");
            style = BulletStyles.Tracking;
        }
        transform.position += transform.up * Time.deltaTime * bSpeed;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
