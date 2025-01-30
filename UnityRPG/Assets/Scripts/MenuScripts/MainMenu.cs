using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("ForestOfEchoes"); // loads forest of echoes scene when new game is pressed
    }

    public void QuitGame()
    {
        Application.Quit(); // quits the game - LK
    }
}
