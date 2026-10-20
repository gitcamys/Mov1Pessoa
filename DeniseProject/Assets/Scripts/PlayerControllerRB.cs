
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerControllerRB : MonoBehaviour
{
    [SerializeField] private float  WalkSpeed = 5.5f;
    [SerializeField] private float  RunningSpeed = 9.0f;
    [SerializeField] private float JumpForce =8.0f;

    private Rigidbody rb; 
    private Vector2 moveVector = Vector2.zero; 
    private float currentMoveSpeed = 0.0f; 
    private bool jumpRequested = false;
    [SerializeField] private float groundCheckExtra = 0.1f; 
    private CapsuleCollider capsule; 
    [SerializeField] private LayerMask groundMask; 
    private bool isGrounded; 

    private InputAction moveInput; 
    private InputAction runInput;
     private InputAction jumpInput;
    private InputAction lookInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        rb.freezeRotation = true; 
        capsule = GetComponent<CapsuleCollider>(); 

        moveInput = InputSystem.actions.FindAction("Move");
        runInput = InputSystem.actions.FindAction("Sprint");
        jumpInput = InputSystem.actions.FindAction("Jump");
        lookInput = InputSystem.actions.FindAction("Look");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentMoveSpeed = WalkSpeed;

        capsule = GetComponent<CapsuleCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        moveVector = moveInput.ReadValue<Vector2>();
        if (jumpInput.WasPressedThisFrame()) 
        {
            jumpRequested = true;
        }
    }
    private void FixedUpdate() 
    { 
        Vector3 move = (transform.forward * moveVector.y + transform.right * moveVector.x) * currentMoveSpeed;

        float verticalVelocity = rb.linearVelocity.y;

        float distance = capsule.height / 2f + groundCheckExtra; 
        Vector3 origin = transform.TransformPoint(capsule.center); 
        isGrounded = Physics.Raycast(origin, Vector3.down, distance, groundMask); 
        if(jumpRequested && isGrounded) 
        { 
            verticalVelocity = JumpForce;
        } 
            jumpRequested =false;

        rb.linearVelocity = new Vector3(move.x, verticalVelocity, move.z);
}
}

/*
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerControllerRB : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float walkSpeed = 5.5f;
    [SerializeField] private float runningSpeed = 9.0f;
    [SerializeField] private float jumpForce = 8.0f;

    [Header("Detecção de Chão")]
    [SerializeField] private float groundCheckExtra = 0.1f;
    [SerializeField] private LayerMask groundMask;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference runAction;
    [SerializeField] private InputActionReference jumpAction;

    private Rigidbody rb;
    private CapsuleCollider capsule;
    private Vector2 moveVector = Vector2.zero;
    private float currentMoveSpeed;
    private bool jumpRequested;
    private bool isGrounded;

    private void OnEnable()
    {
        moveAction.action.Enable();
        runAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        runAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        // Impede que a física do Rigidbody capote o personagem
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentMoveSpeed = walkSpeed;
    }

    private void Update()
    {
        // Leitura de inputs
        moveVector = moveAction.action.ReadValue<Vector2>();

        // Alterna velocidade entre andar e correr
        currentMoveSpeed = runAction.action.IsPressed() ? runningSpeed : walkSpeed;

        // Captura o comando de pulo
        if (jumpAction.action.WasPressedThisFrame() && isGrounded)
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
        HandleMovement();
        HandleJump();
    }

    private void CheckGround()
    {
        // Verifica se o CapsuleCollider está tocando no chão usando SphereCast ou Raycast
        Vector3 rayOrigin = transform.position + capsule.center;
        float rayDistance = (capsule.height / 2f) + groundCheckExtra;

        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, rayDistance, groundMask);
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = (transform.forward * moveVector.y + transform.right * moveVector.x) * currentMoveSpeed;

        // Mantém a velocidade vertical da gravidade e aplica a velocidade horizontal da física
        Vector3 currentVelocity = rb.linearVelocity; // Use rb.velocity se estiver na Unity 2022 ou anterior
        rb.linearVelocity = new Vector3(moveDirection.x, currentVelocity.y, moveDirection.z);
    }

    private void HandleJump()
    {
        if (jumpRequested)
        {
            // Aplica a força de pulo de forma instantânea
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
        }
    }
}

*/