using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeLight : MonoBehaviour
{
    public Light nukeLight;
    public float blastRadius = 10f;
    private float timeAlive = 0;

    private Goose[] geese;
    private GameManager GameManager;

    private void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        GameManager.blastTheLights();

        geese = FindObjectsOfType<Goose>();
        if ( geese.Length > 0 )
        {
            foreach (Goose goose in geese)
            {
                if (Vector3.Distance(goose.transform.position, transform.position) <= blastRadius)
                {
                    goose.deathActions();
                }
            }
        }
    }


    // Update is called once per frame
    private void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= 5f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        nukeLight.intensity -= 7500;
        if (nukeLight.intensity < 0 ) Destroy(nukeLight);

        GameManager.hitTheLights();
    }
}
