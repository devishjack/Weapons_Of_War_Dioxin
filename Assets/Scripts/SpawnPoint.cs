using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject graphics;

    private void Awake()
    {
        graphics.GetComponent<MeshRenderer>().enabled = false;
        graphics.GetComponent<CapsuleCollider>().enabled = false;
    }
}
