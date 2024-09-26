using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeStats;

public class FbStateManager : MonoBehaviour
{

    public FbBrain b;
    public FbGun g;

    //Catalog of all states
    public FbIdleState IdleState = new FbIdleState();
    public FbMoveState MoveState = new FbMoveState();


    public FbStunState StunState = new FbStunState();
    public FbDeathState DeathState = new FbDeathState();

    [Header("\nIce states and variables")]
    public FbIceblockState IceBlockState = new FbIceblockState();
    public FbIceItemState IceItemState = new FbIceItemState();
    public FbIceTimeState IceTimeState = new FbIceTimeState();
    public FbIceDrillState IceDrillState = new FbIceDrillState();


    //Keeps track of our current state.
    public FbBaseState currentState;
    public FbBaseState currentIceState;





    public KeyCode iceBlockKey = KeyCode.LeftShift;//for ice block power



    public GameObject iceWall;
    private GameObject ice;
    private GameObject temp;
    public GameObject indicator; //used to show where an object will be placed
    public GameObject freezeScreen; //used to show that time is slow
    public float currentIceUses = 5;
    public float maxIceUses = 5;
    public int extraUseUpgrades = 0;

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
        if(currentIceState == IceBlockState)
        {
            if(iceBlockTimer <= 0){
                if(currentIceUses < maxIceUses && currentState != IceBlockState){
                    currentIceUses++;
                    b.iceBar.GetComponent<FBIceBar>().IceBar(Mathf.FloorToInt(currentIceUses));
                    iceBlockTimer = iceBlockHealRate;
                }
            }else{iceBlockTimer -= Time.deltaTime;}
        }
        if(currentIceState == IceTimeState)
        {
            if(currentIceUses < maxIceUses)
            {
                currentIceUses += Time.deltaTime / 6f;
            }
        }
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

    //will change what the current ice state is based on our recieved upgrade
    public void IceUpgrade(IceUpgrade upgrade)
    {
        switch (upgrade.iceUpgrade)
        {
            case IceUpgrades.Block:
            
                maxIceUses = 5 + 2 * extraUseUpgrades;
                currentIceUses = 5;
                currentIceState = IceBlockState;
                break;
            
            case IceUpgrades.Wall:
            
                maxIceUses = 2 + extraUseUpgrades;
                currentIceUses = 2;
                ice = iceWall; //this is what we place down
                currentIceState = IceItemState;
                break;

            case IceUpgrades.Time:

                maxIceUses = 2;
                currentIceUses = 2;
                currentIceState = IceTimeState;
                break;

            case IceUpgrades.Drill:

                maxIceUses = 1;
                currentIceUses = 1;
                currentIceState = IceDrillState;
                break;

            //case IceUpgrades.Teleport:

            //    maxIceUses = 1;
            //    currentIceUses = 1;
            //    ice = iceTeleport;
            //    currentIceState = IceTeleportState;
            //    break;

            case IceUpgrades.AbilityUses:
                extraUseUpgrades++;
                if (currentIceState == IceBlockState)
                {
                    maxIceUses = 5 + 2 * extraUseUpgrades;
                    currentIceUses += 2;
                }
                else if (currentIceState == IceItemState)
                {
                    maxIceUses = 2 + extraUseUpgrades;
                    currentIceUses += 2;
                }
                break;
        }
    }

    //moves our indicator around to check if we are capable of placing an object
    public void Preplace(int direction)
    {
        float margin = 0.6f;
        switch(direction)
        {
            case 0:
                {
                    indicator.transform.position = new Vector3(Mathf.FloorToInt((float)gameObject.transform.position.x) + 0.5f, Mathf.FloorToInt((float)gameObject.transform.position.y) + 0.5f, 0) + new Vector3(0, 1,0);
                    if(Mathf.Abs(indicator.transform.position.y - gameObject.transform.position.y) < margin + 0.15f)
                    {
                        indicator.transform.position += new Vector3(0, 1, 0);
                    }
                    break;
                }
            case 1:
                {
                    indicator.transform.position = new Vector3(Mathf.FloorToInt((float)gameObject.transform.position.x) + 0.5f, Mathf.FloorToInt((float)gameObject.transform.position.y) + 0.5f, 0) + new Vector3(-1, 0, 0);
                    if (Mathf.Abs(indicator.transform.position.x - gameObject.transform.position.x) < margin)
                    {
                        indicator.transform.position += new Vector3(-1, 0, 0);
                    }
                    break;
                }
            case 2:
                {
                    indicator.transform.position = new Vector3(Mathf.FloorToInt((float)gameObject.transform.position.x) + 0.5f, Mathf.FloorToInt((float)gameObject.transform.position.y) + 0.5f, 0) + new Vector3(0, -1, 0);
                    if (Mathf.Abs(indicator.transform.position.y - gameObject.transform.position.y) < margin + 0.15f)
                    {
                        indicator.transform.position += new Vector3(0, -1, 0);
                    }
                    break;
                }
            case 3:
                {
                    indicator.transform.position = new Vector3(Mathf.FloorToInt((float)gameObject.transform.position.x) + 0.5f, Mathf.FloorToInt((float)gameObject.transform.position.y) + 0.5f, 0) + new Vector3(1, 0, 0);
                    if (Mathf.Abs(indicator.transform.position.x - gameObject.transform.position.x) < margin)
                    {
                        indicator.transform.position += new Vector3(1, 0, 0);
                    }
                    break;
                }
        }
    }

    //Used by our icewall state to place a wall down if the tile in the direction we are facing is empty
    public void PlaceItem()
    {
        if (currentIceUses > 0)
        {
            if (indicator.GetComponent<Indicator>().canPlace)
            {
                temp = (GameObject)Instantiate(ice, indicator.transform.position, Quaternion.Euler(0, 0, 0));
                if(temp.transform.GetChild(0).GetComponent<IceWallBehaviour>() != null)
                {
                    temp.transform.GetChild(0).GetComponent<IceWallBehaviour>().InstantiateWall(5, gameObject);
                }
                currentIceUses--;
            }
        }
    }
    public IEnumerator Stunned(float time)
    {
        currentState = StunState;
        yield return new WaitForSeconds(time);
        if (b.isAlive)
        {
            currentState = IdleState;
        }
    }
}
