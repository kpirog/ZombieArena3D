using UnityEngine;
using TMPro;

public class Item3DTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemInfoText;
    [SerializeField] private TMP_Text inputText;

    [SerializeField] private GameObject tooltipPanel;

    public float yOffset = 10f;

    private void Start()
    {
        HideTooltip();
    }
    public void DisplayTooltip(ItemBase itemBase)
    {
        itemNameText.SetText(itemBase.name);
        itemNameText.color = itemBase.ItemRarity.color;
        
        WeaponItem weaponItem = itemBase as WeaponItem;
        if(weaponItem != null)
        {
            itemInfoText.text = $"Ammo: {weaponItem.Amount}";
        }
        else
        {
            itemInfoText.text = string.Empty;
        }

        tooltipPanel.SetActive(true);
    }
    public void HideTooltip()
    {
        itemNameText.text = string.Empty;
        itemInfoText.text = string.Empty;
        tooltipPanel.SetActive(false);
    }
    public void SetLookRotation(Transform target)
    {
        transform.LookAt(target.localPosition);
        transform.Rotate(new Vector3(0f, 180f, 0f));
    }
}
