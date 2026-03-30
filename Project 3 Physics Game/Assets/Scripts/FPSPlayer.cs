using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSPlayer: MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5;
    public float runSpeed = 9;
    public float jumpForce = 5f;
    private bool isRunning;
    private bool jumpReady;

    [Header("Camera")]
    public Transform cameraTransform;
    public float lookSensitivity = 100;
    private float yaw;
    private float pitch;

    private Rigidbody rb;

    private Vector2 moveInput;
    private Vector2 lookInput;

    [Header("Grounding")]
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.5f;
    public float groundCheckDistance = 0.5f;
    public bool isGrounded;
    public Transform groundCheck;

    public ObjectGrabber grabber;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //optional lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {  
        CameraLook();
        CheckGround();
    }

    private void FixedUpdate()
    {
        float currentSpeed;
        //checking the state of our variable and 
        if (isRunning)
        {
            currentSpeed = runSpeed;
        } else
        {
            currentSpeed = walkSpeed;
        }


        Vector3 move = transform.forward * moveInput.y * currentSpeed +
            transform.right * moveInput.x * currentSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        //come back here to finish our jump
        if(jumpReady && isGrounded)
        {
            jumpReady = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void CameraLook()
    {
        //if we have no camera, exit the function
        if (cameraTransform == null) return;

        //mouse scaled by sensitivity and frame time
        float mouseX = lookInput.x * lookSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * lookSensitivity * Time.deltaTime;

        //left and right
        yaw += mouseX;
        transform.rotation = Quaternion.Euler(0, yaw, 0);

        //vertical rotation rotates the camera only
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90, 90);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Taking our movement input and saving it inside the variable, then passing it to our movement code
        moveInput = context.ReadValue<Vector2>();
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        if (grabber.BroadcastRotate()) return;
        //taking our input from our mouse and storing inside this variable which gets passed into our CameraLook function
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) jumpReady = true;
    }

    public void OnSprint (InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }
    private void CheckGround()
    {
        //if we do not have a ground check, turn it to false and exit the function
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        //inside of the sphere its going to check groundCheck position, the radius, the radius distance and the layer the player is on and then it will turn it true or false
        isGrounded = Physics.SphereCast(groundCheck.position, groundCheckRadius, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        //visualize end position of the spherecast
        Vector3 end = groundCheck.position + Vector3.down * groundCheckDistance;

        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(end, groundCheckRadius);
    }
}
