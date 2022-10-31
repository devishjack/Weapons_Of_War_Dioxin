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

    public float speed = 12f;
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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }
}
