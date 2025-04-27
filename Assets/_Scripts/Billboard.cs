using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private GameObject PlayerCamera;

    void Start()
    {
        PlayerCamera = GameObject.FindGameObjectWithTag("PlayerCamera");
    }

    void Update()
    {
        //Makes the sprite always face towards the camera
        transform.rotation = Quaternion.LookRotation(PlayerCamera.transform.position - transform.position);
    }
}
