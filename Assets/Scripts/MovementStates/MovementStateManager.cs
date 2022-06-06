using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class MovementStateManager : MonoBehaviour
{
    [HideInInspector] public MovementBaseState currentState;

    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public RunState Run = new RunState();
    public CrouchState Crouch = new CrouchState();

    #region Movement
    [Header("Movement Settings")]

    public float runSpeed = 7f, runBackSpeed = 5f;
    public float walkSpeed = 4f, walkBackSpeed = 2f;
    public float crouchSpeed = 2f, crouchBackSpeed = 1f;

    [HideInInspector] public float currentSpeed;
    [SerializeField] private float smoothMovementSpeed = 0.125f;
    public bool IsSprinting { get; private set; }
    public bool isCrouching { get; private set; } = false;

    [HideInInspector] public Vector3 direction;
    [HideInInspector] public Vector2 input;
    private Vector2 smoothVelocity;
    private Vector2 smoothInput;
    
    private CharacterController characterController;
    #endregion

    #region Gravity    
    [SerializeField] private float gravity = -9.81f;
    private Vector3 velocity;
    #endregion

    #region GroundCheck
    [SerializeField] private float groundCheckOffset;
    [SerializeField] private LayerMask groundMask;
    private Vector3 spherePos;
    #endregion 

    #region InputActions
    private InputAction moveAction;
    [HideInInspector] public InputAction sprintAction;
    [HideInInspector] public InputAction crouchAction;
    private PlayerInput playerInput;
    private string startControlScheme;
    #endregion

    #region Animation
    private int vInputParameter = Animator.StringToHash("vInput");
    private int hzInputParameter = Animator.StringToHash("hzInput");
    [HideInInspector] public Animator anim;
    #endregion
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        sprintAction = playerInput.actions["Sprint"];
        crouchAction = playerInput.actions["Crouch"];
        startControlScheme = PlayerPrefs.GetString("controlScheme");
        playerInput.SwitchCurrentControlScheme(startControlScheme);
        playerInput.defaultControlScheme = startControlScheme;
        playerInput.enabled = false;
        playerInput.enabled = true;
    }
    private void OnEnable()
    {
        sprintAction.performed += ctx => IsSprinting = true;
        sprintAction.canceled += ctx => IsSprinting = false;
        crouchAction.started += ctx => isCrouching = !isCrouching;
    }
    private void OnDisable()
    {
        sprintAction.performed -= ctx => IsSprinting = true;
        sprintAction.canceled -= ctx => IsSprinting = false;
        crouchAction.started -= ctx => isCrouching = !isCrouching;
    }
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        SwitchState(Idle);
    }

    private void Update()
    {
        Move();
        Gravity();

        currentState.UpdateState(this);
    }
    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    private void Move()
    {
        input = moveAction.ReadValue<Vector2>();
        smoothInput = Vector2.SmoothDamp(smoothInput, input, ref smoothVelocity, smoothMovementSpeed);

        direction = transform.right * smoothInput.x + transform.forward * smoothInput.y;
        
        anim.SetFloat(vInputParameter, smoothInput.y);
        anim.SetFloat(hzInputParameter, smoothInput.x);

        characterController.Move(direction * currentSpeed * Time.deltaTime);
    }
    private bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundCheckOffset, transform.position.z);

        if (Physics.CheckSphere(spherePos, characterController.radius - 0.05f, groundMask)) return true;
        return false;
    }
    private void Gravity()
    {
        if(!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if(velocity.y < 0f) velocity.y = -2f;

        characterController.Move(velocity * Time.deltaTime);
    }
}
