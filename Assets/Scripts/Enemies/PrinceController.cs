using EntityStats;
using UnityEngine;

public class PrinceController : MonoBehaviour
{
    Brain princeBrain;

    // Start is called before the first frame update
    void Start()
    {
        princeBrain = GetComponent<Brain>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// EDIT THIS to be a state machine
    /// </summary>
    public void CheckHealthState()
    {
        Debug.Log("health is " + princeBrain.currentStats[(int)EntityStat.Health].ToString());
    }
}
