using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    public Volume ppVolume;
    public GameObject winScreen;
    public GameObject loseScreen;
    public AudioClip gooseHonk;
    public TextMeshProUGUI geeseCounter, livesCounter;

    public static int geeseKilledToWin = 25;
    public static int geeseKilled = 0;
    public static bool playerHurting = false;

    private float gooseSpawnTimer = 0f;
    private float exposure = 1;
    private bool initialDelay = true;
    private float initialTimer = 0f;
    private Vignette ppVignette;
    private Vignette tempVignette;
    private float intensity = .35f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        geeseKilledToWin = geeseWinRequirement;

        livesCounter.SetText("Health: 10");
        geeseCounter.SetText("Dead Geese: 0/30");

        mainLight.intensity = 1;
        RenderSettings.skybox.SetFloat("_Exposure", 1);

        if (ppVolume.profile.TryGet(out tempVignette))
        {
            ppVignette = tempVignette;
        }
    }

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

        float vignIntensity = ((float)ppVignette.intensity);
        if (playerHurting && vignIntensity <= .005)
        {
            intensity = .35f;
            ppVignette.intensity.Override(intensity);
            playerHurting = false;
        } 
        
        if (vignIntensity > 0)
        {
            intensity -= .0007f;
            ppVignette.intensity.Override(intensity);
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
            AudioSource.PlayClipAtPoint(gooseHonk, gooseSpawn);
        }
    }

    public void winCheck()
    {
        geeseCounter.SetText($"Dead Geese: {geeseKilled}/30");
        if (geeseKilled >= geeseKilledToWin)
        {
            
            winScreen.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("YOU WON!");
        }
    }

    public void loseCheck()
    {
        livesCounter.SetText($"Health: {FirstPersonController.health}");
        if (FirstPersonController.health <= 0)
        {
            
            loseScreen.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("YOU LOST!");
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
