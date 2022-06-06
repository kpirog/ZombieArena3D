using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform barrel;
    [SerializeField] private Bullet bulletPrefab;
    
    [SerializeField] private AudioClip shotSound;

    [SerializeField] private int bulletsPerShot;
    [SerializeField] private float fireRate = 0.2f;

    private float fireRateTimer;
    public bool IsShooting { get; private set; } = false;

    private AimStateManager aim;
    private ActionStateManager action;
    private AudioSource audioSource;
    [HideInInspector] public WeaponAmmo ammo;
    private WeaponRecoil recoil;
    private WeaponBloom bloom;
    private PlayerInput playerInput;
    private InputAction shootAction;

    [SerializeField] private float lightIntensity = 2f;
    [SerializeField] private float intensitySpeed = 10f;

    private ParticleSystem muzzleFlashParticles;
    private Light muzzleFlashLight;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        shootAction = playerInput.actions["Shoot"];
    }
    private void OnEnable()
    {
        shootAction.performed += ctx => IsShooting = true;
        shootAction.canceled += ctx => IsShooting = false;
    }
    private void OnDisable()
    {
        shootAction.performed += ctx => IsShooting = true;
        shootAction.canceled += ctx => IsShooting = false;
    }
    private void Start()
    {
        aim = GetComponentInParent<AimStateManager>();
        audioSource = GetComponent<AudioSource>();
        ammo = GetComponent<WeaponAmmo>();
        recoil = GetComponent<WeaponRecoil>();
        bloom = GetComponent<WeaponBloom>();
        action = GetComponentInParent<ActionStateManager>();
        muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
        muzzleFlashLight = GetComponentInChildren<Light>();
        lightIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0f;
        fireRateTimer = fireRate;
    }

    private void Update()
    {
        if (CanShoot() && shootAction.triggered && ammo.currentAmmo > 0) Shoot();
        if (CanShoot() && IsShooting && ammo.currentAmmo > 0) Shoot();

        muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0f, intensitySpeed * Time.deltaTime);
    }
    private void Shoot()
    {
        fireRateTimer = 0f;
        barrel.LookAt(aim.aimPos);
        barrel.localEulerAngles = bloom.SetBloom(barrel);
        audioSource.PlayOneShot(shotSound);
        recoil.TriggerRecoil();
        TriggerMuzzleFlash();
        ammo.currentAmmo--;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation);
            bullet.Release();
        }
    }
    private bool CanShoot()
    {
        fireRateTimer += Time.deltaTime;

        if (fireRateTimer >= fireRate) return true;
        return false;
    }
    private void TriggerMuzzleFlash()
    {
        muzzleFlashParticles.Play();
        muzzleFlashLight.intensity = lightIntensity;
    }
}
