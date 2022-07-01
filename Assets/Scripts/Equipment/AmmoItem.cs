using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Item", menuName = "Items/Ammo Item")]
public class AmmoItem : ItemBase
{
    [Header("Ammo settings")]
    [SerializeField] private int amount;
    [SerializeField] private AmmoType type;
    public override int Amount => amount;
    public AmmoType Type => type;
}
public enum AmmoType
{
    Heavy, Light, Shotgun
}
