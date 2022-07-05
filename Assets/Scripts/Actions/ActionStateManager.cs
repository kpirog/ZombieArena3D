using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class ActionStateManager : MonoBehaviour
{
    [HideInInspector] public ActionBaseState currentState;

    public ReloadState Reload = new ReloadState();
    public NormalState Normal = new NormalState();

    [HideInInspector] public WeaponManager currentWeapon;

    [HideInInspector] public Animator anim;
    private PlayerInput playerInput;
    [HideInInspector] public InputAction reloadAction;
    [HideInInspector] public InputAction pickUpAction;

    public MultiAimConstraint rHandRig;
    public TwoBoneIKConstraint lHandIK;
    [HideInInspector] public RigController rigController;

    public Transform rightHintTransform;
    public Transform rightTargetTransform;
    public Transform leftHintTransform;
    public Transform leftTargetTransform;

    [HideInInspector] public EquipmentUI equipmentUI;
    public AudioSource AudioSource => currentWeapon.AudioSource;
    public bool CanPickUp { get; set; } = false;
    public bool IsPickingUp => CanPickUp;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        currentWeapon = GetComponentInChildren<WeaponManager>();
        reloadAction = playerInput.actions["Reload"];
        pickUpAction = playerInput.actions["PickUp"];
        rigController = FindObjectOfType<RigController>();
        equipmentUI = FindObjectOfType<EquipmentUI>();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        SwitchState(Normal);
    }
    private void Update()
    {
        currentState.UpdateState(this);
    }
    public void SwitchState(ActionBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    public void WeaponReloaded()
    {
        SwitchState(Normal);
        currentWeapon.ammo.Reload();
    }
    public void SwitchCurrentWeapon(WeaponManager newWeapon) => currentWeapon = newWeapon;
    public void MagIn() => AudioSource.PlayOneShot(currentWeapon.ammo.magInSound);
    public void MagOut() => AudioSource.PlayOneShot(currentWeapon.ammo.magOutSound);
    public void ReleaseSlide() => AudioSource.PlayOneShot(currentWeapon.ammo.releaseSlideSound);
}
