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
        HandSlot currentSlot = handSlots[slotIndex];
        
        currentSlot.gameObject.SetActive(true);
        currentSlot.CreateItem(interactable);
        currentSlot.gameObject.SetActive(false);
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
    public WeaponAmmo GetWeaponAmmo(AmmoType ammoType)
    {
        List<WeaponAmmo> ammos = new List<WeaponAmmo>();
        ammos.Clear();

        foreach (HandSlot slot in handSlots)
        {
            if (slot.GetWeaponAmmo() == null) continue;

            ammos.Add(slot.GetWeaponAmmo());
        }

        if(ammos.Count > 0)
        {
            WeaponAmmo ammo = ammos.Where(x => x.ammoType == ammoType).FirstOrDefault();

            return ammo;
        }

        return null;
    }
}
