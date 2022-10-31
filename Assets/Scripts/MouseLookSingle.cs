using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookSingle : MonoBehaviour
{
    public float mouseSense = 800f;
    public Transform playerBody;
    public Transform playerHead;
    private float xRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerHead.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
