using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbStateManager : MonoBehaviour
{


    //Catalog of all states
    public FbIdleState IdleState = new FbIdleState();
    public FbMoveShootState MoveShootState = new FbMoveShootState();

    public FbMoveState MoveState = new FbMoveState();
    public FbShootingState ShootState = new FbShootingState();

    public FbIceblockState IceblockState = new FbIceblockState();
    public FbDeathState DeathState = new FbDeathState();

    //Keeps track of our current state.
    public FbBaseState currentState;


    [Header("Keybinds")]
    public KeyCode[] shootKeys = new KeyCode[4];//used for tracking the offsets, matches up with an array
    public KeyCode shootUpKey = KeyCode.UpArrow; //for shooting weapon
    public KeyCode shootLeftKey = KeyCode.LeftArrow; 
    public KeyCode shootDownKey = KeyCode.DownArrow; 
    public KeyCode shootRightKey = KeyCode.RightArrow; 

    public KeyCode iceBlockKey = KeyCode.LeftShift;//for ice block power


    [Header("\nPlayer Stats")]
    public float health = 100;//current
    public float maxHealth = 100;//max health

    [Header("\nRunning")]
    public float movementSpeed = 10;//run speed
    
    public Vector2 movement;
    public Rigidbody2D rb; //player rigidbody

    [Header("\nShooting")]
    public float firerate;//firerate
    public float gunTimer;//keeps track of firerate
    public GameObject bulletPrefab; //bullet prefad
    
    [Header("\nBullet")]
    public int direction = 0; //if the bullet is up, down, left or right
    public Transform[] launchOffset = new Transform[4]; //the offsets for each direction of shooting
    public float damage = 30; // bullet damage
    public float speed = 5; //bullet speed


    // Start is called before the first frame update
    void Start()
    {
        //sets our rigidbody
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        //Sets our current state to the most boring state we own
        currentState = IdleState;

        //Calls the enter state function of the current state.
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        //tracks WASD
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        currentState.UpdateState(this);

        //Manages the gun timer
        gunTimer -= Time.deltaTime;

        if(health <= 0){
            currentState = DeathState;
        }
    }

    void FixedUpdate(){
        currentState.FixedUpdateState(this);
    }

    void OnCollisionEnter2D(Collision2D other){
        //bases collision data to the state
        currentState.Collision(this, other);
    }

    //is called when a transition condition in our current states update is met
    public void SwitchState(FbBaseState state){
        //switches to the correct state and calls its enter function
        currentState = state;
        state.EnterState(this);
    }

    //called by the two shooting states
    public void Shooting(){
        //Inits the bullet then sets a bunch of its variables    
        var bullet = (GameObject)Instantiate(bulletPrefab, launchOffset[direction].position, launchOffset[direction].rotation);
        bullet.GetComponent<PlayerBulletBehaviour>().bSpeed = speed;
        bullet.GetComponent<PlayerBulletBehaviour>().bDamage = damage;     
    }
}
