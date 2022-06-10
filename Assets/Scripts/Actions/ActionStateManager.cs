using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class ActionStateManager : MonoBehaviour
{
    [HideInInspector] public ActionBaseState currentState;

    public ReloadState Reload = new ReloadState();
    public NormalState Normal = new NormalState();

    public WeaponManager currentWeapon; 
    
    [HideInInspector] public Animator anim;
    private AudioSource audioSource;
    private PlayerInput playerInput;
    [HideInInspector] public InputAction reloadAction;
    [HideInInspector] public InputAction pickUpAction;

    public MultiAimConstraint rHandRig;
    public TwoBoneIKConstraint lHandIK;
    public Transform weaponSlot;
    public Transform rightHintTransform;
    public Transform rightTargetTransform;
    public Transform leftHintTransform;
    public Transform leftTargetTransform;

    public bool CanPickUp { get; set; } = false;
    public bool IsPickingUp => CanPickUp;
    

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        currentWeapon = GetComponentInChildren<WeaponManager>();
        reloadAction = playerInput.actions["Reload"];
        pickUpAction = playerInput.actions["PickUp"];
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = currentWeapon.GetComponent<AudioSource>();
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
    public void MagIn() => audioSource.PlayOneShot(currentWeapon.ammo.magInSound);
    public void MagOut() => audioSource.PlayOneShot(currentWeapon.ammo.magOutSound);
    public void ReleaseSlide() => audioSource.PlayOneShot(currentWeapon.ammo.releaseSlideSound);
}
