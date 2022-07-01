using UnityEngine;

public class Weapon : Interactable
{
    [HideInInspector] public WeaponManager weaponManager;
    [HideInInspector] public WeaponAmmo weaponAmmo;
    private WeaponRecoil weaponRecoil;
    private WeaponBloom weaponBloom;

    public override bool IsInEquipment 
    {
        get => isInEquipment;
        set
        {
            SetRarityParticle(value);
            ToggleComponents(value);
            isInEquipment = value;
        } 
    } 
    protected override void Awake()
    {
        base.Awake();
        
        weaponManager = GetComponent<WeaponManager>();
        weaponAmmo = GetComponent<WeaponAmmo>();
        weaponRecoil = GetComponent<WeaponRecoil>();
        weaponBloom = GetComponent<WeaponBloom>();
    }
    private void OnEnable()
    {
        anim.SetBool("IsInEquipment", IsInEquipment);
    }
    public void ToggleComponents(bool enabled)
    {
        weaponManager.enabled = enabled;
        weaponAmmo.enabled = enabled;
        weaponRecoil.enabled = enabled;
        weaponBloom.enabled = enabled;
    }
}
