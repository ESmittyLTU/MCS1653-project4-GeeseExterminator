using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeLight : MonoBehaviour
{
    public Light nukeLight;
    private float timeAlive = 0;

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
        nukeLight.intensity -= 20000;
        if (nukeLight.intensity < 0 ) Destroy(nukeLight);
    }
}
