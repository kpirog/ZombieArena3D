using UnityEngine;
using UnityEngine.Events;
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

    [HideInInspector] public UnityEvent<int> onAmmoUpdated;

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

        onAmmoUpdated.AddListener(UpdateAmmoUI);
    }
    private void OnDisable() => onAmmoUpdated.RemoveListener(UpdateAmmoUI);
    public void UpdateSlotUI()
    {
        if (ItemBase == null)
        {
            EnableSlotUI(false);
            return;
        }

        EnableSlotUI(true);
        itemIcon.sprite = ItemBase.ItemIcon;
        itemIcon.color = ItemBase.ItemRarity.color;
        amountText.text = ItemBase.Amount > 1 ? ItemBase.Amount.ToString() : string.Empty;
        EquipmentUI.Instance.SetSlotInputUI(this);
    }
    private void EnableSlotUI(bool enable)
    {
        itemIcon.enabled = enable;
        amountText.enabled = enable;
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
    public void UpdateAmmoUI(int ammo) => amountText.text = ammo > 1 ? ammo.ToString() : string.Empty;
    public void SetInputText(string text) => inputText.SetText(text);
}
