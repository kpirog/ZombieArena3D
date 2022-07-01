using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class EquipmentUI : MonoBehaviour
{
    public static EquipmentUI Instance { get; private set; }

    [SerializeField] private EquipmentSlot[] equipmentSlots;

    [SerializeField] private ItemBase[] testItems;

    private Transform player;
    private ItemHolder itemHolder;

    private ActionStateManager actionStateManager;
    private InputManager inputManager;

    private PlayerInput playerInput;
    private InputAction scrollEquipmentAction;
    private InputAction switchItemAction;
    private InputAction dropAction;

    private int gamepadSlotIndex;
    public EquipmentSlot SelectedSlot => equipmentSlots.Where(x => x.IsSelected).FirstOrDefault();

    private void Awake()
    {
        if (Instance == null) Instance = new EquipmentUI();
        Instance = this;

        actionStateManager = FindObjectOfType<ActionStateManager>();
        player = actionStateManager.transform;
        playerInput = actionStateManager.GetComponent<PlayerInput>();
        itemHolder = FindObjectOfType<ItemHolder>();
        inputManager = FindObjectOfType<InputManager>();
        scrollEquipmentAction = playerInput.actions["ScrollEquipment"];
        switchItemAction = playerInput.actions["SwitchItem"];
        dropAction = playerInput.actions["Drop"];
    }
    private void Start()
    {
        gamepadSlotIndex = SelectedSlot.SlotIndex;
        TestAdd();
    }
    private void OnEnable()
    {
        scrollEquipmentAction.started += ctx => ScrollSlots((int)ctx.ReadValue<Vector2>().y);
        switchItemAction.performed += ctx => SwitchEquipmentItem((int)ctx.ReadValue<float>());
        dropAction.started += ctx => DropActiveItem();
    }
    private void OnDisable()
    {
        scrollEquipmentAction.started -= ctx => ScrollSlots((int)ctx.ReadValue<Vector2>().y);
        switchItemAction.performed -= ctx => SwitchEquipmentItem((int)ctx.ReadValue<float>());
        dropAction.started -= ctx => DropActiveItem();
    }
    private void SwitchEquipmentItem(int index)
    {
        int slotIndex = 0;

        if (inputManager.CurrentBindingGroup == "Keyboard&Mouse")
        {
            slotIndex = index - 1;

            equipmentSlots[SelectedSlot.SlotIndex].IsSelected = false;
            equipmentSlots[slotIndex].IsSelected = true;
        }
        else
        {
            if (gamepadSlotIndex < equipmentSlots.Length - 1) gamepadSlotIndex++;
            else if (gamepadSlotIndex == equipmentSlots.Length - 1) gamepadSlotIndex = 0;

            slotIndex = gamepadSlotIndex;

            equipmentSlots[SelectedSlot.SlotIndex].IsSelected = false;
            equipmentSlots[slotIndex].IsSelected = true;
        }

        itemHolder.SetItemActive(slotIndex);
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
        if (item as WeaponItem != null || item as ConsumableItem != null)
        {
            EquipmentSlot emptySlot = equipmentSlots.Where(x => x.ItemBase == null).FirstOrDefault();

            if (emptySlot != null)
            {
                emptySlot.ItemBase = item;
                itemHolder.CreateItem(item.ItemPrefab, emptySlot.SlotIndex);

                if (emptySlot.IsSelected) itemHolder.SetItemActive(emptySlot.SlotIndex);
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
        if (item != null)
        {
            Vector3 dropPosition = new Vector3(player.localPosition.x, player.localPosition.y, player.localPosition.z) + player.forward;
            Interactable dropItem = Instantiate(item.ItemPrefab, dropPosition, Quaternion.identity);
            dropItem.IsInEquipment = false;
            itemHolder.DestroyItem(SelectedSlot.SlotIndex);
        }
    }
    private void DropActiveItem()
    {
        ItemBase item = SelectedSlot.ItemBase;

        if (item != null)
        {
            DropItem(SelectedSlot.ItemBase);
            SelectedSlot.ItemBase = null;
        }
    }
    public void SetSlotInputUI(EquipmentSlot slot)
    {
        string inputText = InputControlPath.ToHumanReadableString(switchItemAction.bindings[slot.SlotIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        slot.SetInputText(inputText);
    }
    [ContextMenu("Test Add")]
    public void TestAdd()
    {
        foreach (var item in testItems)
        {
            AddItem(item);
        }
    }
    public EquipmentSlot GetWeaponSlot(WeaponItem weapon)
    {
        EquipmentSlot weaponSlot = equipmentSlots
            .Where(x => (x.ItemBase as WeaponItem) != null)
            .Where(x => x.ItemBase == weapon)
            .FirstOrDefault();

        if (weaponSlot != null)
            return weaponSlot;

        return null;
    }
}
