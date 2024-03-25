using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBitchin : MonoBehaviour
{
    public Vector3 target = Vector3.zero;
    public Vector2 offset = Vector2.zero;
    public Vector2 range = Vector2.zero;
    bool activated = false;
    public float damage = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("GOTCHA BITCH!");
            collision.gameObject.GetComponent<FbStateManager>().health = collision.gameObject.GetComponent<FbStateManager>().health - damage;
        }
    }
}
