using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Popup;
using static GameStateManager;
using UnityEngine.SceneManagement;
using static ConstVariable;

public class FooterButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite muteButtonImage;
    public Sprite unmuteButtonImage;
    public Image soundButton;

    void Start()
    {
        if(PlayerPrefs.HasKey(SoundSetting))
        {
            if(PlayerPrefs.GetString(SoundSetting) == "true")
            {
                soundButton.sprite = muteButtonImage;
            }
            else
            {
                soundButton.sprite = unmuteButtonImage;
            }
        }
        else
        {
            soundButton.sprite = unmuteButtonImage;
            
        }
    }
    public void onExitButtonClicked()
    {
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.buttonClick);
        if(Popup.Instance != null)
        {
            Popup.Instance.openPopup(PopupTypes.Exit);
        }
    }

    

    public void onInfoButtonClicked()
    {
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.buttonClick);
        if(Popup.Instance != null)
        {
            Popup.Instance.openPopup(PopupTypes.Image);
        }
    }

    public void onSoundButtonClicked()
    {
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.buttonClick);
        SoundManager.Instance.toggleSound();
        if(SoundManager.Instance.getMuteState())
        {
            soundButton.sprite = muteButtonImage;
        }
        else
        {
            soundButton.sprite = unmuteButtonImage;
        }
    }
}
