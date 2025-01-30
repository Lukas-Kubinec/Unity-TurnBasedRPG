/*
 * REFERENCES
 * Cox, D. (2021). Using JsonUtility in Unity to Save and Load Game Data. [online] 
 * Digital Ephemera. Available at: https://videlais.com/2021/02/25/using-jsonutility-in-unity-to-save-and-load-game-data/
 */

using UnityEngine;
using System.IO;
using UnityEditor.Overlays;

public class SaveLoadSystem : MonoBehaviour
{
    // Holds the path to the save file
    string saveFile;

    // Used to access the game data 
    private GameData saveData;

    // Used to load the level
    private SceneLoader sceneLoader;

    void Awake()
    {
        // Checks if the GameData component was already made
        if (GetComponent<SceneLoader>() == null)
        {
            // Adds the GameData component to game object
            sceneLoader = gameObject.AddComponent<SceneLoader>();
        }
        else
        {
            // Gets the GameData component from game object
            sceneLoader = gameObject.GetComponent<SceneLoader>();
        }

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

        // Saves the game - current level
        WriteSaveFile();
    }

    // Used to save the game once the player reaches next level
    public void WriteSaveFile()
    {
        // Generates a JSON out of the public fields from Save Data and saves it to JSON string
        string jsonString = JsonUtility.ToJson(saveData);

        // Writes JSON data to the Save File
        File.WriteAllText(saveFile, jsonString);
    }

    // Used to load the save file when the player loads the save / reaches next level
    public void ReadSaveFile()
    {
        // Checks if the Save File exists
        if (File.Exists(saveFile))
        {
            // Read the Save File and saves the data to the File Contents
            string fileContents = File.ReadAllText(saveFile);

            // Updates the Game Data with the data loaded from Save File
            JsonUtility.FromJsonOverwrite(fileContents, saveData);

            // Loads the desired level
            sceneLoader.LoadGame();
        }
    }
}
