using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLaternB : MonoBehaviour
{
    public Vector2 roomCenter;
    public float roamSpeed; //roam speed
    public float roamDamage; //roam damage
    private float xMovement = -1;
    private float yMovement = -1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x - roomCenter.x > 11) { xMovement = -1; }

        else if (transform.position.x - roomCenter.x < -11) { xMovement = 1; }

        else if (transform.position.y - roomCenter.y > 6) { yMovement = -1; }

        else if (transform.position.y - roomCenter.y < -6) { yMovement = 1; }

        transform.position += new Vector3(xMovement, yMovement, 0) * Time.deltaTime * roamSpeed;
        
    }

    void OnCollisionEnter2D(Collision2D other){
        // checks if it is a player bullet hitting themself
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<FbStateManager>().TakeDamage(roamDamage);
        }    
    }
}
