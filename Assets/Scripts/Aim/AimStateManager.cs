using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class AimStateManager : MonoBehaviour
{
    [HideInInspector] public AimBaseState currentState;

    public DefaultState Default = new DefaultState();
    public AimState Aim = new AimState();

    #region Aim
    [Header("Aim settings")]
    public float aimFov = 40f;
    [SerializeField] private LayerMask aimLayerMask;
    public Transform aimPos;
    [SerializeField] private float aimSmoothSpeed = 20f;

    [HideInInspector] public float defaultFov;
    [HideInInspector] public float currentFov;
    [SerializeField] private float smoothVCamZoom = 10f;

    private CinemachineVirtualCamera vCam;
    public bool IsAiming { get; private set; }
    #endregion

    #region Look
    [Header("Look options")]
    [SerializeField] private Transform followCamTransform;
    [SerializeField] private float mouseSens;
    private float mouseX, mouseY;
    private bool mouseInverted;
    #endregion

    #region SwapShoulder
    [Header("Shoulder swap settings")]
    private float xFollowPos, yFollowPos;
    private float originYPos;
    [SerializeField] private float swapShoulderSpeed = 10f;
    [SerializeField] private float crouchHeight = 0.6f;
    #endregion


    private MovementStateManager movement;
    [HideInInspector] public HudUI hudUI;
    [HideInInspector] public Animator anim;
    [HideInInspector] public EquipmentUI equipmentUI;
    public Transform recoilTransform;
    private PlayerInput playerInput;
    private InputAction lookAction;
    private InputAction aimAction;
    private InputAction shoulderSwapAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];
        aimAction = playerInput.actions["Aim"];
        shoulderSwapAction = playerInput.actions["ShoulderSwap"];
        equipmentUI = FindObjectOfType<EquipmentUI>();
    }
    private void OnEnable()
    {
        aimAction.performed += ctx => IsAiming = true;
        aimAction.canceled += ctx => IsAiming = false;
    }
    private void OnDisable()
    {
        aimAction.performed -= ctx => IsAiming = true;
        aimAction.canceled -= ctx => IsAiming = false;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        movement = GetComponent<MovementStateManager>();
        hudUI = FindObjectOfType<HudUI>();
        defaultFov = vCam.m_Lens.FieldOfView;
        xFollowPos = followCamTransform.localPosition.x;
        originYPos = followCamTransform.localPosition.y;
        yFollowPos = originYPos;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mouseSens = PlayerPrefs.GetFloat("masterSensitivity");
        mouseInverted = PlayerPrefs.GetInt("masterInvertY") > 0;

        SwitchState(Default);
    }
    private void Update()
    {
        mouseX += lookAction.ReadValue<Vector2>().x * mouseSens;

        if (mouseInverted) mouseY -= lookAction.ReadValue<Vector2>().y * mouseSens;
        else mouseY += lookAction.ReadValue<Vector2>().y * mouseSens;

        mouseY = Mathf.Clamp(mouseY, -80f, 80f);

        vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, currentFov, smoothVCamZoom * Time.deltaTime);

        Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCentre);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimLayerMask))
        {
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);
        }

        MoveCamera();
        currentState.UpdateState(this);
    }
    private void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, mouseX, transform.localEulerAngles.z);
        followCamTransform.localEulerAngles = new Vector3(mouseY, followCamTransform.localEulerAngles.y, followCamTransform.localEulerAngles.z);
    }
    public void SwitchState(AimBaseState state)
    {
        if (state is AimState && equipmentUI.EquippedItem is ConsumableItem)
        {
            return;
        }

        currentState = state;
        currentState.EnterState(this);
    }
    private void MoveCamera()
    {
        if (shoulderSwapAction.triggered) xFollowPos = -xFollowPos;
        if (movement.currentState == movement.Crouch) yFollowPos = crouchHeight;
        else yFollowPos = originYPos;

        Vector3 newFollowPos = new Vector3(xFollowPos, yFollowPos, followCamTransform.localPosition.z);
        followCamTransform.localPosition = Vector3.Lerp(followCamTransform.localPosition, newFollowPos, swapShoulderSpeed * Time.deltaTime);
    }
}
