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

    public MultiAimConstraint rHandRig;
    public TwoBoneIKConstraint lHandIK;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        reloadAction = playerInput.actions["Reload"];
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
