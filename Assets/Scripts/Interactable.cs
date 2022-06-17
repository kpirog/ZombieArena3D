using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private ItemBase itemBase;
    [SerializeField] private float pickUpDistance = 2f;

    protected bool isCollected = false;
    protected bool isInEquipment;

    private ActionStateManager action;
    private Item3DTooltip tooltip;

    protected EquipmentUI equipmentUI;
    [HideInInspector] public Animator anim;
    public ItemBase ItemBase => itemBase;
    public virtual bool IsInEquipment { get => isInEquipment; set => isInEquipment = value; }

    protected virtual void Awake()
    {
        action = FindObjectOfType<ActionStateManager>();
        equipmentUI = FindObjectOfType<EquipmentUI>();
        tooltip = FindObjectOfType<Item3DTooltip>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!IsInEquipment)
        {
            if (Vector3.Distance(action.transform.position, transform.position) <= 2f && action.pickUpAction.triggered && !action.CanPickUp)
            {
                action.CanPickUp = true;
                isCollected = true;
            }

            if (isCollected)
            {
                equipmentUI.AddItem(ItemBase);
                Destroy(gameObject);
            }
        }
    }
    private void OnMouseEnter()
    {
        tooltip.transform.position = new Vector3(transform.position.x, transform.position.y + tooltip.yOffset, transform.position.z);
        tooltip.SetLookRotation(Camera.main.transform);
        tooltip.DisplayTooltip(ItemBase);
    }
    private void OnMouseExit()
    {
        tooltip.HideTooltip();
    }
    private void OnDestroy()
    {
        if (Application.isFocused)
            action.CanPickUp = false;
    }
}
