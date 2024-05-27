using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class GunModule : MonoBehaviour
{
    public Ammo currentAmmo; //bullet being fired
    public List<Ammo> ammoList = new List<Ammo>();




    private List<float> spreadsAngle = new List<float>();//angle difference when fired multiple bullets
    private Vector3 launchAng;
    private List<float> spreadsDis = new List<float>();//gap between multiple bullets
    private Vector3 launchDis;

    
    
    


    private GameObject player; // player so we can shoot at them
    private GameObject activeBullet; // husk used to init the bullets
    private Vector3 playerAng; // this is where the bullet is launched from

    private bool LOS; //Used to track if we can see the player
    private LayerMask playerMask; //layer reference for player
    private LayerMask barrierMask; //layer reference for trees

    [HideInInspector]
    public bool automatic; //if we should fire constantly
    [HideInInspector]
    public bool targeted;   //if we should fire preset or targeted
    [HideInInspector]
    public bool needLOS; //determines if the gun should only shoot when it has LOS
    private bool alive = true;
    

   // Start is called before the first frame update
   void Start()
   {
       //so we can have the players transform
       player = GameObject.FindWithTag("Player");

       CalculateShooting(currentAmmo);
   }

    #region Functions for instantiating Bullets
    public void SwapAmmo(int i)
    {
        currentAmmo = ammoList[i];
        CalculateShooting(currentAmmo);
    }

    public void CalculateShooting(Ammo a){
       if(targeted){
           spreadsAngle.Add(a.spreadAngle * ((a.spreadNum - 1)/ -2));
           for(int i = 1; i < a.spreadNum; i++ ){
               spreadsAngle.Add(a.spreadAngle *i + spreadsAngle[0]);
           }

           spreadsDis.Add(a.spreadDis *((a.spreadNum - 1)/ -2));
           for(int i = 1; i < a.spreadNum; i++ ){
               spreadsDis.Add(a.spreadDis *i + spreadsDis[0]);
           }

           if(needLOS && automatic){                
               //find our layers used for our raycast masks
               playerMask |= 0x1 << 6;
               barrierMask |= 0x1 << 7;
               StartCoroutine(LOSTargetShooting());
           }

           else if(!needLOS && automatic){StartCoroutine(TargetShooting());}

       }
       else if(!targeted && automatic){StartCoroutine(PresetShooting());}
   }
    #endregion

    #region Functions for shooting Automatically
    IEnumerator TargetShooting(){  
       while(alive){
           StartCoroutine(TargetShoot(currentAmmo));
           yield return new WaitForSeconds(currentAmmo.firerate);
       }
   }

   IEnumerator LOSTargetShooting(){  
       while(alive){
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


   IEnumerator PresetShooting(){  

       while(alive){
           StartCoroutine(PresetShoot(currentAmmo));
           yield return new WaitForSeconds(currentAmmo.firerate);
       }


   }
    #endregion

    #region Functions for making bullets
    IEnumerator TargetShoot(Ammo a){
       playerAng = new Vector3(0, 0, (Mathf.Atan2(player.transform.position.y - gameObject.transform.position.y, player.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg - 90f));
       launchDis = new Vector3(-(player.transform.position.y - transform.position.y), (player.transform.position.x - transform.position.x), 0).normalized;

       for(int j = 0; j < currentAmmo.burstNum; j++){
           for(int i = 0; i < currentAmmo.spreadNum; i++){
               launchAng.z = playerAng.z + spreadsAngle[i];
               activeBullet = (GameObject)Instantiate(a.prefab, gameObject.transform.position + launchDis* spreadsDis[i], Quaternion.Euler(launchAng));
               activeBullet.GetComponent<EnemyBulletBehaviour>().SetBullet(currentAmmo);
            }
           yield return new WaitForSeconds(0.1f);
       }
   }

   IEnumerator PresetShoot(Ammo a){
       for(int j = 0; j < currentAmmo.burstNum; j++){
           for(int i = 0; i < currentAmmo.spreadNum; i++){
               launchAng.z = playerAng.z + i* currentAmmo.spreadAngle;
               activeBullet = (GameObject)Instantiate(a.prefab, gameObject.transform.position, Quaternion.Euler(launchAng));
               activeBullet.GetComponent<EnemyBulletBehaviour>().SetBullet(currentAmmo);
           }
           yield return new WaitForSeconds(0.1f);
       }
   }

    #endregion
}