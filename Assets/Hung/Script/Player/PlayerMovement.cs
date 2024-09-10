using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [Header("Setting")]
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Vector3 move;
    Vector3 velocity;
    bool isGrounded;

    [Header("Look")]
    public float mouseSensitivity = 100f;
    public Transform cam;
    float xRotation = 0f;
    float mouseX, mouseY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //gravity
        velocity.y += gravity * 2 *  Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        if (isGrounded)
        {
            controller.Move(move * speed * Time.fixedDeltaTime);
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void OnMove(InputValue input)
    {
        move.x = input.Get<Vector2>().x;
        move.z = input.Get<Vector2>().y;

        move = transform.right * move.x + transform.forward * move.z;

    }

    void OnJump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void OnLook(InputValue input)
    {
        Debug.Log(mouseX);
        mouseX = input.Get<Vector2>().x * mouseSensitivity * Time.deltaTime;
        mouseY = input.Get<Vector2>().y * mouseSensitivity * Time.deltaTime;       
    }

    private void OnEnable()
    {
        
    }
}
