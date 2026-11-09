using UnityEngine;

public class Mover : MonoBehaviour
{
    public float jumpForce = 5f;
    public float speed = 5f;

    public float rayDistance = 1.0f;
    public LayerMask collisionMask;

    public Server.TickRate serverT;

    public float mouseSensitivity = 100f;

    public Transform playerBody;
    public Rigidbody rb;
    public Transform camera;

    private float xRotation = 0f;

    private bool jumpRequested;

    public Transform groundCheck;
    public float groundRadius = 0.2f;

    private void Start()
    {
        serverT.OnTick += Move;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            jumpRequested = true;
    }

    private void OnDestroy()
    {
        serverT.OnTick -= Move;
    }

    public void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);

        Vector3 movement =
            playerBody.right * moveHorizontal +
            playerBody.forward * moveVertical;

        movement = Vector3.ClampMagnitude(movement, 1f);

        Vector3 direction = movement.normalized;

        if (movement.magnitude > 0.01f)
        {
            if (!Physics.Raycast(
                transform.position,
                direction,
                out RaycastHit hit,
                rayDistance,
                collisionMask))
            {
                transform.position += movement * speed * Time.deltaTime;
            }
        }

        // Прыжок
        if (!jumpRequested)
            return;

        jumpRequested = false;

        if (Physics.CheckSphere(
            groundCheck.position,
            groundRadius,
            collisionMask))
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(
            transform.position,
            Vector3.down,
            rayDistance,
            collisionMask
        );
    }
}