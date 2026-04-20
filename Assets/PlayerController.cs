using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float jumpForce = 5f;
    public bool isGrounded;

    public Transform playerCamera;
    public float mouseSensitivity = 2f;
    public float lookUpLimit = -60f;
    public float lookDownLimit = 60f;

    private Rigidbody _rb;
    private float _xRotation = 0f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        RotatePlayer();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }
    void FixedUpdate()
    {
        MovePlayer();
    }
    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move.y = 0;
        move = move.normalized;

        Vector3 targetVelocity = move * walkSpeed;

        targetVelocity.y = _rb.linearVelocity.y;

        _rb.linearVelocity = targetVelocity;
    }

    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, lookUpLimit, lookDownLimit);

        playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; 
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
}