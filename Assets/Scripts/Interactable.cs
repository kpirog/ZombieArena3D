using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private ItemBase itemBase;
    [SerializeField] private float pickUpDistance = 2f;

    protected bool isCollected = false;
    protected bool isInEquipment;
    private ActionStateManager action;
    protected EquipmentUI equipmentUI;
    public ItemBase ItemBase => itemBase;
    public virtual bool IsInEquipment { get => isInEquipment; set => isInEquipment = value; }

    private void Start()
    {
        action = FindObjectOfType<ActionStateManager>();
        equipmentUI = FindObjectOfType<EquipmentUI>();
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
    private void OnDestroy()
    {
        if (Application.isFocused)
            action.CanPickUp = false;
    }
}
