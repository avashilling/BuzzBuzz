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
    private Quaternion initialModelRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;

        // Remember the model’s starting rotation so we can tilt relative to it
        if (beeModel != null)
            initialModelRotation = beeModel.localRotation;
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
        // --- Rotation (Yaw) with A/D --- (rotating around world up)
        float turnInput = 0f;
        if (Input.GetKey(KeyCode.A)) turnInput = -1f;
        if (Input.GetKey(KeyCode.D)) turnInput = 1f;
        transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.deltaTime);

        // --- Forward/Backward with W/S ---
        float forwardInput = 0f;
        if (Input.GetKey(KeyCode.W)) forwardInput = 1f;
        if (Input.GetKey(KeyCode.S)) forwardInput = -1f;

        // --- Vertical with Space / Shift ---
        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.Space)) verticalInput = 1f;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) verticalInput = -1f;

        // Your local forward is +X, up is +Y, and left is +Z.
        // Flipped the sign on X so W moves forward instead of sideways.
        Vector3 inputDir = new Vector3(0, verticalInput, -forwardInput).normalized;

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

        rb.linearVelocity = currentVelocity;
    }

    void HandleTilt()
    {
        if (beeModel == null) return;

        // Get local velocity relative to the bee
        Vector3 localVel = transform.InverseTransformDirection(currentVelocity);

        // --- Forward/back tilt around Z axis ---
        // (Positive X = forward movement in your setup)
        float tiltZ = Mathf.Clamp(localVel.x * forwardTilt / Mathf.Max(0.0001f, moveSpeed), -forwardTilt, forwardTilt);

        // --- Left/right tilt around X axis ---
        float turnInput = 0f;
        if (Input.GetKey(KeyCode.A)) turnInput = -1f;
        if (Input.GetKey(KeyCode.D)) turnInput = 1f;
        float tiltX = turnInput * turnTilt;

        Quaternion targetRot = initialModelRotation * Quaternion.Euler(tiltX, 0f, -tiltZ);
        beeModel.localRotation = Quaternion.Slerp(beeModel.localRotation, targetRot, Time.deltaTime * tiltSmooth);
    }
}
