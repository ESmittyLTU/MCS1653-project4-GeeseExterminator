using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 1;
    public float maxTimeAlive = 2f;
    public AudioClip hit;

    private bool hitSomething = false;
    private float timeAlive = 0;

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        timeAlive += Time.deltaTime;
        if (timeAlive > maxTimeAlive)
        {
            hitSomething = true;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!hitSomething && other.gameObject.CompareTag("Enemy"))
        {
            AudioSource.PlayClipAtPoint(hit, other.GetContact(0).point);
            other.gameObject.GetComponent<Goose>().health -= damage;
        }
        hitSomething = true;
        Destroy(gameObject);
    }
}
