
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Running")]
    public bool canRun = true;
    public float runSpeed = 9f;
    public KeyCode runningKey = KeyCode.LeftShift;

    public bool IsRunning { get; private set; }

    private Rigidbody rb;
    private Vector2 movementInput;

    public List<Func<float>> speedOverrides =
        new List<Func<float>>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Don't move while a menu is open.
        if (UIManager.Instance != null &&
            UIManager.Instance.IsPanelOpen())
        {
            movementInput = Vector2.zero;
            IsRunning = false;
            return;
        }

        // Cursor lock does NOT disable movement.
        movementInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        movementInput = Vector2.ClampMagnitude(
            movementInput, 1f
        );

        IsRunning = canRun && Input.GetKey(runningKey);
    }

    private void FixedUpdate()
    {
        if (rb == null || rb.isKinematic)
            return;

        float targetSpeed = IsRunning ? runSpeed : speed;

        if (speedOverrides.Count > 0)
            targetSpeed = speedOverrides[speedOverrides.Count - 1]();

        Vector3 direction =
            transform.right * movementInput.x +
            transform.forward * movementInput.y;

        Vector3 velocity = direction * targetSpeed;

        rb.linearVelocity = new Vector3(
            velocity.x,
            rb.linearVelocity.y,
            velocity.z
        );
    }
}