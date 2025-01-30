using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLooseScript : MonoBehaviour
{

    // Allows to choose the scenes in the editor
    public SceneAsset NextScene;
    public SceneAsset CurrentScene;

    public void LoadNextScene()
    {
        // Loads the next scene
        SceneManager.LoadScene(NextScene.name);
    }

    public void RestartCurrentScene()
    {
        // Restarts current scene
        SceneManager.LoadScene(CurrentScene.name);
    }
}
