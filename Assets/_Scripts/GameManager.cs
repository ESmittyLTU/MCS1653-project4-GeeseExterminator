using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float minRangeFromPlayer = 10f;
    public float maxRangeFromPlayer = 20f;
    public GameObject goosePrefab;
    public float gooseSpawnCooldown = 5f;
    public int geeseWinRequirement = 25;
    public Light mainLight;
    public LayerMask groundLayerMask;
    public float initialDelayAmount = 10f;

    public static int geeseKilledToWin = 25;
    public static int geeseKilled = 0;

    private float gooseSpawnTimer = 0f;
    private float exposure = 1;
    private bool initialDelay = true;
    private float initialTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        geeseKilledToWin = geeseWinRequirement;
    }

    /* CHECKLIST
     * 
     * [X] Player movement
     * [X] Geese Spawning (Crashland within range of player)
     * [X] Geese movement (towards player, navmesh video)
     * [X] Pistol (held near player view, fires bullets (capsule, trail renderer))
     * [X] Geese take damage when bullet hits them (use bool damageDealt to prevent pierce)
     * [X] Geese deal damage to player (onCollisionStay with a cooldown)
     * [X] Geese have a chance to lay landmines every 10/15 seconds
     * [X] Landmines explode and deal damage to player on contact
     * [X] Player wins after killing X geese (static var)
     * 
     * FINAL BUILD STATUS
     * [ ] Terrain
     * [ ] Bullet dies after time
     * [ ] Pistol can't fire infinitely
     * [ ] Pressing one or two disables/enables rocket launcher/pistol
     * [ ] Geese animations
     * [ ] Landmine textures
     * 
     * STRETCH/UNSURE
     * [ ] Cranked (player dies within certain amount of time without killing a goose, just use static timer)
     * [ ] 
     * [ ]
     * [ ]
     * [ ]
     * 
     */

    // Update is called once per frame
    void Update()
    {
        if (!initialDelay) { 
            gooseSpawnTimer += Time.deltaTime; 
        } else
        {
            initialTimer += Time.deltaTime;
            if (initialTimer >= initialDelayAmount)
            {
                initialDelay = false;
            }
        }
        if (gooseSpawnTimer >= gooseSpawnCooldown)
        {
            spawnGoose();
            gooseSpawnTimer = 0f;
        }
    }

    public void spawnGoose()
    {
        Vector2 unitCirclePoint = Random.insideUnitCircle.normalized;
        Vector3 direction = new Vector3(unitCirclePoint.x, 0, unitCirclePoint.y);
        direction *= Random.Range(minRangeFromPlayer, maxRangeFromPlayer);
        //Particle effect + play a sound

        Vector3 spawnLocation = FirstPersonController.playerPos += direction + Vector3.up * 200;

        Ray ray = new Ray(spawnLocation, Vector3.down);
        bool didHit = Physics.Raycast(ray, out RaycastHit hit, 300f, groundLayerMask);
        if (didHit && hit.transform.CompareTag("Ground"))
        {
            Vector3 gooseSpawn = hit.point;
            Instantiate(goosePrefab, gooseSpawn, Quaternion.identity);
        }
    }

    public static void winCheck()
    {
        if (geeseKilled >= geeseKilledToWin)
        {
            Debug.Log("YOU WON!");
        }
    }

    public void dimTheLights()
    {
        exposure -= .0066666f;
        mainLight.intensity = exposure;
        if (exposure > .05f)
        {
            RenderSettings.skybox.SetFloat("_Exposure", exposure);
        }
    }

    public void blastTheLights()
    {
        exposure = 3;
        mainLight.intensity = exposure;
        RenderSettings.skybox.SetFloat("_Exposure", exposure);
    }

    public void hitTheLights()
    {
        if (exposure > 1)
        {
            exposure -= .0126666f;
        } else
        {
            exposure = 1;
        }
        mainLight.intensity = exposure;
        RenderSettings.skybox.SetFloat("_Exposure", exposure);
    }
}
