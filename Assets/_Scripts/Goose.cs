using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goose : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float damageCooldownSeconds = 2f;
    public Transform groundReference;
    public float heightDiffReqForJump = 5f;
    public float jumpSpeed = 10f;
    public float gooseStandardGravity = .9f;
    public float gooseFallGravity = .7f;

    private float damageTimer = 0;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float distFromPlayer = Vector3.Distance(transform.position, FirstPersonController.playerPos);
        //Move to player within range
        if (distFromPlayer > .85f)
        {
            transform.position = Vector3.MoveTowards(transform.position, FirstPersonController.playerPos, moveSpeed * Time.deltaTime);
        }

        damageTimer += Time.deltaTime;

        //If goose is on the ground and the player is substantially higher, allow goose to jump
        if (IsGrounded() && FirstPersonController.playerPos.y > transform.position.y + heightDiffReqForJump)
        {
            rb.velocity = Vector3.up * jumpSpeed;
        }
    }

    void FixedUpdate()
    {
        if (!IsGrounded() && rb.velocity.y < -1f)
        {
            rb.AddForce(Physics.gravity * gooseFallGravity, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(Physics.gravity * gooseStandardGravity, ForceMode.Acceleration);
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (!FirstPersonController.invincible && damageTimer > damageCooldownSeconds && other.gameObject.CompareTag("Player"))
        {
            FirstPersonController.health--;
            damageTimer = 0;
            Debug.Log($"Player took damage. Health: {FirstPersonController.health}");

        }
    }

    
    private bool IsGrounded()
    {
        return Physics.Raycast(groundReference.position, Vector3.down, .2f);
    }
}
