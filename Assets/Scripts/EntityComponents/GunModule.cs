using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Ammo{
    public string ammoName;
    public float speed;
    public float firerate;
    public float damage;
    public int rebound;
}


public class GunModule : MonoBehaviour
{
    public Ammo currentAmmo;
    public List<Ammo> ammoList = new List<Ammo>();


    public bool targeted = true;


    [Header("Spread  Count")]
    public float spreadNum;
    public float burstNum;

    [Header("Spread Angle")]
    public float spreadAngle;
    public List<float> spreadsAngle = new List<float>();
    private Vector3 launchAng;

    [Header("Spread Distance")]
    public float spreadDis;
    public List<float> spreadsDis = new List<float>();
    private Vector3 launchDis;

    
    
    


    private GameObject player; // player so we can shoot at them
    public GameObject bulletPrefab; //bullet prefab
    private GameObject activeBullet; // husk used to init the bullets
    private Vector3 playerAng; // this is where the bullet is launched from

    public bool needLOS; //determines if your gun should only shoot when it has LOS
    private bool LOS; //Used to track if we can see the player
    private LayerMask playerMask; //layer reference for player
    private LayerMask barrierMask; //layer reference for trees
   
    public bool automatic;
    private bool alive = true;
    

   // Start is called before the first frame update
   void Start()
   {
       //so we can have the players transform
       player = GameObject.FindWithTag("Player");

       CalculateShooting();
   }


    public void SwapAmmo(int i)
    {
        currentAmmo = ammoList[i];
    }

    public void CalculateShooting(){
       if(targeted){
           spreadsAngle.Add(spreadAngle*((spreadNum - 1)/ -2));
           for(int i = 1; i < spreadNum; i++ ){
               spreadsAngle.Add(spreadAngle*i + spreadsAngle[0]);
           }

           spreadsDis.Add(spreadDis*((spreadNum - 1)/ -2));
           for(int i = 1; i < spreadNum; i++ ){
               spreadsDis.Add(spreadDis*i + spreadsDis[0]);
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

   IEnumerator TargetShooting(){  
       while(alive){
           StartCoroutine(TargetShoot());
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
                   LOS = true;
               }
               else{
                   LOS = false;
               }
           }

           if(LOS){
               StartCoroutine(TargetShoot());
           }

           yield return new WaitForSeconds(currentAmmo.firerate);
       }

   }


   IEnumerator PresetShooting(){  

       while(alive){
           StartCoroutine(PresetShoot());
           yield return new WaitForSeconds(currentAmmo.firerate);
       }


   }

   IEnumerator TargetShoot(){
       playerAng = new Vector3(0, 0, (Mathf.Atan2(player.transform.position.y - gameObject.transform.position.y, player.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg - 90f));
       launchDis = new Vector3(-(player.transform.position.y - transform.position.y), (player.transform.position.x - transform.position.x), 0).normalized;

       for(int j = 0; j < burstNum; j++){
           for(int i = 0; i < spreadNum; i++){
               launchAng.z = playerAng.z + spreadsAngle[i];
               activeBullet = (GameObject)Instantiate(bulletPrefab, gameObject.transform.position + launchDis* spreadsDis[i], Quaternion.Euler(launchAng));
               activeBullet.GetComponent<EnemyBulletBehaviour>().bSpeed = currentAmmo.speed;
               activeBullet.GetComponent<EnemyBulletBehaviour>().bDamage = currentAmmo.damage;   
               activeBullet.GetComponent<EnemyBulletBehaviour>().bRebound = currentAmmo.rebound;   
           }
           yield return new WaitForSeconds(0.1f);
       }
   }

   IEnumerator PresetShoot(){
       for(int j = 0; j < burstNum; j++){
           for(int i = 0; i < spreadNum; i++){
               launchAng.z = playerAng.z + i* spreadAngle;
               activeBullet = (GameObject)Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.Euler(launchAng));
               activeBullet.GetComponent<EnemyBulletBehaviour>().bSpeed = currentAmmo.speed;
               activeBullet.GetComponent<EnemyBulletBehaviour>().bDamage = currentAmmo.damage;   
               activeBullet.GetComponent<EnemyBulletBehaviour>().bRebound = currentAmmo.rebound;   

           }
           yield return new WaitForSeconds(0.1f);
       }
   }
   
}

/*
#if UNITY_EDITOR
[CustomEditor(typeof(GunModule))]

public class GunModuleInspector : Editor
{

    public override void OnInspectorGUI()
    {

        GunModule gm = target as GunModule;

        //Targetting style toggle
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Toggle Automatic", GUILayout.Width(150f))){gm.automatic = !gm.automatic;}
        if(gm.automatic){ EditorGUILayout.LabelField("Automatic", EditorStyles.boldLabel);}
        else if(!gm.automatic){ EditorGUILayout.LabelField("Manual", EditorStyles.boldLabel);}
        EditorGUILayout.EndHorizontal();

        //Targetting style toggle
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Switch Style", GUILayout.Width(150f))){gm.targeted = !gm.targeted;}
        if(gm.targeted){ EditorGUILayout.LabelField("Targeted", EditorStyles.boldLabel);}
        else if(!gm.targeted){ EditorGUILayout.LabelField("Preset", EditorStyles.boldLabel);}
        EditorGUILayout.EndHorizontal();

        //LOS toggle
        if(gm.targeted){
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Toggle LOS", GUILayout.Width(150f))){gm.needLOS = !gm.needLOS;}
            if(gm.needLOS){ EditorGUILayout.LabelField("LOS", EditorStyles.boldLabel);}
            else if(!gm.needLOS){ EditorGUILayout.LabelField("NO LOS", EditorStyles.boldLabel);}
            EditorGUILayout.EndHorizontal();
        }

        //standard variables
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Standard Variables", EditorStyles.boldLabel);
        gm.speed = EditorGUILayout.FloatField("Speed", gm.speed);
        gm.firerate = EditorGUILayout.FloatField("Firerate", gm.firerate);
        gm.damage = EditorGUILayout.FloatField("Damage", gm.damage);
        gm.rebound = EditorGUILayout.IntField("Rebound", gm.rebound);

        //variables needed if we shoot multiple bullets per shot
        GUILayout.Space(10);
        EditorGUILayout.LabelField("MultiShot Variables", EditorStyles.boldLabel);
        gm.burstNum = EditorGUILayout.FloatField("Burst Count", gm.burstNum);
        gm.spreadNum = EditorGUILayout.FloatField("Bullet Count", gm.spreadNum);

        if(gm.spreadNum > 1){
            if(gm.targeted){gm.spreadDis = EditorGUILayout.FloatField("Distance between bullets", gm.spreadDis);}
            gm.spreadAngle = EditorGUILayout.FloatField("Angle between bullets ", gm.spreadAngle);
        }

        serializedObject.ApplyModifiedProperties();
    }

}
#endif
*/
