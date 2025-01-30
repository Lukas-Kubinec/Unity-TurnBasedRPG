using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // audio source types
    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    // music for each scene
    [Header("Background Music")]
    public AudioClip mainMenuMusic;
    public AudioClip forestOfEchoesBG;
    public AudioClip caveOfIllusionsBG;
    public AudioClip mountainOfDespairBG;
    public AudioClip castleFinaleBG;
    public AudioClip battleMusic;

    // non-diegetic sfx 
    [Header("SFX")]
    public AudioClip death;
    public AudioClip victory;

    // diegetic sfx
    [Header("Nature SFX")]
    public AudioClip rocksTumbling;
    public AudioClip campfire;
    public AudioClip fireball;
    public AudioClip birdsChirping;
    public AudioClip lightning1;
    public AudioClip lightning2;
    public AudioClip lightning3;
    public AudioClip lightning4;
    public AudioClip lightning5;
    public AudioClip waterDripping;
    public AudioClip volcanicMovement;
    public AudioClip lightWaterStream;
    public AudioClip echoingWaterStream;
    public AudioClip harshWind;

    public void PlayMusic(AudioClip music) // plays the given audio parameter
    {
        musicSource.PlayOneShot(music);
    }

    public void PlaySFX(AudioClip sfx) // plays the given audio parameter
    {
        sfxSource.PlayOneShot(sfx);
    }
}


/*
 * References:
 * Unity AUDIO MANAGER Tutorials - https://www.youtube.com/playlist?list=PLf6aEENFZ4FuL4XSo0rEUgecY7FED8p-I
 */