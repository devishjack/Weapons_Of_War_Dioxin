using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSingle : MonoBehaviour
{
    public GameObject PlayerModel;
    public CharacterController controller;

    private Vector3 Velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 5f;

    public Transform groundCheck;
    public float groundDis = 1f;
    public LayerMask groundMask;

    public bool isGrounded;

    public float speed = 4f;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDis, groundMask);

        if(isGrounded && Velocity.y < 0)
        {
            Velocity.y = -10f;
        }

        Debug.Log(isGrounded);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("working");
            Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 8f;
        }
        else
        {
            speed = 4f;
        }

        Velocity.y += gravity * 3f * Time.deltaTime;

        controller.Move(Velocity * Time.deltaTime);
    }
}
