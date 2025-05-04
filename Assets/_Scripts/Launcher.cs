using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Launcher : MonoBehaviour
{
    public GameObject laserPointer, laser;
    public float laserLength = 200f;
    public GameObject crosshairProjectorHolder;
    public DecalProjector crosshairProjector;
    public LayerMask groundLayerMask;
    public float launcherCooldown = 15f;
    public GameObject nukePrefab;

    private float launcherTimer = 10;
    private Ray laserRay;

    private void Start()
    {
        crosshairProjector.enabled = false;
        laser.SetActive(false);
    }

    private void Update()
    {
        //Update the timer, and if 
        launcherTimer += Time.deltaTime;
        if (!laser.activeSelf && launcherTimer >= launcherCooldown)
        {
            laser.SetActive(true);
            crosshairProjector.enabled = true;
        }

        
        //If laser is active, draw a ray from it and put the crosshair on the ground
        if (laser.activeSelf)
        {
            //Debug.DrawRay(laserPointer.transform.position, laserPointer.transform.up * laserLength, Color.green);
            laserRay = new Ray(laserPointer.transform.position, laserPointer.transform.up);
            bool didHit = Physics.Raycast(laserRay, out RaycastHit hit, laserLength, groundLayerMask);
            if (didHit && hit.transform.CompareTag("Ground"))
            {
                crosshairProjector.enabled = true;
                Vector3 crosshairSpawn = hit.point;
                crosshairProjectorHolder.transform.position = crosshairSpawn;
            }
            else
            {
                crosshairProjector.enabled = false;
            }

            if (Input.GetMouseButtonDown(0) && didHit)
            {
                launcherFire();
            }
        }
    }

    //When player fires
    public void launcherFire()
    {
        //Reset timer and disable laser
        launcherTimer = 0;
        laser.SetActive(false);

        Instantiate(nukePrefab, crosshairProjectorHolder.transform.position + Vector3.up * 300, Quaternion.Euler(10, 0, 10));
        



    }
}
