using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerSceneLoader : MonoBehaviour
{
    // Allows to choose the scene in the editor
    public SceneAsset SceneToLoad;

    // Function that checks if the Player entered the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Compares the Trigger Tag
        if (other.CompareTag("Player"))
        {
            if (SceneToLoad != null)
            {
                // Loads the scene selected in editor
                SceneManager.LoadScene(SceneToLoad.name);            
            }
        }
    }
}
