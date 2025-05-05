using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject self;
    public static bool paused;

    void Start()
    {
        self.SetActive(false);
        paused = false;
    }

    void Update()
    {
        if (!paused && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESCAPED");
            self.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            paused = true;
        }
        else if (paused && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            self.SetActive(false);
            paused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
