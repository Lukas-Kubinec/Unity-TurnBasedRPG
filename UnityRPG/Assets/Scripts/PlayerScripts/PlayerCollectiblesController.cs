using TMPro;
using UnityEngine;

/*
 * !!! NOT IN USE !!!
 */

public class PlayerCollectiblesController : MonoBehaviour
{

    // Player's Collectible Counter
    private int score = 0;

    // UI Elements
    public TextMeshProUGUI CollectibleCounter;


    private void Start()
    {
        // Updates the UI Score at the beginning of the game
        CollectibleCounter.text = "Score: " + score.ToString();
    }


    // Function that checks if the Player entered a trigger
    private void OnTriggerEnter(Collider other)
    {
        // Compares the Trigger Tag
        if (other.CompareTag("Collectible"))
        {
            // Adds one to Player's score
            score++;

            // Updates the UI Score text
            CollectibleCounter.text = "Score: " + score.ToString();

            // Destroys the collectible object
            GameObject.Destroy(other.gameObject);
        }
    }
}
