using UnityEngine;

public class ShieldBar : BaseNeedBar
{
    public override float FillValue => playerStats.CurrentShield;
    public override float MaxFillValue => playerStats.StatsModel.MaxShield;
    protected override void UpdateUI()
    {
        base.UpdateUI();
    }
}
