using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;


public class UpgradeIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Upgrade upgrade;
    [SerializeField] private GameObject upgradeLabel;
    [SerializeField] private GameObject upgradeIcon;
    public TMP_Text descriptionText;

    public int multipleCount = 1;
    [SerializeField] private GameObject multipleLabel;

    public void SetUpgradeUI(Upgrade a, TMP_Text b )
    {
        upgrade = a;
        descriptionText = b;
        upgradeIcon.GetComponent<Image>().sprite = upgrade.uiPic;
        upgradeLabel.GetComponent<TMP_Text>().SetText(upgrade.upgradeName);
    }
    //Detect when Cursor enters the GameObject and turns on the description
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        upgradeLabel.SetActive(true);
        descriptionText.SetText(upgrade.upgradeDescription);
    }

    //Detect when Cursor leaves the GameObject and turns off the description
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        descriptionText.SetText(" ");
        upgradeLabel.SetActive(false);
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
}
