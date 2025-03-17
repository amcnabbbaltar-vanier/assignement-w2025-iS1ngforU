using UnityEngine;
using Cinemachine;
using System.Threading;
using System.Collections;

[RequireComponent(typeof(Rigidbody))] // Ensures that a Rigidbody component is attached to the GameObject
public class CharacterMovement : MonoBehaviour
{
    // ============================== Movement Settings ==============================
    [Header("Movement Settings")]
    [SerializeField] private float baseWalkSpeed = 5f;    // Base speed when walking
    [SerializeField] private float baseRunSpeed = 8f;     // Base speed when running
    [SerializeField] private float rotationSpeed = 10f;   // Speed at which the character rotates

    // ============================== Jump Settings =================================
    [Header("Jump Settings")]
    [SerializeField] public float jumpForce = 5f;       // Jump force applied to the character
    [SerializeField] private float doubleJumpForce = 7f;   // Higher force for second jump
    [SerializeField] private float groundCheckDistance = 1.1f; // Distance to check for ground contact (Raycast)

    // ============================== Modifiable from other scripts ==================
    public float speedMultiplier = 1.0f; // Additional multiplier for character speed ( WINK WINK )

    // ============================== Private Variables ==============================
    private Rigidbody rb; // Reference to the Rigidbody component
    private Animator animator;
    private Transform cameraTransform; // Reference to the camera's transform
    private int count = 0;

    private float health = 0;
    private PlayerHealth playerHealth;


    // Input variables
    private float moveX; // Stores horizontal movement input (A/D or Left/Right Arrow)
    private float moveZ; // Stores vertical movement input (W/S or Up/Down Arrow)
    private bool jumpRequest; // Flag to check if the player requested a jump
    private Vector3 moveDirection; // Stores the calculated movement direction

 // ============================== Power-up Flags ==============================
    private bool canDoubleJump = false;
    private int jumpCount = 0;
    private bool hasFlipped = false;
    public AudioSource src;
    public AudioClip jumpFx;

    // ============================== Animation Variables ==============================
    [Header("Anim values")]
    public float groundSpeed; // Speed value used for animations

    public GameObject myGameObject;
    public string doubleJump = "JumpBoost";
    public string speedBoost = "SpeedBoost";
    public string points = "Points";

    // ============================== Character State Properties ==============================
    /// <summary>
    /// Checks if the character is currently grounded using a Raycast.
    /// If false, the character is in the air.
    /// </summary>
    public bool IsGrounded =>
        Physics.Raycast(transform.position + Vector3.up * 0.05f, Vector3.down, groundCheckDistance);

    /// <summary>
    /// Checks if the player is currently holding the "Run" button.
    /// </summary>
    private bool IsRunning => Input.GetButton("Run");

    // ============================== Unity Built-in Methods ==============================

    /// <summary>
    /// Called when the script is first initialized.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        animator = GetComponent<Animator>();

        if (Camera.main)
            cameraTransform = Camera.main.transform;
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    /// <summary>
    /// Called every frame, used to register player input.
    /// </summary>
    private void Update()
    {
        RegisterInput(); // Collect player input

    }

    /// <summary>
    /// Called every physics update (FixedUpdate ensures physics stability).
    /// </summary>
    private void FixedUpdate()
    {
        HandleMovement(); // Process movement and physics-based updates
    }

    // ============================== Initialization ==============================

    /// <summary>
    /// Initializes Rigidbody and camera reference.
    /// Also locks and hides the cursor for better control.
    /// </summary>
    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        rb.freezeRotation = true; // Prevent Rigidbody from rotating due to physics interactions
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smooth physics interpolation

        // Assign the main camera if available
        if (Camera.main)
            cameraTransform = Camera.main.transform;

        // Lock and hide the cursor for better gameplay control
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // ============================== Input Handling ==============================

    /// <summary>
    /// Reads player input values and registers movement/jump requests.
    /// </summary>
    private void RegisterInput()
    {
        moveX = Input.GetAxis("Horizontal"); // Get horizontal movement input
        moveZ = Input.GetAxis("Vertical");   // Get vertical movement input

        // Register a jump request if the player presses the Jump button
        if (Input.GetButtonDown("Jump"))
        {
            HandleJump();
        }
    }

