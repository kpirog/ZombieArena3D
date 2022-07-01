using UnityEngine;

public class HandSlot : MonoBehaviour
{
    public Interactable item;

    public void CreateItem(Interactable interactable)
    {
        if (item == null)
        {
            interactable = Instantiate(interactable, transform.position, Quaternion.identity, transform);
            item = interactable;

            Weapon weapon = item as Weapon;
            if (weapon != null) weapon.IsInEquipment = true;
            weapon.anim.SetBool("IsInEquipment", true);
        }
    }
    public void DestroyItem()
    {
        Destroy(item.gameObject);
        item = null;
    }
    public WeaponAmmo GetWeaponAmmo()
    {
        if (item as Weapon != null)
        {
            return (item as Weapon).weaponAmmo;
        }

        return null;
    }
}
