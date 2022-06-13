using UnityEngine;

public class Weapon : Interactable
{
    private WeaponManager weaponManager;
    private WeaponAmmo weaponAmmo;
    private WeaponRecoil weaponRecoil;
    private WeaponBloom weaponBloom;

    private bool isInEquipment;
    public bool IsInEquipment 
    {
        get => isInEquipment;
        set
        {
            ToggleComponents(value);
            isInEquipment = value;
        } 
    } 
    private void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        weaponAmmo = GetComponent<WeaponAmmo>();
        weaponRecoil = GetComponent<WeaponRecoil>();
        weaponBloom = GetComponent<WeaponBloom>();
    }
    public void ToggleComponents(bool enabled)
    {
        weaponManager.enabled = enabled;
        weaponAmmo.enabled = enabled;
        weaponRecoil.enabled = enabled;
        weaponBloom.enabled = enabled;
    }
}
