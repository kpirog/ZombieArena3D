using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    public int clipSize;
    [SerializeField] private int extraAmmo;
    private int fullAmmo;

    public AudioClip magInSound;
    public AudioClip magOutSound;
    public AudioClip releaseSlideSound;

    public AmmoType ammoType;

    [HideInInspector] public int currentAmmo;
    public bool CanReload => currentAmmo < clipSize && extraAmmo > 0;
    public int FullAmmo
    {
        get => currentAmmo + extraAmmo;
        set
        {
            fullAmmo = value;
        }
    }

    private void Start()
    {
        currentAmmo = clipSize;
    }
    public void Reload()
    {
        if (currentAmmo < clipSize)
        {
            int neededAmmo = clipSize - currentAmmo;

            if (extraAmmo > neededAmmo)
            {
                currentAmmo = clipSize;
                extraAmmo -= neededAmmo;
            }
            else if (extraAmmo < neededAmmo)
            {
                currentAmmo += extraAmmo;
                extraAmmo = 0;
            }
            else
            {
                currentAmmo = clipSize;
                extraAmmo = 0;
            }
        }
    }
    public void AddAmmo(int ammo) => extraAmmo += ammo;
}
