using EntityStats;
using JAFprocedural;
using UnityEngine;

public class WispController : MonoBehaviour
{
    Animator anim;
    Brain wispBrain;
    Rigidbody2D body;

    LayerMask barrierMask;
    public float avoidVal = 1.5f;


    private void OnEnable()
    {
        if (body != null)
        {
            body.bodyType = RigidbodyType2D.Dynamic;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        wispBrain = GetComponent<Brain>();
        body = GetComponent<Rigidbody2D>();

        barrierMask |= 0x1 << 7;
    }

    // Update is called once per frame
    void Update()
    {
        if (wispBrain.isAlive)
        {
            Wander();
            if(wispBrain.roomIndex == 0)
            {
                //control the others
                
                for(int i = wispBrain.mom.population-1; i > 0 && wispBrain.isAlive; i--)
                {
                    Transform cBoid = wispBrain.mom.GetRoomEnemy(i);
                    Transform ifBoid = wispBrain.mom.GetRoomEnemy(i - 1);

                    cBoid.rotation = Quaternion.Euler(0, 0, angle(cBoid.position, ifBoid.position));
                    CheckForCollisions(cBoid);
                    cBoid.position = Vector2.MoveTowards(cBoid.position, ifBoid.position, GetSpeed() * Time.deltaTime);
                }
            }

            if(wispBrain.roomIndex < 1)
            {
                body.AddForce(transform.up.normalized + CheckForCollisions(transform) * Time.deltaTime * GetSpeed(), ForceMode2D.Impulse);
                if (body.velocity.magnitude > GetSpeed())
                {
                    body.velocity = Vector2.ClampMagnitude(body.velocity, GetSpeed());
                }
            }
        }
    }
    float GetSpeed()
    {
        return wispBrain.currentStats[(int)EntityStat.Speed];
    }

    void Wander()
    {
        if(RNG.GenRand(1, 10) > 8)
        {
            float wRot = (RNG.GenRand(-1, 3) + RNG.GenRand(-1, 3)) % 2;
            transform.Rotate(0, 0, wRot * 5);
        }
    }

    private float angle(Vector3 boid, Vector3 target)
    {
        return (Mathf.Atan2(target.y - boid.y, target.x - boid.x) * Mathf.Rad2Deg - 90f);
    }

    Vector3 LinearSeparation(Vector3 position, Vector3 obstacle)
    {
        Vector3 avoidVector = obstacle - position;
        if(avoidVector.magnitude < avoidVal)
        {
            float intensity = (GetSpeed() * 10) * (avoidVal - avoidVector.magnitude);
            return avoidVector * intensity;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// rework this
    /// </summary>
    /// <param name="boid">wisp being moved</param>
    /// <returns></returns>
    Vector3 CheckForCollisions(Transform boid)
    {
        Vector3 avoidance = Vector3.zero;

        RaycastHit2D wallDetect = Physics2D.Raycast(boid.position + boid.up, boid.up, 2f, barrierMask);
        if(wallDetect.collider != null)
        {
            Vector3 obstaclePosition = AstarDebugLayer.Instance.renderGrid.GetCellCenterWorld(AstarDebugLayer.Instance.renderGrid.WorldToCell(wallDetect.point));
            avoidance -= LinearSeparation(boid.position, obstaclePosition);
        }

        if(avoidance != Vector3.zero)
        {
            boid.Rotate(0, 0, 45);
        }

        return avoidance;
    }
}
