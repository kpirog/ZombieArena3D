using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    [Header("Item base settings")]
    [SerializeField] private Interactable itemPrefab;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private ItemRarity itemRarity;
    [SerializeField] private int sellPrice;

    public Interactable ItemPrefab => itemPrefab;
    public Sprite ItemIcon => itemIcon;
    public ItemRarity ItemRarity => itemRarity;
    public int SellPrice => sellPrice;
    public abstract int Amount { get; }
}