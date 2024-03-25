using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLaternB : MonoBehaviour
{
    public float roamSpeed; //roam speed
    public float roamDamage; //roam damage
    private float xMovement = -1;
    private float yMovement = -1;
    private float[] rayCasts = new float[4];
    private LayerMask barrierMask; //layer reference for trees  private LayerMask barrierMask; //layer reference for trees

    public float xflipTimer;
    public float yflipTimer;
    public float flipRate = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        barrierMask |= 0x1 << 7;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(xMovement, yMovement, 0) * Time.deltaTime * roamSpeed;
        
        //Manages the gun timer
        xflipTimer += Time.deltaTime;
        yflipTimer += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D other){
        // checks if it is a player bullet hitting themself
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<FbStateManager>().health = other.gameObject.GetComponent<FbStateManager>().health - roamDamage;

        }
        //anything else the bullet explodes
        else if(other.gameObject.tag == "Barrier"){
            
            rayCasts[0] = Physics2D.Raycast(transform.position, transform.up, 20, barrierMask).distance; 
            rayCasts[1] = Physics2D.Raycast(transform.position, -transform.up, 20, barrierMask).distance;
            rayCasts[2] = Physics2D.Raycast(transform.position, -transform.right, 20, barrierMask).distance;
            rayCasts[3] = Physics2D.Raycast(transform.position, transform.right, 20, barrierMask).distance;  
            
            var minValue = Mathf.Min(rayCasts);

            if((rayCasts[0] ==  minValue || rayCasts[1] ==  minValue) && yflipTimer > flipRate){
                yMovement = yMovement * -1;
                yflipTimer = 0f;
            }
            else if((rayCasts[2] ==  minValue || rayCasts[3] ==  minValue) && xflipTimer > flipRate){
                xMovement = xMovement * -1;
                xflipTimer = 0f;
            }
        }      
    }
}
