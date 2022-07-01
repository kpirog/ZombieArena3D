using UnityEngine;

public class Ammo : Interactable
{
    private ItemHolder itemHolder;

    public AmmoItem AmmoItem => ItemBase as AmmoItem;
    public AmmoType AmmoType => AmmoItem.Type;
    public bool CanPickUp => itemHolder.GetWeaponAmmo(AmmoType) != null;


    protected override void Awake()
    {
        base.Awake();
        itemHolder = FindObjectOfType<ItemHolder>();
    }
    protected override void PickUp()
    {
        if (CanPickUp)
        {
            tooltip.HideTooltip();
            WeaponManager currentWeapon = action.currentWeapon;

            if (action.currentWeapon != null)
            {
                if (currentWeapon.GetAmmoType() == AmmoType)
                {
                    currentWeapon.ammo.AddAmmo(AmmoItem.Amount);
                    EquipmentUI.Instance.SelectedSlot.onAmmoUpdated?.Invoke(currentWeapon.ammo.FullAmmo);
                    Destroy(gameObject);
                    return;
                }
            }

            WeaponAmmo weaponAmmo = itemHolder.GetWeaponAmmo(AmmoType);
            weaponAmmo.AddAmmo(AmmoItem.Amount);
            
            WeaponItem weaponItem = weaponAmmo.GetComponent<Interactable>().ItemBase as WeaponItem;
            EquipmentSlot weaponSlot = EquipmentUI.Instance.GetWeaponSlot(weaponItem);
            weaponSlot.onAmmoUpdated?.Invoke(weaponAmmo.FullAmmo);

            Destroy(gameObject);
        }
        else
        {
            isCollected = false;
            action.CanPickUp = false;
        }
    }

}
