using UnityEngine.Tilemaps;
using UnityEngine;

public class DoorTileBehaviour : MonoBehaviour
{
    BoxCollider2D coll;
    public DoorStates tileState = DoorStates.OPEN;
    private DoorStates dirtyState = DoorStates.OPEN;
    public Vector3Int tileLocation;
    public bool unlocked = false;
    // Start is called before the first frame update
    void Start()
    {
        coll = gameObject.GetComponent<BoxCollider2D>();
        coll.enabled = true;

        if (DoorGridLayer.Instance != null) DoorGridLayer.Instance.stateList.Add(tileLocation, tileState);
    }

    private void Update()
    {
        if(dirtyState != tileState)
        {
            DoorGridLayer.Instance.stateList[tileLocation] = tileState;
            dirtyState = tileState;
            DoorGridLayer.Instance.renderGrid.RefreshTile(tileLocation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        //haven't added key behaviour yet just unlock immediately
        if((int)tileState > 1)
        {
            unlocked = true;
            //coll.enabled = false;
            tileState -= 2;
            dirtyState = tileState;
            DoorGridLayer.Instance.stateList[tileLocation] = tileState;
            DoorGridLayer.Instance.renderGrid.RefreshTile(tileLocation);
        }
    }
}
