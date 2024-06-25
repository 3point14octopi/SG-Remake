using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GunModule : MonoBehaviour
{
    public Ammo currentAmmo; //bullet being fired
    public List<Ammo> ammoList = new List<Ammo>();




    public List<float> spreadsAngle = new List<float>();//angle difference when fired multiple bullets
    private Vector3 launchAng;
    public List<float> spreadsDis = new List<float>();//gap between multiple bullets
    private Vector3 launchDis;

    
    
    


    private GameObject player; // player so we can shoot at them
    private GameObject activeBullet; // husk used to init the bullets
    private Vector3 playerAng; // this is where the bullet is launched from

    private bool LOS; //Used to track if we can see the player
    public LayerMask playerMask; //layer reference for player
    private LayerMask barrierMask; //layer reference for trees

    [HideInInspector]
    public bool automatic; //if we should fire constantly
    [HideInInspector]
    public bool targeted;   //if we should fire preset or targeted
    [HideInInspector]
    public bool needLOS; //determines if the gun should only shoot when it has LOS
    protected bool active = true;
    

   // Start is called before the first frame update
   void Start()
   {
       //so we can have the players transform
       player = GameObject.FindWithTag("Player");
       //find our layers used for our raycast masks
       playerMask |= 0x1 << 6;
       barrierMask |= 0x1 << 7;
       CalculateShooting(currentAmmo);
   }

    #region Functions for Changing things

    //called to set up a guns spread distance
    public void CalculateShooting(Ammo a){
        spreadsAngle.Clear();
        spreadsDis.Clear();
        if (targeted){
           spreadsAngle.Add(a.spreadAngle * ((a.spreadNum - 1)/ -2));
           for(int i = 1; i < a.spreadNum; i++ ){
               spreadsAngle.Add(a.spreadAngle *i + spreadsAngle[0]);
           }

           spreadsDis.Add(a.spreadDis *((a.spreadNum - 1)/ -2));
           for(int i = 1; i < a.spreadNum; i++ ){
               spreadsDis.Add(a.spreadDis *i + spreadsDis[0]);
           }

           if(needLOS && automatic){                

               StartCoroutine(LOSTargetShooting());
           }

           else if(!needLOS && automatic){StartCoroutine(TargetShooting());}

       }
       else if(!targeted && automatic){StartCoroutine(PresetShooting());}
   }

    //changes current ammo to be a different ammo type we have stored
    public void SwapAmmo(int i)
    {
        currentAmmo = ammoList[i];
        CalculateShooting(currentAmmo);
    }

    //changes the gun fromautomatic to not. Could mean stopping the coroutines could mean starting one up
    public void ToggleAutomatic(bool b){
        automatic = b;
        if(automatic) {
            if (targeted){
                if (needLOS && automatic){
                    StartCoroutine(LOSTargetShooting());
                }

                else if (!needLOS && automatic) { StartCoroutine(TargetShooting()); }

            }
            else if (!targeted && automatic) { StartCoroutine(PresetShooting()); }
        }
        if (!automatic){
            StopAllCoroutines();
        }
    }
    #endregion

    //this region holds the functions that will not actually spawn a bullet but manage the fireratesn and call the function that does spawn the bullets
    #region Functions for shooting Automatically

    //manages firerate for tagetted guns that dont care if they have LOS or not
    IEnumerator TargetShooting(){  
       while(active){
           StartCoroutine(TargetShoot(currentAmmo));
           yield return new WaitForSeconds(currentAmmo.firerate);
       }
    }

    //the shooting function for if we need line of sight. Uses raycasts to accomplish this
   IEnumerator LOSTargetShooting(){  
       while(active){
           //finds the player with a raycast and then raycasts up until the player looking for a tree
           RaycastHit2D playerRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 20, playerMask);         
           RaycastHit2D barrierRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, playerRay.distance, barrierMask);
           if(playerRay.collider != null){
               if(playerRay.collider.CompareTag("Player") && barrierRay.collider == null){
                   LOS = true;}
               else{LOS = false;}
           }

           if(LOS){
               StartCoroutine(TargetShoot(currentAmmo));
           }

           yield return new WaitForSeconds(currentAmmo.firerate);
       }

   }

    //basically manages firerate for preset shooting sequences
   IEnumerator PresetShooting(){  

       while(active){
           StartCoroutine(PresetShoot(currentAmmo));
           yield return new WaitForSeconds(currentAmmo.firerate);
       }


   }
    #endregion

    //these next functions make the bullets the dont worry about the firerates or when they are called
    #region Functions for making bullets

    //used for guns that track the player. They use player.transform.position to find their vector 
    IEnumerator TargetShoot(Ammo a){
       playerAng = new Vector3(0, 0, (Mathf.Atan2(player.transform.position.y - gameObject.transform.position.y, player.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg - 90f));
       launchDis = new Vector3(-(player.transform.position.y - transform.position.y), (player.transform.position.x - transform.position.x), 0).normalized;

       for(int j = 0; j < currentAmmo.burstNum; j++){
           for(int i = 0; i < currentAmmo.spreadNum; i++){
               launchAng.z = playerAng.z + spreadsAngle[i];
               activeBullet = (GameObject)Instantiate(a.prefab, gameObject.transform.position + launchDis * spreadsDis[i], Quaternion.Euler(launchAng));
               activeBullet.GetComponent<BulletBehaviour>().SetBullet(a);
            }
           yield return new WaitForSeconds(0.1f);
       }
    }

    //fires bullets in preset directions. Picture how the wisp shoots in 8 directions
   IEnumerator PresetShoot(Ammo a){
       for(int j = 0; j < a.burstNum; j++){
           for(int i = 0; i < a.spreadNum; i++){
               launchAng.z = playerAng.z + i* a.spreadAngle;
               activeBullet = (GameObject)Instantiate(a.prefab, gameObject.transform.position, Quaternion.Euler(launchAng));
               activeBullet.GetComponent<BulletBehaviour>().SetBullet(a);
           }
           yield return new WaitForSeconds(0.1f);
       }
   }

    //only used by frostbite needs it own function because we take in a cardinal direction from the arrow keys to know which way to shoot
    protected IEnumerator FbShoot(Ammo a, Vector3 pos, Vector3 ang)
    {
        launchDis = new Vector3(pos.y - player.transform.position.y, pos.x - player.transform.position.x, 0).normalized;
        for (int j = 0; j < a.burstNum; j++)
        {
            for (int i = 0; i < a.spreadNum; i++)
            {
                //CalculateShooting(a);
                launchAng.z = ang.z + spreadsAngle[i];
                activeBullet = (GameObject)Instantiate(a.prefab, pos + launchDis * spreadsDis[i], Quaternion.Euler(launchAng));
                activeBullet.GetComponent<PlayerBulletBehaviour>().SetBullet(a);
            }
           yield return new WaitForSeconds(0.1f);
        }
    }

    #endregion
}