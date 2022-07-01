using UnityEngine;

public class HandSlot : MonoBehaviour
{
    public Interactable item;

    public void CreateItem(Interactable interactable)
    {
        if (item == null)
        {
            Interactable newInteractable = Instantiate(interactable, transform.position, Quaternion.identity, transform);
            newInteractable.IsInEquipment = true;
            newInteractable.anim.SetBool("IsInEquipment", true);
            item = newInteractable;
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
