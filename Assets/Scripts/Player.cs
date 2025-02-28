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
    private bool canDash = true;
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
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0;
        forward.Normalize();
        right.y = 0;
        right.Normalize();

        Vector3 moveDirection = (right * direction.x + forward * direction.y).normalized;
        rb.AddForce(speed * moveDirection);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private int jumpCount = 0;
    private const int maxJumpCount = 2;

    private void Jump()
    {
        if (rb != null && jumpCount < maxJumpCount)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }
    }

    private IEnumerator Dash() {
        if (!canDash) yield break;
        canDash = false;
        bool originalGravity = rb.useGravity;
        rb.useGravity = false;
        Vector3 dashDirection = mainCamera.transform.forward.normalized;
        rb.AddForce(dashDirection * dashingPower, ForceMode.Impulse);
        yield return new WaitForSeconds(dashingTime);
        rb.useGravity = originalGravity;
        canDash = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}
