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

    public Animator anim;
    public GameObject healthbar;
    public GameObject iceBar;

    [Header("Keybinds")]
    public KeyCode[] shootKeys = new KeyCode[4];//used for tracking the offsets, matches up with an array
    public Stack<int> keyHistory = new Stack<int>();
    public KeyCode shootUpKey = KeyCode.UpArrow; //for shooting weapon
    public KeyCode shootLeftKey = KeyCode.LeftArrow; 
    public KeyCode shootDownKey = KeyCode.DownArrow; 
    public KeyCode shootRightKey = KeyCode.RightArrow; 
    public KeyCode iceBlockKey = KeyCode.LeftShift;//for ice block power


    [Header("\nPlayer Stats")]
    public float health = 10;//current
    public float maxHealth = 10;//max health
    public float movementSpeed = 10;//run speed

    public int iceBlockHP = 5;//hits on the ice block
    public float iceBlockHealRate = 2f;//time it tkes for the iceblock to go back a level
    public float iceBlockTimer = 0;

    [Header("\nRunning")]
    public Vector2 movement;
    public Rigidbody2D rb; //player rigidbody

    [Header("\nShooting")]
    public float firerate;//firerate
    public float gunTimer;//keeps track of firerate
    public GameObject bulletPrefab; //bullet prefad
    
    [Header("\nBullet")]
    public int direction = 0; //if the bullet is up, down, left or right
    public Transform[] launchOffset = new Transform[4]; //the offsets for each direction of shooting
    public int damage = 5; // bullet damage
    public float speed = 8; //bullet speed

   [Header("Flash Hit")]
    public Material flash;
    private Material material;
    public float flashDuration;
    private Coroutine flashRoutine;


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

        //grabs our material for flash effect
        material = gameObject.GetComponent<SpriteRenderer>().material;
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

        //Manages the iceblock timer
        if(iceBlockTimer <= 0){
            if(iceBlockHP < 5 && currentState != IceblockState){
                iceBlockHP++;
                iceBar.GetComponent<FBIceBar>().IceBar(iceBlockHP);
                iceBlockTimer = iceBlockHealRate;
            }
        }else{iceBlockTimer -= Time.deltaTime;}
    }

    void FixedUpdate(){
        currentState.FixedUpdateState(this);
    }

    void OnCollisionEnter2D(Collision2D other){
        //Offloads collision data to the state
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

        for (int i = 1; i <= keyHistory.Count; ++i) {
            //Inits the bullet then sets a bunch of its variables 
            if (Input.GetKey(shootKeys[keyHistory.Peek()])){
                var bullet = (GameObject)Instantiate(bulletPrefab, launchOffset[keyHistory.Peek()].position, launchOffset[keyHistory.Peek()].rotation);
                bullet.GetComponent<PlayerBulletBehaviour>().bSpeed = speed;
                bullet.GetComponent<PlayerBulletBehaviour>().bDamage = damage;

                if(keyHistory.Peek() == 0){anim.Play("FrostbiteThrowUp");} 
                else if(keyHistory.Peek() == 1){anim.Play("FrostbiteThrowDown");}     
                else if(keyHistory.Peek() == 2){anim.Play("FrostbiteThrowLeft");}     
                else if(keyHistory.Peek() == 3){anim.Play("FrostbiteThrowRight");}    
                break; 
            }
            else{
                keyHistory.Pop();
            }
        }
    }

    public IEnumerator FlashRoutine(){

        gameObject.GetComponent<SpriteRenderer>().material = flash;
        
        yield return new WaitForSeconds(flashDuration);

        gameObject.GetComponent<SpriteRenderer>().material = material;

        flashRoutine = null;

    }

    public void Flash(){
        if(flashRoutine != null){
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }
}
