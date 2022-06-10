using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform barrel;
    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private AudioClip shotSound;

    [SerializeField] private int bulletsPerShot;
    [SerializeField] private float fireRate = 0.2f;

    [Header("Weapon position settings")]

    [SerializeField] private Vector3 weaponPosition;
    [SerializeField] private Vector3 weaponRotation;
    [SerializeField] private Vector3 rightHintPosition;
    [SerializeField] private Vector3 rightHintRotation;
    [SerializeField] private Vector3 rightTargetPosition;
    [SerializeField] private Vector3 rightTargetRotation;
    [SerializeField] private Vector3 leftHintPosition;
    [SerializeField] private Vector3 leftHintRotation;
    [SerializeField] private Vector3 leftTargetPosition;
    [SerializeField] private Vector3 leftTargetRotation;

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

    [Header("Light settings")]
    [SerializeField] private float lightIntensity = 2f;
    [SerializeField] private float intensitySpeed = 10f;

    private ParticleSystem muzzleFlashParticles;
    private Light muzzleFlashLight;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        action = FindObjectOfType<ActionStateManager>();
        shootAction = playerInput.actions["Shoot"];
    }
    private void OnEnable()
    {
        SetWeaponPosition();
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
        aim = FindObjectOfType<AimStateManager>();
        audioSource = GetComponent<AudioSource>();
        ammo = GetComponent<WeaponAmmo>();
        recoil = GetComponent<WeaponRecoil>();
        bloom = GetComponent<WeaponBloom>();
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
    private void SetWeaponPosition()
    {
        transform.localPosition = new Vector3(weaponPosition.x, weaponPosition.y, weaponPosition.z);
        transform.localRotation = Quaternion.Euler(weaponRotation.x, weaponRotation.y, weaponRotation.z);

        action.leftHintTransform.localPosition = new Vector3(leftHintPosition.x, leftHintPosition.y, leftHintPosition.z);
        action.leftHintTransform.localRotation = Quaternion.Euler(leftHintRotation.x, leftHintRotation.y, leftHintRotation.z);

        action.leftTargetTransform.localPosition = new Vector3(leftTargetPosition.x, leftTargetPosition.y, leftTargetPosition.z);
        action.leftTargetTransform.localRotation = Quaternion.Euler(leftTargetRotation.x, leftTargetRotation.y, leftTargetRotation.z);

        action.rightHintTransform.localPosition = new Vector3(rightHintPosition.x, rightHintPosition.y, rightHintPosition.z);
        action.rightHintTransform.localRotation = Quaternion.Euler(rightHintRotation.x, rightHintRotation.y, rightHintRotation.z);

        action.rightTargetTransform.localPosition = new Vector3(rightTargetPosition.x, rightTargetPosition.y, rightTargetPosition.z);
        action.rightTargetTransform.localRotation = Quaternion.Euler(rightTargetRotation.x, rightTargetRotation.y, rightTargetRotation.z);
    }
}
