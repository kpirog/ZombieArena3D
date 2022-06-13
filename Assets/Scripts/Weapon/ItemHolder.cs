using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public List<Interactable> interactables;

    private void Awake()
    {
        interactables = new List<Interactable>(GetComponentsInChildren<Interactable>());
    }
    public void CreateItem(Interactable interactable)
    {
        interactable = Instantiate(interactable, transform.position, Quaternion.identity, transform);
        interactables.Add(interactable);
        
        Weapon weapon = interactable as Weapon;
        if (weapon != null) weapon.IsInEquipment = true;

        interactable.gameObject.SetActive(false);

        if (interactables.Count == 1) SetItemActive(0);
    }
    public void SetItemActive(int index)
    {
        if (interactables.Count == 0 || index > interactables.Count - 1) return;

        Interactable activeItem = interactables.Where(x => x.gameObject.activeInHierarchy).FirstOrDefault();

        if (activeItem != null) activeItem.gameObject.SetActive(false);

        interactables[index].gameObject.SetActive(true);
    }
}
