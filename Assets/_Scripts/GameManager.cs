using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float minRangeFromPlayer = 10f;
    public float maxRangeFromPlayer = 20f;
    public GameObject goosePrefab;
    public float gooseSpawnCooldown = 5f;
    public int geeseWinRequirement = 25;
    
    public static int geeseKilledToWin = 25;
    public static int geeseKilled = 0;

    private float gooseSpawnTimer = 0f;

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
        gooseSpawnTimer += Time.deltaTime;
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

        Vector3 spawnLocation = FirstPersonController.playerPos += direction;
        Instantiate(goosePrefab, spawnLocation, Quaternion.identity);
    }

    public static void winCheck()
    {
        if (geeseKilled >= geeseKilledToWin)
        {
            Debug.Log("YOU WON!");
        }
    }
}
