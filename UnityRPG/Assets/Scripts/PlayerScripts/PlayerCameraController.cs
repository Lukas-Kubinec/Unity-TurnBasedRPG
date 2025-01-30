using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/*
 * !!! NOT IN USE !!!
 */

public class PlayerCameraController : MonoBehaviour
{

    // Camera options
    public Camera MainCamera;
    public Camera CombatCamera;

    // UI Elements
    public TextMeshProUGUI CombatUI;


    // Function that checks if the Player entered a trigger
    private void OnTriggerEnter(Collider other)
    {
        // Compares the Trigger Tag
        if (other.CompareTag("TriggerCombat"))
        {
            // Changes the UI text
            CombatUI.text = "You entered the Trigger!";

            // Enables/Disables cameras
            CombatCamera.enabled = true;
            MainCamera.enabled = false;
        }
    }

    // Function that checks if the Player left a trigger
    private void OnTriggerExit(Collider other)
    {
        // Compares the Trigger Tag
        if (other.CompareTag("TriggerCombat"))
        {
            // Changes the UI text
            CombatUI.text = "You left the Trigger!";

            // Enables/Disables cameras
            CombatCamera.enabled = false;
            MainCamera.enabled = true;
        }
    }

}
