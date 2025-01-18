using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConstVariable;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public AudioSource backgroundMusic;  
    public AudioSource soundEffects; 

    public AudioClip buttonClick;
    public AudioClip popup;

    private bool isMuted = false; 

    private void Start()
    {
        if(PlayerPrefs.HasKey(SoundSetting))
        {
            if(PlayerPrefs.GetString(SoundSetting) == "true")
            {
                MuteAudio();
            }
            else
            {
                UnmuteAudio();
            }
        }
        else
        {
            PlayerPrefs.SetString(SoundSetting, "false");
        }
        // Play background music if it's not muted
        if (!isMuted)
        {
            PlayBackgroundMusic();
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;  // Loop the background music
            backgroundMusic.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
        }
    }


    public void toggleSound()
    {
        if(isMuted) UnmuteAudio();
        else MuteAudio();
    }
    public void MuteAudio()
    {
        PlayerPrefs.SetString(SoundSetting, "true");
        isMuted = true;
        backgroundMusic.mute = true;
        soundEffects.mute = true;
    }

    // Function to unmute audio directly
    public void UnmuteAudio()
    {
        PlayerPrefs.SetString(SoundSetting, "false");
        isMuted = false;
        backgroundMusic.mute = false;
        soundEffects.mute = false;
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (soundEffects != null && clip != null)
        {
            soundEffects.PlayOneShot(clip);
        }
    }

    public bool getMuteState()
    {
        return isMuted;
    }
   
}
