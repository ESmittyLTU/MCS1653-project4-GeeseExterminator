using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    public int damage = 3;
    private bool detonated = false;
    private GameManager gameManager;
    public AudioClip hurtSound;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!detonated && other.gameObject.CompareTag("Player")) 
        {
            if (!FirstPersonController.invincible)
            {
                detonated = true;
                GameManager.playerHurting = true;
                AudioSource.PlayClipAtPoint(hurtSound, FirstPersonController.playerPos);
                FirstPersonController.health -= damage;
                FirstPersonController.invincible = true;
                gameManager.loseCheck();
                Debug.Log($"Stepped on landmine. Health: {FirstPersonController.health}");
                Destroy(gameObject);
            }
        }
    }

}
