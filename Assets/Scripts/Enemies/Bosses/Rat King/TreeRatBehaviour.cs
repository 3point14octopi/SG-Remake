using EntityStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRatBehaviour : MonoBehaviour
{
    protected delegate void CurrentState(); // our delegate variable type
    protected CurrentState currentState; // our current phase is stored in this delegate

    private float waitTime; //how long we yield return new time
    private bool timeWaited = false; //used to know when the yield time is completed
    private float speed;
    private Animator anim;
    public Vector3 destination;

    private GameObject ratLauncher; //gameobject reference to the launcher


    /// <summary>
    /// recieves the direction to aim from the launcher and calibrates
    /// </summary>
    /// <param name="direction">0 = run right, 1 = down, 2 = left</param>
    /// <param name="launcher">rat launcher ref</param>
    public void CreateRat(RatLaunchStats ratLaunchStats, GameObject launcher)
    {
        ratLauncher = launcher;
        speed = gameObject.GetComponent<Brain>().Stats[(int)EntityStat.Speed];
        anim = gameObject.GetComponent<Animator>();
        currentState = ChargeUp;

        transform.position = ratLaunchStats.launchPosition;
        switch (ratLaunchStats.launchDirection) //sets which way the rat should run and the animation based on the direction variable
        {
            case 0:
                destination = new Vector2(transform.position.x + 30, transform.position.y);
                anim.Play("RatJesterLookRight");
                break;

            case 1:
                destination = new Vector2(transform.position.x, transform.position.y - 20);
                anim.Play("RatJesterLookDown");
                break;

            case 2:
                destination = new Vector2(transform.position.x - 30, transform.position.y);
                anim.Play("RatJesterLookLeft");
                break;
        }

        WaitTime(3f);
    }

    // Update is called once per frame
    void Update()
    {
        currentState();
    }
    private void ChargeUp()
    {

    }
    private void Run() //runs toward the destination until we reach it then destroy ourselves
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * 1.5f * Time.deltaTime);
        if (transform.position.x == destination.x && transform.position.y == destination.y)
        {
            ratLauncher.GetComponent<RatLauncher>().RemoveActive();
            Destroy(gameObject);
        }
        else { Debug.Log(transform.position - destination); }
    }
    public void WaitTime(float f)
    {
        waitTime = f;
        StartCoroutine(TimeWaiter());
    }
    public IEnumerator TimeWaiter() //waits in the trees with snout sticking out for "waittime" seconds before charging across the screen
    {
        yield return new WaitForSeconds(waitTime);
        currentState = Run;
        anim.SetBool("Charge", true);
    }
}
