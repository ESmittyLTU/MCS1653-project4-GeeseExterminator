using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float minRangeFromPlayer = 10f;
    public float maxRangeFromPlayer = 20f;
    public GameObject goosePrefab;
    public float gooseSpawnCooldown = 5f;

    private float gooseSpawnTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /* CHECKLIST
     * 
     * [X] Player movement
     * [X] Geese Spawning (Crashland within range of player)
     * [X] Geese movement (towards player, navmesh video)
     * [ ] Pistol (held near player view, fires bullets (capsule, trail renderer))
     * [ ] Geese take damage when bullet hits them (use bool damageDealt to prevent pierce)
     * [X] Geese deal damage to player (onCollisionStay with a cooldown)
     * [ ] Geese have a chance to lay landmines every 10/15 seconds
     * [X] Landmines explode and deal damage to player on contact
     * [ ] Player wins after killing X geese (static var)
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
}
