using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image inputImage;
    [SerializeField] private Image backgroundImage;

    [SerializeField] private TMP_Text amountText;
    [SerializeField] private TMP_Text inputText;

    private ItemBase itemBase;
    private RectTransform iconRectTransform;
    private bool isSelected = false;
    public ItemBase ItemBase { get => itemBase; set { itemBase = value; UpdateSlotUI(); } }

    public int SlotIndex { get; private set; }
    public bool IsSelected
    {
        get => isSelected;
        set
        {
            SetSlotSelected(value);
            isSelected = value;
        }
    }
    private void Awake()
    {
        SlotIndex = transform.GetSiblingIndex();
        iconRectTransform = itemIcon.GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        UpdateSlotUI();

        if (SlotIndex == 0)
        {
            IsSelected = true;
        }
    }
    public void UpdateSlotUI()
    {
        if (itemBase == null)
        {
            EnableSlotUI(false);
            return;
        }

        EnableSlotUI(true);
        itemIcon.sprite = itemBase.ItemIcon;
        itemIcon.color = SetRarityColor(itemBase.ItemRarity);
        amountText.text = itemBase.Amount > 1 ? itemBase.Amount.ToString() : string.Empty;
    }
    private void EnableSlotUI(bool enable)
    {
        itemIcon.enabled = enable;
        inputImage.gameObject.SetActive(enable);
    }
    private void SetSlotSelected(bool selected)
    {
        Color lightColor = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0.1f);
        Color darkColor = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 1f);
        
        if (selected)
        {
            iconRectTransform.offsetMax = new Vector2(-5f, -5f);
            iconRectTransform.offsetMin = new Vector2(5f, 5f);
            backgroundImage.color = darkColor;
        }
        else
        {
            iconRectTransform.offsetMax = Vector2.zero;
            iconRectTransform.offsetMin = Vector2.zero;
            backgroundImage.color = lightColor;
        }
    }
    private Color SetRarityColor(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Common: return Color.grey;
            case ItemRarity.Rare: return Color.blue;
            case ItemRarity.Epic: return Color.magenta;
            case ItemRarity.Legendary: return Color.yellow;
            default: return Color.white;
        }
    }
}
