using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/Consumable Item")]
public class ConsumableItem : ItemBase
{
    [Header("Consumable settings")]
    [SerializeField] private float healthRegen;
    [SerializeField] private float manaRegen;
    [SerializeField] private int amount;

    public float HealthRegen => healthRegen;
    public float ManaRegen => manaRegen;
    public override int Amount => amount;
}