    // ============================== Movement Handling ==============================

    /// <summary>
    /// Handles movement-related logic: calculating direction, jumping, rotating, and moving.
    /// </summary>
    private void HandleMovement()
    {
        CalculateMoveDirection(); // Compute the movement direction based on input
        RotateCharacter(); // Rotate the character towards the movement direction
        MoveCharacter(); // Move the character using velocity-based movement
        

    }

    /// <summary>
    /// Calculates the movement direction based on player input and camera orientation.
    /// </summary>
    private void CalculateMoveDirection()
    {
        // If the camera is not assigned, move based on world space
        if (!cameraTransform)
        {
            moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        }
        else
        {
            // Get forward and right vectors from the camera perspective
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Ignore Y-axis movement to prevent unwanted tilting
            forward.y = 0f;
            right.y = 0f;

            // Normalize vectors to maintain consistent movement speed
            forward.Normalize();
            right.Normalize();

            // Calculate movement direction relative to the camera orientation
            moveDirection = (forward * moveZ + right * moveX).normalized;
        }
    }

    /// <summary>
    /// Handles jumping by applying an impulse force if the character is grounded.
    /// </summary>
    private void HandleJump()
    {
        if (IsGrounded)
        {
            // First jump
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            animator.SetTrigger("Jump");
            jumpCount = 1;
        }
        else if (jumpCount == 1 && canDoubleJump) // Allow second jump if power-up is active
        {
            rb.velocity = new Vector3(rb.velocity.x, doubleJumpForce, rb.velocity.z);
            src.clip = jumpFx;
            src.Play();

            if (!hasFlipped)
            {
                animator.SetTrigger("doFlip");
                hasFlipped = true;
            }

            jumpCount++; // Prevent further jumps
        }
    }

    
    

    /// <summary>
    /// Rotates the character towards the movement direction.
    /// </summary>
    private void RotateCharacter()
    {
        // Rotate only if the character is moving
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Moves the character using Rigidbody's velocity instead of MovePosition.
    /// This ensures smooth movement while avoiding physics conflicts.
    /// </summary>
    private void MoveCharacter()
    {
        // Determine movement speed (walking or running)
        float speed = IsRunning ? baseRunSpeed : baseWalkSpeed;

        // Set ground speed value for animation purposes
        groundSpeed = (moveDirection != Vector3.zero) ? speed : 0.0f;

        // Preserve the current Y velocity to maintain gravity effects
        Vector3 newVelocity = new Vector3(
            moveDirection.x * speed * speedMultiplier,
            rb.velocity.y, // Keep the existing Y velocity for jumping & gravity
            moveDirection.z * speed * speedMultiplier
        );

        // Apply the new velocity directly
        rb.velocity = newVelocity;
    }

    

    private void OnTriggerEnter(Collider other)
        {
            // Check if the colliding object has the specified tag (optional)
            if (other.gameObject.CompareTag(doubleJump))
            {
                canDoubleJump = true;  // Enable double jump
                Debug.Log("Double jump enabled!");
                StartCoroutine(DisableDoubleJumpAfterDelay(5f)); // Disable after 5 seconds

            }

            if (other.gameObject.CompareTag(speedBoost))
            {
                baseWalkSpeed += 3;
                baseRunSpeed += 3;
                Debug.Log("speed has increased");

                StartCoroutine(ResetSpeedAfterDelay(5f));
                
            }

            if (other.gameObject.CompareTag(points))
            {
                count += 50;

            }
            
        }

        private IEnumerator ResetSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        baseWalkSpeed -= 3; // Reset walk speed
        baseRunSpeed -= 3;  // Reset run speed
        Debug.Log("Speed boost expired. Speed reset.");
    }

    private IEnumerator DisableDoubleJumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDoubleJump = false;  // Disable double jump after time
        Debug.Log("Double jump disabled.");
    }


    public void GotHit()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(3);
            animator.SetTrigger("GotHit");
            
        }
    }

}

