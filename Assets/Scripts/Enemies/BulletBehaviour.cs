using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAFprocedural;
using Unity.VisualScripting;
using static UnityEngine.RuleTile.TilingRuleOutput;
using EntityStats;
using System;


public class BulletBehaviour : MonoBehaviour
{
    //all 3 of these things are updated by the person that calls them
    public float bSpeed; //bullet speed
    public float bDamage = 1; //Delete after merge because we use onhit
    public int bRebound;
    private Vector3 wallCenter;

    public List<string> ignoreTags = new List<string>();

    // Update is called once per frame
    void Update()
    {
        //moves the bullet continously in the direction
        transform.position += transform.up * Time.deltaTime * bSpeed;
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
    public void SetBullet(Ammo ammo)
    {

        bSpeed = ammo.speed;
        bRebound = ammo.rebound;

        //transfers on hit data from the ammo type to the bullet object
        for (int i = 0; i < EntityStat.GetNames(typeof(EntityStat)).Length; i++) //transfers on hit data from the ammo type to the bullet object
        {
            gameObject.GetComponent<OnHit>().effects.Add(ammo.bulletEffects[i]);
        }
    }
}
/*
transform.position = Vector3.Reflect(transform.position, Vector3.up);
if (Mathf.Abs(wallCenter.x - transform.position.x) > Mathf.Abs(wallCenter.y - transform.position.y))
{
}
else if (Mathf.Abs(wallCenter.x - transform.position.x) < Mathf.Abs(wallCenter.y - transform.position.y))
{

}
else if (Mathf.Abs(wallCenter.x - transform.position.x) == Mathf.Abs(wallCenter.y - transform.position.y))
{
                Debug.Log("I hit a wall");
                Vector3Int tile = AstarDebugLayer.Instance.renderGrid.WorldToCell(other.rigidbody.ClosestPoint(transform.position));
                wallCenter = AstarDebugLayer.Instance.renderGrid.GetCellCenterWorld(tile);
 
 
 */
