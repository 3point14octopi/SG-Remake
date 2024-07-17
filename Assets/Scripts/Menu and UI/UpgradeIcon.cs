using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;


public class UpgradeIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Upgrade upgrade;
    private GameObject upgradeLabel;
    public TMP_Text descriptionText;

    public void SetUpgradeUI(Upgrade a, TMP_Text b )
    {
        upgrade = a;
        descriptionText = b;
        gameObject.GetComponent<Image>().sprite = upgrade.uiPic;
        upgradeLabel = transform.Find("Upgrade Label").gameObject;
        upgradeLabel.GetComponent<TMP_Text>().SetText(upgrade.upgradeName);
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        upgradeLabel.SetActive(true);
        descriptionText.SetText(upgrade.upgradeDescription);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        descriptionText.SetText(" ");
        upgradeLabel.SetActive(false);
    }
}
