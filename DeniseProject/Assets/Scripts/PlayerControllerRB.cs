
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

    private Camera mainCamera;

    [SerializeField] private float LookSensivity =0.2f;
    [SerializeField] private float LookAngleLimit = 90f;
    private float lookAngle = 0.0f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        rb.freezeRotation = true; 
        mainCamera =GetComponentInChildren<Camera>();
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
        currentMoveSpeed = runInput.IsPressed() ? RunningSpeed : WalkSpeed;
        Vector2 lookVector = lookInput.ReadValue<Vector2>();
        HandleCamera(lookVector);

        
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
    private void HandleCamera(Vector2 lookVector)
    {
        lookAngle += -lookVector.y * LookSensivity;
        lookAngle = Mathf.Clamp(lookAngle, -LookAngleLimit, LookAngleLimit);
        mainCamera.transform.localRotation = Quaternion.Euler(lookAngle, 0, 0);
        transform.rotation *= Quaternion.Euler (0, lookVector.x * LookSensivity, 0);
    }
}

