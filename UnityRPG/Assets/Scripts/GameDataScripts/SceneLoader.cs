using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneLoader : MonoBehaviour
{

    // Holds the path to the save file
    string saveFile;

    // Used to access the game data 
    private GameData saveData;

    void Awake()
    {
        // Checks if the GameData component was already made
        if (GetComponent<GameData>() == null)
        {
            // Adds the GameData component to game object
            saveData = gameObject.AddComponent<GameData>();
        }
        else
        {
            // Gets the GameData component from game object
            saveData = gameObject.GetComponent<GameData>();
        }

        // Updates the file path to the save data
        saveFile = Application.persistentDataPath + "/saveData.json";
    }

    public void LoadGame()
    {
        // Checks if the Save File exists
        if (File.Exists(saveFile))
        {
            // Read the Save File and saves the data to the File Contents
            string fileContents = File.ReadAllText(saveFile);

            // Updates the Game Data with the data loaded from Save File
            JsonUtility.FromJsonOverwrite(fileContents, saveData);

            // Loads the desired level
            LoadScene(saveData.gameScene);
        }
    }

    private void LoadScene(int gameScene)
    {
        string LoadLevel;

        // Assigns scene name depending on the input
        switch (gameScene)
        {
            case 0:
                LoadLevel = "ForestOfEchoes";
                break;
            case 1:
                LoadLevel = "MountainOfDespair";
                break;
            case 2:
                LoadLevel = "CaveOfIllusions";
                break;
            case 3:
                LoadLevel = "CastleOfTheFinalBattle";
                break;
            default:
                LoadLevel = "MainMenu";
                break;

        }

        // Unfreezes time, in case game was loaded from the pause menu
        Time.timeScale = 1.0f;

        // Loads the desired level
        SceneManager.LoadScene(LoadLevel);
    }

}
