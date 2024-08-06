using JAFprocedural;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LanternController : MonoBehaviour
{
    Vector3 centredSide;
    Vector3 target;
    bool isBobbing = false;
    float bobSpeed = 1;

    Animator anims;
    BoxCollider2D myCollider;
    // Start is called before the first frame update
    void Start()
    {
        //classic single GetComponent at start
        anims = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBobbing)
        {
            //slide the boys up and down
            transform.position = Vector3.MoveTowards(transform.position, target, bobSpeed * Time.deltaTime);
            //pick a new target if reached
            if (Vector3.Distance(transform.position, target) < 0.1) StartBob(bobSpeed);
        }
    }

    public IEnumerator GoToStart(float distanceToTravel, float speed)
    {
        for(float traveled = 0f; traveled < distanceToTravel;)
        {
            float dThisFrame = Time.deltaTime * speed;
            transform.position += transform.right * (-dThisFrame);
            traveled += dThisFrame;
            yield return null;
        }
        centredSide = transform.position;
        StartBob(speed);
    }

    public void StartBob(float speed)
    {
        int yAmount = RNG.GenRand(0, 7) - 3;
        target = centredSide + new Vector3(0, yAmount, 0);
        isBobbing = true;
        bobSpeed = speed;
    }

    public IEnumerator Blast()
    {
        isBobbing = false;
        anims.Play("Blast");
        yield return new WaitForSeconds(1f);

        //this variable will let us know what direction to slide the collider in 
        //so that it looks like it is being emitted from the lantern
        float mult = (transform.rotation.y / 180 == 0) ? -1 : 1;
        Debug.Log("mult is " + mult.ToString());
        for(int i = 2; i < 12; i++)
        {
            //shift the collider left (or right) depending on the orientation of the lantern
            float x = ((i - 1) / (2 * mult));
            Debug.Log(x.ToString());
            myCollider.offset = new Vector2((float)((i - 1) /2) , 0);
            myCollider.size = new Vector2(i, 1);

            //wait for next frame
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        myCollider.offset = new Vector2(0, 0);
        myCollider.size = new Vector2(1, 1);
        anims.Play("Idle");
        yield return null;
        StartBob(bobSpeed);
    }
}
