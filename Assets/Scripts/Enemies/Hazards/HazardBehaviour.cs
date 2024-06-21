using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HazardBehaviour : MonoBehaviour
{
    //Different hazard types that are available
    public enum HazardType {
        Pit,
        Spikes,
        Moving
    }
    public HazardType hazardType;
    [HideInInspector]
    public bool hitPlayer;
    [HideInInspector]
    public float damage; //damage the hazard does to the player
    [HideInInspector]
    public float knockBack; //distance the player is pushed back in tiles

    [HideInInspector]
    public float pitTime; // time a pit hazard will keep you in a pitww
    private Vector3 knockBackLocation;

    [HideInInspector]
    public float speed;

    [HideInInspector]
    public bool breakable;

    [HideInInspector]
    public float health;
    [HideInInspector]
    public Material flash;
    [HideInInspector]
    private Material material;
    [HideInInspector]
    public float flashDuration;

    void Start()
    {
        material = gameObject.GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //using for the moving type
        if(hazardType == HazardType.Moving){
            transform.position += transform.right * Time.deltaTime * speed;
        } 
    }

    void OnTriggerEnter2D(Collider2D other){

        //damages the player if it is walked into by the player
        if (other.gameObject.tag == "Player" && hitPlayer)
        {
            //if(hazardType == HazardType.Spikes){
            //    StartCoroutine(BounceOff(other));
            //}

            //else if(hazardType == HazardType.Pit){
            //    StartCoroutine(FallIn(other));
            //}

            //else if(hazardType == HazardType.Moving){
            //    StartCoroutine(RolledOver(other));
            //}
        }


        else if (other.gameObject.tag == "PlayerBullet")
        {
            if(breakable){
                health = health - other.gameObject.GetComponent<PlayerBulletBehaviour>().bDamage;
                StartCoroutine(Flash());
                //if the damage is too much the enemy dies
                if(health <= 0){
                    Destroy(gameObject);
                }
            }
            Destroy(other.gameObject);
        }

    }



    //in order it turns off our game, calls the damage function, knocks the player back on a normalized vector, pauses time form
    //IEnumerator BounceOff(Collider2D other){
    //    //other.gameObject.GetComponent<FbStateManager>().SwitchState(other.gameObject.GetComponent<FbStateManager>().StunState);  
    //    //other.transform.position += new Vector3((other.transform.position.x - transform.position.x)*knockBack, (other.transform.position.y - transform.position.y)*knockBack, 0);

    //    //if(other.gameObject.GetComponent<FbStateManager>().health > 0){
    //    //    yield return new WaitForSeconds(other.gameObject.GetComponent<FbStateManager>().flashDuration);
    //    //    other.gameObject.GetComponent<FbStateManager>().SwitchState(other.gameObject.GetComponent<FbStateManager>().IdleState);
    //    //}
    //}

    //IEnumerator FallIn(Collider2D other){
    //    //other.gameObject.GetComponent<FbStateManager>().SwitchState(other.gameObject.GetComponent<FbStateManager>().StunState);
    //    //knockBackLocation = new Vector3((other.transform.position.x - transform.position.x)*knockBack, (other.transform.position.y - transform.position.y)*knockBack, 0);
    //    //other.transform.position = transform.position;
    //    //// other.gameObject.GetComponent<FbStateManager>().anim.Play("FrostbiteFall")
    //    //yield return new WaitForSeconds(pitTime);
    //    //if(other.gameObject.GetComponent<FbStateManager>().health > 0){
    //    //    other.transform.position += knockBackLocation;

    //    //    yield return new WaitForSeconds(other.gameObject.GetComponent<FbStateManager>().flashDuration);
    //    //    other.gameObject.GetComponent<FbStateManager>().SwitchState(other.gameObject.GetComponent<FbStateManager>().IdleState);
    //    //}
    //}

    //IEnumerator RolledOver(Collider2D other){
    //    //other.gameObject.GetComponent<FbStateManager>().SwitchState(other.gameObject.GetComponent<FbStateManager>().StunState);
    //    //other.gameObject.GetComponent<FbStateManager>().TakeDamage(damage);
    //    //other.transform.position += new Vector3((other.transform.position.x - transform.position.x)*knockBack, (other.transform.position.y - transform.position.y)*knockBack, 0);

    //    //if(other.gameObject.GetComponent<FbStateManager>().health > 0){
    //    //    yield return new WaitForSeconds(other.gameObject.GetComponent<FbStateManager>().flashDuration);
    //    //    other.gameObject.GetComponent<FbStateManager>().SwitchState(other.gameObject.GetComponent<FbStateManager>().IdleState);
    //    //}
    //}

    IEnumerator Flash(){

        gameObject.GetComponent<SpriteRenderer>().material = flash;      
        yield return new WaitForSeconds(flashDuration);
        gameObject.GetComponent<SpriteRenderer>().material = material;

    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(HazardBehaviour))]

public class HazardCustomInspector : Editor
{

    public override void OnInspectorGUI()
    {

        HazardBehaviour hzb = target as HazardBehaviour;
        base.OnInspectorGUI();
        serializedObject.ApplyModifiedProperties();

        if(hzb.hazardType == HazardBehaviour.HazardType.Pit){ //this shows up when it is in pit mode
            hzb.pitTime = EditorGUILayout.FloatField("Time Trapped", hzb.pitTime);
        }

        else if(hzb.hazardType == HazardBehaviour.HazardType.Spikes){ // inspector that shows when it is in spike mode
        }

        else if(hzb.hazardType == HazardBehaviour.HazardType.Moving){ //inspector that shows in moving mode
            hzb.speed = EditorGUILayout.FloatField("Speed", hzb.speed);
        }

        //makes
        if(GUILayout.Button("Impact the player", GUILayout.Width(150f))){
            hzb.hitPlayer = !hzb.hitPlayer;
                hzb.gameObject.GetComponent<BoxCollider2D>().isTrigger = !hzb.gameObject.GetComponent<BoxCollider2D>().isTrigger;
            if(!hzb.hitPlayer){
                hzb.damage = 0;
                hzb.knockBack = 0;
            }
        }
        if(hzb.hitPlayer){
            hzb.damage = EditorGUILayout.FloatField("Damage", hzb.damage);
            hzb.knockBack = EditorGUILayout.FloatField("Knockback", hzb.knockBack);
        }

        //Makes it so the hazard can take damage, could change
        if(GUILayout.Button("Make it breakable", GUILayout.Width(150f))){
            hzb.breakable = !hzb.breakable;
        }
        if(hzb.breakable){
            hzb.health = EditorGUILayout.FloatField("Health", hzb.health);
            hzb.flashDuration = EditorGUILayout.FloatField("Flash Duration", hzb.flashDuration);
        }

        
        serializedObject.ApplyModifiedProperties();
    }

}
#endif
