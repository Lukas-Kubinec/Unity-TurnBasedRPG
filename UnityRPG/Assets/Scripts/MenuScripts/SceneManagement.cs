using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    private AudioManager audioManager;
    private Scene currentScene;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); // finds the audio manager
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene(); // gets current scene

        // the if statements play different music depending on the currently active scene
        if (currentScene == SceneManager.GetSceneByName("MainMenu"))
        {
            audioManager.PlayMusic(audioManager.mainMenuMusic);
        }
        else if (currentScene == SceneManager.GetSceneByName("ForestOfEchoes"))
        {
            audioManager.PlayMusic(audioManager.forestOfEchoesBG);
        }
        else if (currentScene == SceneManager.GetSceneByName("CaveOfIllusions"))
        {
            audioManager.PlayMusic(audioManager.caveOfIllusionsBG);
        }
        else if (currentScene == SceneManager.GetSceneByName("MountainOfDespair"))
        {
            audioManager.PlayMusic(audioManager.mountainOfDespairBG);
        }
        else if (currentScene == SceneManager.GetSceneByName("CastleOfTheFinalBattle"))
        {
            audioManager.PlayMusic(audioManager.castleFinaleBG);
        }
        else if (currentScene == SceneManager.GetSceneByName("CastleFight"))
        {
            audioManager.PlayMusic(audioManager.battleMusic);
        }
        else if (currentScene == SceneManager.GetSceneByName("ForestFight"))
        {
            audioManager.PlayMusic(audioManager.battleMusic);
        }
        else if (currentScene == SceneManager.GetSceneByName("CaveFight"))
        {
            audioManager.PlayMusic(audioManager.battleMusic);
        }
        else if (currentScene == SceneManager.GetSceneByName("MountainFight"))
        {
            audioManager.PlayMusic(audioManager.battleMusic);
        }
    }
}