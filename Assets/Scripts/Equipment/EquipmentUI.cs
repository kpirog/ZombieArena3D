using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField] private EquipmentSlot[] equipmentSlots;
    [SerializeField] private ItemBase testItemBase;
    [SerializeField] private ItemBase testItemBase2;

    private Transform player;
    private ItemHolder itemHolder;

    private ActionStateManager actionStateManager;
    private PlayerInput playerInput;
    private InputAction scrollEquipmentAction;

    public EquipmentSlot SelectedSlot => equipmentSlots.Where(x => x.IsSelected).FirstOrDefault();

    private void Awake()
    {
        actionStateManager = FindObjectOfType<ActionStateManager>();
        player = actionStateManager.transform;
        playerInput = actionStateManager.GetComponent<PlayerInput>();
        itemHolder = FindObjectOfType<ItemHolder>();
        scrollEquipmentAction = playerInput.actions["ScrollEquipment"];
    }
    private void OnEnable()
    {
        scrollEquipmentAction.started += ctx => ScrollSlots((int)ctx.ReadValue<Vector2>().y);
        TestAdd();
    }
    private void OnDisable()
    {
        scrollEquipmentAction.started -= ctx => ScrollSlots((int)ctx.ReadValue<Vector2>().y);
    }
    private void ScrollSlots(int scroll)
    {
        int selectedIndex = SelectedSlot.SlotIndex;

        if (scroll > 0)
        {
            if (selectedIndex >= equipmentSlots.Length - 1) return;

            equipmentSlots[selectedIndex].IsSelected = false;
            equipmentSlots[selectedIndex + 1].IsSelected = true;
        }
        else if (scroll < 0)
        {
            if (selectedIndex <= 0) return;

            equipmentSlots[selectedIndex].IsSelected = false;
            equipmentSlots[selectedIndex - 1].IsSelected = true;
        }

        itemHolder.SetItemActive(SelectedSlot.SlotIndex);
    }
    public void AddItem(ItemBase item)
    {
        if (item as WeaponItem != null)
        {
            EquipmentSlot emptySlot = equipmentSlots.Where(x => x.ItemBase == null).FirstOrDefault();

            if (emptySlot != null)
            {
                emptySlot.ItemBase = item;
                itemHolder.CreateItem(item.ItemPrefab);
            }
            else
            {
                EquipmentSlot currentSlot = SelectedSlot;
                DropItem(currentSlot.ItemBase);
                currentSlot.ItemBase = item;
            }
        }
    }
    private void DropItem(ItemBase item)
    {
        if (item as WeaponItem != null)
        {
            Interactable dropItem = Instantiate(item.ItemPrefab, new Vector3(player.position.x, player.position.y, player.position.z + 5f), Quaternion.identity);
        }
        else
        {
            for (int i = 0; i < item.Amount; i++)
            {
                Interactable dropItem = Instantiate(item.ItemPrefab, new Vector3(player.position.x, player.position.y, player.position.z + (5f + i)), Quaternion.identity);
            }
        }
    }

    [ContextMenu("Test Add")]
    public void TestAdd()
    {
        AddItem(testItemBase);
        AddItem(testItemBase2);
    }
}
