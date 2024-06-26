using EntityStats;
using JAFprocedural;
using UnityEngine;

public class TreeKnightController : MonoBehaviour
{
    Brain treeBrain;
    Animator anim;
    AudioSource audioS;
    private int direction = 1;

    public AudioClip walk;
    public AudioClip wall;
    private void OnEnable()
    {
        if(anim != null)
        {
            anim.SetBool("Death", false);
            GetComponent<GunModule>().ToggleAutomatic(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        treeBrain = GetComponent<Brain>();
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (treeBrain.isAlive)
        {
            transform.position += transform.right * direction * Time.deltaTime * treeBrain.currentStats[(int)EntityStat.Speed];

            if (!audioS.isPlaying)
            {
                if (RNG.GenRand(1, 2) == 1) audioS.PlayOneShot(walk);
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Barrier")
        {
            direction = -direction;
            anim.SetFloat("Direction", direction);
            audioS.PlayOneShot(wall);
        }
    }
}
