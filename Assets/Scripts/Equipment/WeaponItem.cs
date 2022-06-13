using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Items/Weapon Item")]
public class WeaponItem : ItemBase
{
    [Header("Weapon settings")]
    [SerializeField] private int ammo;

    public override int Amount => ammo;
}
