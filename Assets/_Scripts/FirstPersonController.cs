using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed  =   2.0f;
    public float jumpSpeed  =   2.0f;
    public float yawSpeed   =   260.0f;
    public float pitchSpeed =   260.0f;
    public float minPitch   =   -45.0f;
    public float maxPitch   =   45.0f;
    public Transform groundReference;
    public int inspectorHealth;
    public float invincibilityLength = .5f;
    public GameObject bulletPrefab;
    public Transform barrelTip;
    public float shotDelay = 1.5f;
    public GameObject gun, laserPointer;
    public int selectedWeapon = 1;
    public GameObject projectorHolder;

    public static int health;
    public static Vector3 playerPos;
    public static bool invincible = false;


    private Rigidbody rb;
    private Transform cameraTransform;
    private float invincibilityTimer = 0f;
    private float firingTimer = 0;
    
   

    // Start is called before the first frame update
    void Start()
    {
        health = inspectorHealth;
        rb = GetComponent<Rigidbody>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        invincibilityTimer = invincibilityLength;
    }

    // Update is called once per frame
    void Update()
    {

        //Invincibility cooldown
        if (invincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                invincible = false;
                invincibilityTimer = invincibilityLength;
            }
        }

        //put all input axis info into variable
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float yaw = Input.GetAxis("Mouse X");
        float pitch = Input.GetAxis("Mouse Y");
        

        Vector3 velo = Vector3.zero;
        velo += transform.right * h;
        velo += transform.forward * v;
        velo *= moveSpeed;

        //set the jump
        velo.y = rb.velocity.y;

        //if (Input.GetKeyDown(KeyCode.Space) &&
        //    //Physics.CheckSphere(groundReference.position, .04f))
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() )
        {
            velo.y = jumpSpeed;
        }

        //apply movement
        rb.velocity = velo;

        //apply rotation
        transform.localEulerAngles += new Vector3(
            0,
            yaw * yawSpeed * Time.deltaTime,
            0);

        float pitchDelta = -1 * pitch * pitchSpeed * Time.deltaTime;
        float newPitch = cameraTransform.localEulerAngles.x + pitchDelta;
        newPitch = angleWithin180(newPitch);

        //Debug.Log($"Pitch before: {newPitch}");
        newPitch = Mathf.Clamp(newPitch, minPitch, maxPitch);
        //Debug.Log($"Pitch after: {newPitch}");

        cameraTransform.localEulerAngles = new Vector3(newPitch,
            cameraTransform.localEulerAngles.y,
            cameraTransform.localEulerAngles.z);

        //Updating static var playerPos with a vertical offset so that geese don't clip through floor tracking onto player (idk why player y pos isnt centered)
        playerPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        //Click to fire bullet
        firingTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedWeapon == 1) 
            {
                if (firingTimer >= shotDelay)
                {
                    firingTimer = 0;
                    Instantiate(bulletPrefab, barrelTip.position, barrelTip.rotation);
                }
            }
        }

        //Weapon switching
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gun.SetActive(true);
            laserPointer.SetActive(false);
            selectedWeapon = 1;
            projectorHolder.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gun.SetActive(false);
            laserPointer.SetActive(true);
            selectedWeapon = 2;
            projectorHolder.SetActive(true);
        }


}

    private bool IsGrounded()
    {
        return Physics.Raycast(groundReference.position, Vector3.down, .25f);
    }

    private float angleWithin180(float angle)
    {
        return angle > 180 ? angle - 360 : angle;
        //this does the same thing as the ternary operator above
        //if (angle > 180) 
        //    return angle - 360;
        //else 
        //    return angle;
    }
}
