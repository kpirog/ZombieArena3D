using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    public int clipSize;
    [SerializeField] private int extraAmmo;

    public AudioClip magInSound;
    public AudioClip magOutSound;
    public AudioClip releaseSlideSound;

    [HideInInspector] public int currentAmmo;
    public bool CanReload => currentAmmo < clipSize && extraAmmo > 0;

    private void Start()
    {
        currentAmmo = clipSize;
    }

    public void Reload()
    {
        if(currentAmmo < clipSize)
        {
            int neededAmmo = clipSize - currentAmmo;

            if (extraAmmo > neededAmmo)
            {
                currentAmmo = clipSize;
                extraAmmo -= neededAmmo;
            }
            else if(extraAmmo < neededAmmo)
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
}
