using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAFprocedural;
using Unity.VisualScripting;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class EnemyBulletBehaviour : MonoBehaviour
{
    //all 3 of these things are updated by the person that calls them
    public float bSpeed; //bullet speed
    public float bDamage; //bullet damage
    public int bRebound;
    private Vector3 wallCenter;

     // Update is called once per frame
    void Update()
    {
        //moves the bullet continously in the direction
        transform.position += transform.up * Time.deltaTime * bSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "PlayerBullet" || other.gameObject.tag == "EnemyBullet"){
            
        }
        //anything else the bullet explodes
        else if(other.gameObject.tag == "Player"){
            Destroy(gameObject);
        }     
        else{
            if (bRebound > 0){
                bRebound--;
                transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
            }

            else if(bRebound == 0){Destroy(gameObject);}
        }   
    }
    
    public void SetBullet(Ammo a)
    {

        bSpeed = a.speed;
        bDamage = a.damage;
        bRebound = a.rebound;
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
