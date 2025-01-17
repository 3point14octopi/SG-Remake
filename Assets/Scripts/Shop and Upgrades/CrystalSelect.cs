using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class CrystalSelect : MonoBehaviour
{
    private bool inRange = false;
    private bool enabledScreen = false;
    private bool powerChosen = false;
    public GameObject player;
    public GameObject crystalScreen;
    public GameObject powerIcon;
    public GameObject itemHolder;
    public TMP_Text descriptionText;
    [SerializeField] public List<Upgrade> IcePowers = new List<Upgrade>(); //includes repeats
    [SerializeField] public List<GameObject> Icons = new List<GameObject>(); //nor repeats
    public GameObject selector;
    public int selectorValue = 1;
    public Vector3 iconScale;

    // Update is called once per frame
    private void Start()
    {
        for (int i = 0; i < IcePowers.Count; i++)
        {
            GameObject temp = Instantiate(powerIcon, itemHolder.transform);
            temp.GetComponent<UpgradeIcon>().SetIcePowerUI(IcePowers[i], descriptionText, iconScale);
            Icons.Add(temp);

        }
    }
    void Update()
    {
        if (inRange)
        {
            if (Input.GetKeyUp(KeyCode.Space) && !powerChosen) EnableScreen();
        }
        if (enabledScreen)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) SelectLeft();
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) SelectRight();
            if (Input.GetKeyDown(KeyCode.Space)) SelectPower();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Meow");
        inRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inRange = false;
    }
    
    private void EnableScreen()
    {
        enabledScreen = true;
        player.GetComponent<FbStateManager>().SwitchState(player.GetComponent<FbStateManager>().StunState);
        crystalScreen.SetActive(true);
        SelectRight();
    }
    //Moves the selector to the power up to the left
    private void SelectLeft()
    {
        selectorValue--;
        if (selectorValue < 0) selectorValue = Icons.Count - 1; // Wrap around to the last item
        selector.transform.position = Icons[selectorValue].transform.position;
        Icons[selectorValue].GetComponent<UpgradeIcon>().ToggleDescription();
    }

    //Moves the selector to the power up to the right
    private void SelectRight()
    {
        selectorValue++;
        if (selectorValue >= Icons.Count) selectorValue = 0; // Wrap around to the first item
        selector.transform.position = Icons[selectorValue].transform.position;
        Icons[selectorValue].GetComponent<UpgradeIcon>().ToggleDescription();
    }

    private void SelectPower()
    {
        enabledScreen = false;
        player.GetComponent<FbStateManager>().SwitchState(player.GetComponent<FbStateManager>().IdleState);
        player.GetComponent<FbUpgradeManager>().AddUpgrade(IcePowers[selectorValue]);
        crystalScreen.SetActive(false);
        powerChosen = true;
    }
}
