using System.Collections.Generic;
using UnityEngine;
using EntityStats;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;


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


    private float bLifeSpan = 30;
    private float distance = 0;
    private Vector2 spawnPoint = new Vector2();
    private OnHit onHitComponent;

    [Tooltip("Reference for the animator component")]
    public Animator Animator;
    [Tooltip("Reference for the dissolve animation (if there isn't one leave it blank")]
    public AnimationClip dissolveAnimation;


    /////////////////////////////////////////////////////////////////////////////////////////////////


    private void OnEnable() { 
        target = GameObject.FindGameObjectWithTag("Player");
        onHitComponent = gameObject.GetComponent<OnHit>();
        spawnPoint = transform.position;
    }
    
    void FixedUpdate() { 
        movement.BulletUpdate(this);
        if (distance < bLifeSpan) distance = Vector3.Distance(spawnPoint, gameObject.transform.position);
        else StartCoroutine(Dissolve());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            Debug.Log("Phase 1");
        if (!ignoreTags.Contains(other.gameObject.tag) && other.gameObject.tag != "PlayerBullet" && other.gameObject.tag != "EnemyBullet"){
            Debug.Log("Phase 2");
            //if it isnt a character and has some juice left it will bounce off the wall
            if (bRebound > 0 && other.gameObject.tag != "Player" && other.gameObject.tag != "Enemy"){
                Debug.Log("Phase 3");
                bRebound--;
                transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
            }

            else StartCoroutine(Kill());
        }   
    }
    
    /// <summary>
    /// should happen everytime a bullet is instantiated. Puts its stored variables into action
    /// </summary>
    /// <param name="bullet"></param>
    public void SetBullet(Bullet bullet)
    {

        bSpeed = bullet.speed;
        bRebound = bullet.rebound;
        bArc = bullet.arcAngle;
        gameObject.transform.localScale = new Vector2(bullet.size, bullet.size);
        if(bullet.lifeSpan != -1) bLifeSpan = bullet.lifeSpan;
        AssignBehaviour(bullet.style);
        outward = transform.up;


        //transfers on hit data from the ammo type to the bullet object
        var effectNames = EntityStat.GetNames(typeof(EntityStat)).Length;
        for (int i = 0; i < effectNames; i++) //transfers on hit data from the ammo type to the bullet object
        {
            onHitComponent.effects.Add(bullet.bulletEffects[i]);
        }
    }

    /// <summary>
    /// uses the style variable from a bullet to choose what logic it will use to move
    /// </summary>
    /// <param name="newStyle"></param>
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


    public IEnumerator Kill()
    {
        if (dissolveAnimation) Animator.Play("Dissolve");
        bSpeed = 0.5f;
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    public IEnumerator Dissolve()
    {
        if (dissolveAnimation != null)
        {
            Animator.Play(dissolveAnimation.name);
            yield return new WaitForSeconds(dissolveAnimation.length - 0.2f);
        }
        Destroy(gameObject);
    }
}
