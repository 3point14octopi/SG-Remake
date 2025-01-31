using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RatKingPhases : PhaseFrameWork
{
    public GameObject player;
    public float speed; // Speed of the Rat King
    public float smoothTime = 0.3f; // Time to reach the target smoothly
    private Vector2 velocity = Vector2.zero;

    public GameObject ratlauncher;

    private GameObject ratHusk; // husk used to init the rats
    public GameObject ratPrefab;
    public float ratSpawnRate;
    private float ratTimer;
    // Start is called before the first frame update
    void Start()
    {
        speed = gameObject.GetComponent<Brain>().currentStats[1];
        // Fills our delegate array with our phase logic
        PhaseDelegateArray = new PhaseDelegate[]
        {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            PhaseFour,
            PhaseFive
        };
        currentPhaseDelegate = PhaseDelegateArray[currentPhase];
    }




    protected override void PhaseOne()
    {
        ChasePlayer();
        if(ratTimer <= 0)
        {
            ratHusk = (GameObject)Instantiate(ratPrefab, gameObject.transform.position, Quaternion.identity);
            ratHusk.GetComponent<JesterRatBehaviour>().SpawnIn(player, gameObject);
            ratTimer = ratSpawnRate;
        }
        else { ratTimer -= Time.deltaTime; }
    }

    protected override void PhaseTwo()
    {
        if (phaseStart) ratlauncher.GetComponent<RatLauncher>().waveNum = 8;
        ChasePlayer();
        if (ratTimer <= 0)
        {
            ratHusk = (GameObject)Instantiate(ratPrefab, gameObject.transform.position, Quaternion.identity);
            ratHusk.GetComponent<JesterRatBehaviour>().SpawnIn(player, gameObject);
            ratTimer = ratSpawnRate;
        }
        else { ratTimer -= Time.deltaTime; }
    }

    protected override void PhaseThree()
    {
        if (phaseStart) ratSpawnRate = 6;
        ChasePlayer();
        if (ratTimer <= 0)
        {
            ratHusk = (GameObject)Instantiate(ratPrefab, gameObject.transform.position, Quaternion.identity);
            ratHusk.GetComponent<JesterRatBehaviour>().SpawnIn(player, gameObject);
            ratTimer = ratSpawnRate;
        }
        else { ratTimer -= Time.deltaTime; }
    }
    protected override void PhaseFour()
    {
        if (phaseStart)
        {
            ratlauncher.GetComponent<RatLauncher>().waveNum = 5;
            ratlauncher.GetComponent<RatLauncher>().isConstant = true;
        }
        ChasePlayer();
        if (ratTimer <= 0)
        {
            ratHusk = (GameObject)Instantiate(ratPrefab, gameObject.transform.position, Quaternion.identity);
            ratHusk.GetComponent<JesterRatBehaviour>().SpawnIn(player, gameObject);
            ratTimer = ratSpawnRate;
        }
        else { ratTimer -= Time.deltaTime; }
    }



    private void ChasePlayer()
    {
           
            // Get the current position of the Rat King and the player
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = player.transform.position;

            // Smoothly move towards the player
            Vector2 newPosition = Vector2.SmoothDamp(currentPosition, targetPosition, ref velocity, smoothTime, speed);

            // Update the position of the Rat King
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

            anim.SetFloat("XSpeed", velocity.x);
            anim.SetFloat("YSpeed", velocity.y);
        if (Mathf.Abs(currentPosition.x - targetPosition.x) < Mathf.Abs(currentPosition.y - targetPosition.y)) anim.SetBool("TravelVert", true);
        else anim.SetBool("TravelVert", false);

    }
}
