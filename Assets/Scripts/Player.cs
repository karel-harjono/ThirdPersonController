using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed = 2;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private Transform forwardIndicator;
    [SerializeField] float maxSpeed = 10f; // maximum speed
    
    // dashing params
    private bool isDashing = false;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;

    private Rigidbody rb;
    private Camera mainCamera;

    private void Start()
    {
        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnSpacePressed.AddListener(Jump);
        inputManager.OnDashPressed.AddListener(() => StartCoroutine(Dash()));
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    private void Update() {
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        transform.forward = cameraForward;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    private void MovePlayer(Vector2 direction)
    {
        Transform camTransform = mainCamera.transform;

        Vector3 forward = camTransform.forward;
        forward.y = 0; 
        forward.Normalize();

        Vector3 right = camTransform.right;
        right.y = 0; 
        right.Normalize();

        Vector3 moveDirection = (forward * direction.y + right * direction.x).normalized;

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (horizontalVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(speed * moveDirection, ForceMode.Acceleration);
        } else {
            Vector3 velocityDir = horizontalVelocity.normalized;
            float alignment = Vector3.Dot(velocityDir, moveDirection);

            if (alignment < 0.99f)
            {
                Vector3 steeringForce = (moveDirection - velocityDir) * speed;
                rb.AddForce(steeringForce, ForceMode.Acceleration);
            }
        }
    }

    private int jumpCount = 0;
    private const int maxJumpCount = 2;

    private bool isJumping = false;

    private void Jump()
    {
        if (rb != null && jumpCount < maxJumpCount)
        {
            isJumping = true;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }
    }

    private IEnumerator Dash() {
        if (isDashing) yield break;
        isDashing = true;
        bool originalGravity = rb.useGravity;
        rb.useGravity = false;
        Vector3 dashDirection = mainCamera.transform.forward.normalized;
        rb.AddForce(dashDirection * dashingPower, ForceMode.Impulse);
        yield return new WaitForSeconds(dashingTime);
        rb.useGravity = originalGravity;
        isDashing = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            isJumping = false;
        }
    }
}
