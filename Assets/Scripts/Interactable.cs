using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private ItemBase itemBase;
    [SerializeField] private float pickUpDistance = 2f;

    protected bool isCollected = false;
    private ActionStateManager action;
    protected EquipmentUI equipmentUI;

    public ItemBase ItemBase => itemBase;

    private void Start()
    {
        action = FindObjectOfType<ActionStateManager>();
        equipmentUI = FindObjectOfType<EquipmentUI>();
    }
    private void Update()
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
    private void OnDestroy()
    {
        action.CanPickUp = false;
    }
}
