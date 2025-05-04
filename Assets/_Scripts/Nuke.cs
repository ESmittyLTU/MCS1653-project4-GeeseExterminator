using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Nuke : MonoBehaviour
{
    public Rigidbody rb;
    public float gravMultiplier = 5f;
    public GameObject explosionPrefab;

    private DecalProjector crosshair;

    private void Start()
    {
        crosshair = FindObjectOfType<DecalProjector>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravMultiplier, ForceMode.Acceleration);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Instantiate(explosionPrefab, other.contacts[0].point + Vector3.up * .5f, Quaternion.identity);
            crosshair.enabled = false;
            Destroy(gameObject);
        }
    }
}
