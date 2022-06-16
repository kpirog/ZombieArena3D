using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public List<HandSlot> handSlots;

    private ActionStateManager actionStateManager;

    private bool AnyHandSlotActive => handSlots.Any(x => x.gameObject.activeInHierarchy);

    private void Awake()
    {
        handSlots = new List<HandSlot>(GetComponentsInChildren<HandSlot>());
        actionStateManager = FindObjectOfType<ActionStateManager>();
    }
    private void Start()
    {
        handSlots.ForEach(x => x.gameObject.SetActive(false));
    }
    private void Update()
    {
        if(!AnyHandSlotActive) SetItemActive(0);
    }
    public void CreateItem(Interactable interactable, int slotIndex)
    {
        handSlots[slotIndex].gameObject.SetActive(true);
        handSlots[slotIndex].CreateItem(interactable);
        handSlots[slotIndex].gameObject.SetActive(false);
    }
    public void SetItemActive(int index)
    {
        if (handSlots.Count == 0) return;

        HandSlot activeSlot = handSlots.Where(x => x.gameObject.activeInHierarchy).FirstOrDefault();

        if (activeSlot != null) activeSlot.gameObject.SetActive(false);

        if (index <= handSlots.Count - 1)
        {
            HandSlot currentSlot = handSlots[index];
            currentSlot.gameObject.SetActive(true);

            if (currentSlot.item as Weapon != null)
            {
                actionStateManager.SwitchCurrentWeapon((currentSlot.item as Weapon).weaponManager);
            }
        }
    }
    public void DestroyItem(int index)
    {
        handSlots[index].DestroyItem();
    }
}
