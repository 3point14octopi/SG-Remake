using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;


public class UpgradeIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public bool crystal = false;
    public Upgrade upgrade;
    [SerializeField] private GameObject upgradeLabel;
    [SerializeField] private GameObject upgradeIcon;
    public TMP_Text descriptionText;

    public int multipleCount = 1;
    [SerializeField] private GameObject multipleLabel;

    /// <summary>
    /// When this is in upgrade mode we use this setup
    /// </summary>
    /// <param Upgrade="a"></param>
    /// <param Description Box="b"></param>
    public void SetUpgradeUI(Upgrade a, TMP_Text b )
    {
        upgrade = a;
        descriptionText = b;
        upgradeIcon.GetComponent<Image>().sprite = upgrade.uiPic;
        upgradeLabel.GetComponent<TMP_Text>().SetText(upgrade.upgradeName);
    }

    /// <summary>
    /// This setup is used when it is acting as a powerup selector
    /// </summary>
    /// <param Upgrade="a"></param>
    /// <param Description Box="b"></param>
    public void SetIcePowerUI(Upgrade a, TMP_Text b, Vector3 c)
    {
        upgrade = a;
        descriptionText = b;
        upgradeIcon.GetComponent<Image>().sprite = upgrade.uiPic;
        upgradeLabel.GetComponent<TMP_Text>().SetText(upgrade.upgradeName);
        crystal = true;
        transform.localScale = c;
    }
    //Detect when Cursor enters the GameObject and turns on the description
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (!crystal)
        {
            upgradeLabel.SetActive(true);
            ToggleDescription();
        }
    }

    //Detect when Cursor leaves the GameObject and turns off the description
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (!crystal)
        {
            descriptionText.SetText(" ");
            upgradeLabel.SetActive(false);
        }
    }

    /// <summary>
    /// changes the multiplier symbol beside an upgrade icon when we pick up a duplicate
    /// </summary>
    public void MultipleCounter()
    {
        multipleCount++;
        multipleLabel.SetActive(true);
        multipleLabel.GetComponent<TMP_Text>().SetText("x" + multipleCount);
    }

    public void ToggleDescription()
    {
        descriptionText.SetText(upgrade.upgradeDescription);
    }

}
