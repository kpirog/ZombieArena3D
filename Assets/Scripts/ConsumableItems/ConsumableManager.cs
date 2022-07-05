using UnityEngine.InputSystem;
using UnityEngine;

public class ConsumableManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction useAction;

    public Consumable selectedItem;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        useAction = playerInput.actions["UseItem"];
    }
    private void OnEnable()
    {
        useAction.started += ctx => UseItem();   
    }
    private void OnDisable()
    {
        useAction.started -= ctx => UseItem();
    }
    public void SetItem(Interactable item)
    {
        Consumable consumable = item as Consumable;

        if (consumable == null)
        {
            selectedItem = null;
        }
        else
        {
            selectedItem = consumable;
        }
    }
    public void UseItem()
    {
        if (selectedItem != null)
        {
            selectedItem.Use();
        }
    }
}
