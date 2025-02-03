using UnityEngine;

public class ETSelect : MonoBehaviour
{
    public bool activated;

    private void OnEnable()
    {
        activated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        activated = true;
    }


    public void SendTo(Vector2 location)
    {
        transform.position = new Vector3(location.x, location.y, transform.position.z);
    }

}
