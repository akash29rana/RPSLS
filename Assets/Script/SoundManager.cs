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

    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip timerStart;
    public AudioClip timerEnding;

    bool isMuted = false; 

    bool isSoundEffectPlaying = false;
    AudioClip currentSoundEffect;
    float soundEffectTime = 0f;

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
            if (isSoundEffectPlaying)
            {
                soundEffects.Stop();
                soundEffectTime = 0f; 
                isSoundEffectPlaying = false;
            }

            soundEffects.clip = clip;
            soundEffects.Play();
            currentSoundEffect = clip;
            isSoundEffectPlaying = true;
        }
    }

    public void PauseSoundEffect()
    {
        if (isSoundEffectPlaying)
        {
            soundEffectTime = soundEffects.time;  // Store the time of the current sound effect
            soundEffects.Stop();
            isSoundEffectPlaying = false;
        }
    }

    public void ResumeSoundEffect()
    {
        if (!isSoundEffectPlaying && currentSoundEffect != null)
        {
            soundEffects.clip = currentSoundEffect;  // Set the previously played clip
            soundEffects.time = soundEffectTime;     // Resume from the stored time
            soundEffects.Play();                     // Play the sound effect
            isSoundEffectPlaying = true;
        }
    }

    public bool getMuteState()
    {
        return isMuted;
    }
   
}
