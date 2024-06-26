using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform startTarget;
    bool zoomed = false;
    public bool chaseTarget = false;
    bool honing = false;
    Vector3 honeTarget = new Vector3(0, 0, -5);
    public float speed;
    public static FollowCam Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(startTarget.position.x, startTarget.position.y, -5);
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (chaseTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(startTarget.transform.position.x, startTarget.transform.position.y, -5), 8 * Time.deltaTime);
            if (Vector2.Distance(transform.position, startTarget.position) == 0)
            {
                transform.SetParent(startTarget, true);
                chaseTarget = false;
            }
        }else if (honing)
        {
            transform.position = Vector3.MoveTowards(transform.position, honeTarget, speed * Time.deltaTime);
            if (transform.position.x == honeTarget.x && transform.position.y == honeTarget.y) honing = false;
        }
    }

    public void SlideToNew(Vector3 newTarget)
    {
        chaseTarget = false;
        honing = true;
        honeTarget = new Vector3(newTarget.x, newTarget.y, -5);
        transform.SetParent(null, true);
    }

    public void ForceJump(Vector3 position)
    {
        honing = false;
        chaseTarget = false;
        transform.position = new Vector3(position.x, position.y, -5);
    }

    public void ZoomIn()
    {
        if (!zoomed)
        {
            cam.orthographicSize -= 2;
            zoomed = true;
        }
    }

    public void ZoomOut()
    {
        if (zoomed)
        {
            cam.orthographicSize += 2;
            zoomed = false;
        }
        
    }
}
