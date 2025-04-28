using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 1;

    private bool hitSomething = false;

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!hitSomething && other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Goose>().health -= damage;
        }
        hitSomething = true;
        Destroy(gameObject);
    }
}
