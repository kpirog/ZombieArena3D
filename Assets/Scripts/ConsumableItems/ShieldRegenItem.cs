using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRegenItem : Consumable
{
    public override void Use()
    {
        if (IsInEquipment)
        {
            playerStats.AddShield(ConsumableItem.ShieldRegen);
            Destroy(gameObject);
        }
    }
}
