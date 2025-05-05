using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class Goose : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float damageCooldownSeconds = 2f;
    public Transform groundReference;
    public float heightDiffReqForJump = 5f;
    public float jumpSpeed = 10f;
    public float gooseStandardGravity = .9f;
    public float gooseFallGravity = .7f;
    public GameObject landminePrefab;
    public float landmineCooldownSeconds = 10f;
    public int landmineChance = 5; // Chance out of 10 to spawn landmine
    public int health = 3;
    public Animator anim;
    public AudioClip hurtSound;
    public AudioClip gooseHonk;
    public AudioClip gooseDies;

    private float damageTimer = 0;
    private float landmineTimer = 0;
    private Rigidbody rb;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            deathActions();
        }

        float distFromPlayer = Vector3.Distance(transform.position, FirstPersonController.playerPos);
        //Move to player within range
        if (distFromPlayer > .85f)
        {
            transform.position = Vector3.MoveTowards(transform.position, FirstPersonController.playerPos, moveSpeed * Time.deltaTime);
        }

        landmineTimer += Time.deltaTime;
        damageTimer += Time.deltaTime;

        if (landmineTimer >= landmineCooldownSeconds)
        {
            if (Random.Range(1, 11) <= landmineChance)
            {
                Ray ray = new Ray(groundReference.position, Vector3.down);
                bool didHit = Physics.Raycast(ray, out RaycastHit hit);
                if (didHit && hit.transform.CompareTag("Ground"))
                {
                    Vector3 landmineSpawn = hit.point;
                    Instantiate(landminePrefab, landmineSpawn, Quaternion.identity);
                }
            }
            landmineTimer = 0;
        }

        //If goose is on the ground and the player is substantially higher, allow goose to jump
        if (IsGrounded() && FirstPersonController.playerPos.y > transform.position.y + heightDiffReqForJump)
        {
            rb.velocity = Vector3.up * jumpSpeed;
        }

        if (Random.Range(1, 500) == 1)
        {
            AudioSource.PlayClipAtPoint(gooseHonk, transform.position);
        }
    }

    public void deathActions()
    {
        GameManager.geeseKilled++;
        AudioSource.PlayClipAtPoint(gooseDies, transform.position);
        Debug.Log($"{GameManager.geeseKilled} geese killed!");
        gameManager.winCheck();
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (!IsGrounded() && rb.velocity.y < -1f)
        {
            rb.AddForce(Physics.gravity * gooseFallGravity, ForceMode.Acceleration);
            anim.Play("geese-floating");
        }
        else
        {
            anim.Play("running");
            rb.AddForce(Physics.gravity * gooseStandardGravity, ForceMode.Acceleration);
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (!FirstPersonController.invincible && damageTimer > damageCooldownSeconds && other.gameObject.CompareTag("Player"))
        {
            FirstPersonController.health--;
            GameManager.playerHurting = true;
            AudioSource.PlayClipAtPoint(hurtSound, FirstPersonController.playerPos);
            damageTimer = 0;
            gameManager.loseCheck();
            FirstPersonController.invincible = true;
            Debug.Log($"Player took damage. Health: {FirstPersonController.health}");

        }
    }

    
    private bool IsGrounded()
    {
        return Physics.Raycast(groundReference.position, Vector3.down, .2f);
    }
}
