using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private void Start()
    {
        // checks for a saved volume
        if (PlayerPrefs.HasKey("saveMasterVolume"))
        {
            LoadMasterVolume();
        }
        else
        {
            SetMasterVolume();
        }

        // checks for a saved volume
        if (PlayerPrefs.HasKey("saveMusicVolume"))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolume();
        }

        // checks for a saved volume
        if (PlayerPrefs.HasKey("saveSFXVolume"))
        {
            LoadSFXVolume();
        }
        else
        {
            SetSFXVolume();
        }
    }

    public void SetMasterVolume()
    {
        float masterVolume = masterSlider.value; // sets the value of the master volume slider to a float
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20); // sets the logrithmic of the given float to a name (logrithmic is used to make the value match the audio mixers value)
        PlayerPrefs.SetFloat("saveMasterVolume", masterVolume); // stores float into a save
    }

    public void SetMusicVolume()
    {
        float musicVolume = musicSlider.value; // sets the value of the music volume slider to a float
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20); // sets the logrithmic of the given float to a name (logrithmic is used to make the value match the audio mixers value)
        PlayerPrefs.SetFloat("saveMusicVolume", musicVolume); // stores float into a save
    }

    public void SetSFXVolume()
    {
        float sfxVolume = sfxSlider.value; // sets the value of the sfx volume slider to a float
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20); // sets the logrithmic of the given float to a name (logrithmic is used to make the value match the audio mixers value)
        PlayerPrefs.SetFloat("saveSFXVolume", sfxVolume); // stores float into a save
    }

    private void LoadMasterVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("saveMasterVolume"); // loads the saved volume float

        SetMasterVolume(); // sets that save as the new volume
    }

    private void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("saveMusicVolume"); // loads the saved volume float

        SetMusicVolume(); // sets that save as the new volume
    }

    private void LoadSFXVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("saveSFXVolume"); // loads the saved volume float

        SetSFXVolume(); // sets that save as the new volume
    }
}


/*
 * References:
 * Unity AUDIO MANAGER Tutorials - https://www.youtube.com/playlist?list=PLf6aEENFZ4FuL4XSo0rEUgecY7FED8p-I
 */