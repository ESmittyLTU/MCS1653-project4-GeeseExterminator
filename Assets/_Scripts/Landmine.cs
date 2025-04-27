using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    public int damage = 3;
    private bool detonated = false;

    void OnTriggerEnter(Collider other)
    {
        if (!detonated && other.gameObject.CompareTag("Player")) 
        {
            detonated = true;
            FirstPersonController.health -= damage;
            Debug.Log($"Stepped on landmine. Health: {FirstPersonController.health}");
            Destroy(gameObject);
        }
    }

}
