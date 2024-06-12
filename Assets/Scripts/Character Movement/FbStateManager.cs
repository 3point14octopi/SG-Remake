using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UpgradeStats;

public class FbStateManager : MonoBehaviour
{

    public FbBrain b;
    //Catalog of all states
    public FbIdleState IdleState = new FbIdleState();
    public FbMoveState MoveState = new FbMoveState();


    public FbStunState StunState = new FbStunState();
    public FbDeathState DeathState = new FbDeathState();

    public FbIceblockState IceBlockState = new FbIceblockState();
    //public FbIceblockState IceDecoyState = new FbIceblockState();
    //public FbIceblockState IceWallState = new FbIceblockState();



    //Keeps track of our current state.
    public FbBaseState currentState;
    public FbBaseState currentIceState;





    public KeyCode iceBlockKey = KeyCode.LeftShift;//for ice block power


    public int iceBlockHP = 5;//hits on the ice block
    public float iceBlockHealRate = 2f;//time it tkes for the iceblock to go back a level
    public float iceBlockTimer = 0;







    // Start is called before the first frame update
    void Start()
    {


        //Sets our current state to the most boring state we own
        currentState = IdleState;
        currentIceState = IceBlockState;

        //Calls the enter state function of the current state.
        currentState.EnterState(this);


    }

    // Update is called once per frame
    void Update()
    {

        currentState.UpdateState(this);



        //Manages the iceblock timer
        if(iceBlockTimer <= 0){
            if(iceBlockHP < 5 && currentState != IceBlockState){
                iceBlockHP++;
                b.iceBar.GetComponent<FBIceBar>().IceBar(iceBlockHP);
                iceBlockTimer = iceBlockHealRate;
            }
        }else{iceBlockTimer -= Time.deltaTime;}
    }

    //this is used mostly for the moving function inside of our moving states
    void FixedUpdate(){
        currentState.FixedUpdateState(this);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    //Only state that currently uses this is the Iceblock state which lowers the ice durability on hit
    void OnCollisionEnter2D(Collision2D other){
        currentState.Collision(this, other);
    }



    //is called when a transition condition in our current states update is met
    public void SwitchState(FbBaseState state){
        //switches to the correct state and calls its enter function
        currentState = state;
        state.EnterState(this);
    }







}
