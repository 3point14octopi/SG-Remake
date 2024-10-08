using JAFprocedural;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LanternController : MonoBehaviour
{
    Vector3 centredSide;
    Vector3 target;
    public bool isBobbing = false;
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

        
        for(int i = 2; i < 12; i++)
        {
            //because one of the objects is flipped, it will expand in the opposite direction
            myCollider.offset = new Vector2((float)((i - 1) / 2), 0);
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

    public IEnumerator SpinBlast(float contractionSpeed = 0f)
    {
        isBobbing = false;

        yield return ReturnToOrigin(contractionSpeed);
        StartCoroutine(Blast());
        yield return Rotate();
    }

    private IEnumerator ReturnToOrigin(float speedOverload = 0f)
    {
        float returnRate = (speedOverload != 0f) ? speedOverload : bobSpeed;
        for(Vector3 target = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.position.z); 
            transform.position != target;)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, returnRate * Time.deltaTime);
            yield return new WaitForSeconds(0);
        }
    }

    private IEnumerator Rotate()
    {
        yield return new WaitForSeconds(0.3f);
        
        Vector3 gobRot = new Vector3(0, 0, 2);
        for (int i = 0; i < 120; i++)
        {
            transform.Rotate(gobRot);
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.4f);
        transform.Rotate(new Vector3(0, 0, 120));
    }
}
