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

    //Keeps track of our current state.
    public FbBaseState currentState;


    [Header("Keybinds")]

    public KeyCode[] shootKeys = new KeyCode[4];
    public KeyCode shootUpKey = KeyCode.UpArrow; //for shooting weapon
    public KeyCode shootLeftKey = KeyCode.LeftArrow; 
    public KeyCode shootDownKey = KeyCode.DownArrow; 
    public KeyCode shootRightKey = KeyCode.RightArrow; 

    public KeyCode iceBlockKey = KeyCode.LeftShift;//for ice block power


    [Header("\nPlayer Stats")]
    public float health = 100;//current
    public float maxHealth = 100;//max health

    [Header("\nRunning")]
    public float movementSpeed;//run speed
    
    public float horizontalInput; //for W && S
    public float verticalInput; //for A && D
    public Rigidbody2D rb; //player rigidbody

    [Header("\nShooting")]
    public float firerate;//firerate
    public float gunTimer;//keeps track of firerate
    public GameObject bulletPrefab;
    
    [Header("\nBullet")]
    public int direction = 0;
    public Transform[] launchOffset = new Transform[4];
    public float damage = 30;
    public float speed = 5;
    public GameObject bullet;


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
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        currentState.UpdateState(this);

        //Manages the gun timer
        gunTimer -= Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D other){
        //currentState.Collision(this, other);
        if (other.gameObject.tag == "Bullet" && other.gameObject.GetComponent<BulletBehaviour>().bPlayer == false)
        {
            health = health - other.gameObject.GetComponent<BulletBehaviour>().bDamage;
            if(health <= 0){
                Destroy(gameObject);
            }
        }

    }

    //is called when a transition condition in our current states update is met
    public void SwitchState(FbBaseState state){
        //switches to the correct state and calls its enter function
        currentState = state;
        state.EnterState(this);
    }

    public void Shooting(){    
        bullet = (GameObject)Instantiate(bulletPrefab, launchOffset[direction].position, launchOffset[direction].rotation);
        bullet.GetComponent<BulletBehaviour>().bSpeed = speed;
        bullet.GetComponent<BulletBehaviour>().bDamage = damage;
        bullet.GetComponent<BulletBehaviour>().bPlayer = true;     
    }
}
