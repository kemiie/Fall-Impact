using UnityEngine;
using System.Collections;

public class MOVING : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraPivot;

    [Header("Jump Settings")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Flying Settings")]
    public float flyingSpeed = 8f;
    public float flyingUpDownSpeed = 6f;

    private CharacterController controller;
    private float yVelocity;
    private float xRotation = 0f;

    private float normalSpeed;
    private Coroutine speedBoostCoroutine;
    private Coroutine flightCoroutine;

    private bool isFlying = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        normalSpeed = speed;

        if (cameraPivot == null && Camera.main != null)
        {
            cameraPivot = Camera.main.transform.parent;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();

        if (isFlying)
        {
            HandleFlying();
        }
        else
        {
            HandleMovement();
        }
    }

    void HandleMouseLook()
    {
        float mouseX =
            Input.GetAxis("Mouse X") *
            mouseSensitivity *
            100f *
            Time.deltaTime;

        float mouseY =
            Input.GetAxis("Mouse Y") *
            mouseSensitivity *
            100f *
            Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraPivot.localRotation =
            Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move =
            transform.right * x +
            transform.forward * z;

        if (controller.isGrounded)
        {
            if (yVelocity < 0)
            {
                yVelocity = -2f;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                yVelocity =
                    Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * speed;
        velocity.y = yVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    // =========================
    // FLYING
    // =========================

    public void ActivateFlight(float duration)
    {
        if (flightCoroutine != null)
        {
            StopCoroutine(flightCoroutine);
        }

        flightCoroutine = StartCoroutine(Flight(duration));
    }

    private IEnumerator Flight(float duration)
    {
        isFlying = true;

        // Stop falling when flight starts
        yVelocity = 0f;

        Debug.Log("Balloon flight activated!");

        yield return new WaitForSeconds(duration);

        isFlying = false;

        // Give gravity control back to the player
        yVelocity = 0f;

        Debug.Log("Balloon flight ended!");

        flightCoroutine = null;
    }

    void HandleFlying()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // WASD movement
        Vector3 move =
            transform.right * x +
            transform.forward * z;

        move *= flyingSpeed;

        // Space = up
        if (Input.GetKey(KeyCode.Space))
        {
            move.y = flyingUpDownSpeed;
        }
        // Left Ctrl = down
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            move.y = -flyingUpDownSpeed;
        }
        else
        {
            move.y = 0f;
        }

        controller.Move(move * Time.deltaTime);
    }

    // =========================
    // SPEED BOOST
    // =========================

    public void ActivateSpeedBoost(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }

        speedBoostCoroutine =
            StartCoroutine(SpeedBoost(multiplier, duration));
    }

    private IEnumerator SpeedBoost(float multiplier, float duration)
    {
        speed = normalSpeed * multiplier;

        Debug.Log("Speed boost activated! Speed: " + speed);

        yield return new WaitForSeconds(duration);

        speed = normalSpeed;

        Debug.Log("Speed boost ended! Speed: " + speed);

        speedBoostCoroutine = null;
    }
}