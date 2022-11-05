using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookSingle : MonoBehaviour
{
    public float mouseSense = 800f;
    public Transform playerBody;
    public Transform playerHead;
    private float xRotation = 0f;
    private SurvivalController sc;
    private FPSAI enemy;
    public GameObject wc;
    public SceneChanger sceneChange;

    private void Awake()
    {
        sc = wc.GetComponent<SurvivalController>();
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerHead.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerHead.transform.position, playerHead.transform.forward, out hit))
        {
            if (hit.transform.name == "Enemy(Clone)")
            {
                enemy = hit.transform.GetComponent<FPSAI>();
                enemy.Ded();
                sc.enemiesLeft--;
            }
        }
    }

    public void PlayerDed()
    {
        Cursor.lockState = CursorLockMode.Confined;
        sceneChange.ChangeScene("YouDed");
    }
}
