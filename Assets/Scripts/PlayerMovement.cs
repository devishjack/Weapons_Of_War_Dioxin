using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.SceneManagement;

public class PlayerMovement : NetworkBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        PlayerModel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "TestMap")
        {
            if(PlayerModel.activeSelf == false)
            {
                SetPosition();
                Cursor.lockState = CursorLockMode.Locked;
                PlayerModel.SetActive(true);
            }
            if (hasAuthority)
            {
                Movement();
            }
        }
    }

    public void SetPosition()
    {
        transform.position = new Vector3(0f, 5f, 0f);
    }

    private void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDis, groundMask);

        if (isGrounded && Velocity.y < 0)
        {
            Velocity.y = -10f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
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
