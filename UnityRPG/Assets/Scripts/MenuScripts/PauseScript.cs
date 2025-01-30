using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScript : MonoBehaviour
{
    protected HandleCursorVisibility CursorSetVisibility; // Gets component to check if cursor is meant to be always shown - LK

    public GameObject pauseMenu;

    public bool isPaused;

    void Start()
    {
        pauseMenu.SetActive(false); // deactivates pause menu on start

        CursorSetVisibility = gameObject.GetComponent<HandleCursorVisibility>(); // Gets component to check if cursor is meant to be always shown - LK
    }

    void Update()
    {
        // checks if escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // checks if game is paused
            if (isPaused)
            {
                // continues game if paused
                ContinueGame();
            }
            else
            {
                // pauses game if game isn't paused
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        // Handles cursor visibility - LK
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        pauseMenu.SetActive(true); // activates pause menu
        Time.timeScale = 0.0f; // freezes time
        isPaused = true; // sets isPaused to true
        Cursor.lockState = CursorLockMode.None; // unlocks cursor
    }

    public void ContinueGame()
    {
        if (!CursorSetVisibility.AlwaysShowCursor)
        {
            // Handles cursor visibility if Cursor is not meant to be shown at all times - LK
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        pauseMenu.SetActive(false); // deactivates pause menu
        Time.timeScale = 1.0f; // unfreezes time
        isPaused = false; // sets isPaused to false
    }
}


/*
 * References:
 * 6 Minute PAUSE MENU Unity Tutorial - https://www.youtube.com/watch?v=9dYDBomQpBQ
*/