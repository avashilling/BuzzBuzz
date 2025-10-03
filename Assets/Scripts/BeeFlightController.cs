using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BeeFlightController : MonoBehaviour
{
    [Header("References")]
    public Transform beeModel;   // mesh/visual child

    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float acceleration = 20f;
    public float deceleration = 15f;
    public float turnSpeed = 120f;   // degrees per second

    [Header("Tilt Settings")]
    public float forwardTilt = 15f; // tilt forward/back
    public float turnTilt = 20f;    // bank left/right
    public float tiltSmooth = 6f;

    private Rigidbody rb;
    private Vector3 currentVelocity;
    private Vector3 targetVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        HandleInput();
        MovePlayer();

    }

    private void LateUpdate()
    {
        HandleTilt();
    }

    void HandleInput()
    {
        // --- Rotation (Yaw) with A/D ---
        float turnInput = 0f;
        if (Input.GetKey(KeyCode.A)) turnInput = -1f;
        if (Input.GetKey(KeyCode.D)) turnInput = 1f;

        transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.deltaTime);

        // --- Forward/Backward with W/S (kept inverted to match your orientation) ---
        float forwardInput = 0f;
        if (Input.GetKey(KeyCode.W)) forwardInput = -1f;
        if (Input.GetKey(KeyCode.S)) forwardInput = 1f;

        // --- Vertical: Space = up, Shift = down (either Shift key) ---
        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.Space)) verticalInput = 1f;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) verticalInput = -1f;

        // Target velocity in local space (forward/back + up/down only)
        Vector3 inputDir = new Vector3(0f, verticalInput, forwardInput).normalized;
        targetVelocity = transform.TransformDirection(inputDir) * moveSpeed;
    }

    void MovePlayer()
    {
        // Smooth accel/decel
        currentVelocity = Vector3.MoveTowards(
            currentVelocity,
            targetVelocity,
            (targetVelocity.magnitude > currentVelocity.magnitude ? acceleration : deceleration) * Time.fixedDeltaTime
        );

        // Apply to Rigidbody (use rb.velocity)
        rb.linearVelocity  = currentVelocity;
    }

    void HandleTilt()
    {
        if (beeModel == null) return;

        // Get local velocity relative to Player
        Vector3 localVel = transform.InverseTransformDirection(currentVelocity);

        // ---- FIXED: Forward tilt sign corrected ----
        // Use localVel.z directly (no extra negative) so forward/back tilt matches movement
        float tiltX = Mathf.Clamp(localVel.z * forwardTilt / Mathf.Max(0.0001f, moveSpeed), -forwardTilt, forwardTilt);

        // Bank tilt from turning (based on A/D input, not strafing)
        float turnInput = 0f;
        if (Input.GetKey(KeyCode.A)) turnInput = -1f;
        if (Input.GetKey(KeyCode.D)) turnInput = 1f;
        float tiltZ = turnInput * turnTilt;

        Quaternion targetRot = Quaternion.Euler(tiltX, 0f, tiltZ);
        beeModel.localRotation = Quaternion.Slerp(beeModel.localRotation, targetRot, Time.deltaTime * tiltSmooth);
    }
}
