using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameStateManager;

public class Popup : MonoBehaviour
{

    Dictionary<string,string> powerOfEachCharacter = new Dictionary<string, string>{
        {"Rock" , "\n\n.Rock can crush Scissors \n\n .Rock can crush Lizard"},
        {"Paper" , "\n\n.Paper can cover Rock \n\n .Paper can disproves Spock"},
        {"Scissors" , "\n\n.Scissors can cut Paper \n\n .Scissors can decapitates Lizard"},
        {"Lizard" , "\n\n.Lizard can eat paper \n\n .Lizard can poisons Spock"},
        {"Spock" , "\n\n.Spock can smash Scissors \n\n .Spock can vaporize Rock"},

    };

    public enum PopupTypes
	{
        Exit,
		Image,
        Text,
        GameOver,

	}
    // Start is called before the first frame update
    [SerializeField]
    GameObject popupObject;

    public TextMeshProUGUI textToShow;
    public Image imageToShow;

    public GameObject exitToShow;
    public TextMeshProUGUI exitTextToShow;

    public GameObject gameOverToShow;

    public TextMeshProUGUI gameOverHighScoreToShow;

    public TextMeshProUGUI gameOverScoreToShow;
    public GameObject gameOverNewTagToShow;
    private static Popup _instance;
  
    bool isPopupOpen = false;
    public static Popup Instance
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

     private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public bool getIsPopupOpen()
    {
        return isPopupOpen;
    }
    public void openPopup(PopupTypes popType, string whichCharacterSelected = "")
    {
        if(isPopupOpen) return;
        isPopupOpen = true;
        switch(popType)
        {
            case PopupTypes.Exit:
                if(gameStateManagerInstance != null )
                {
                    if(gameStateManagerInstance.getCurrentGameState() == GameStates.MainMenu)
                    {
                        exitTextToShow.text = "Quit Game?";
                    }
                    else
                    {
                        exitTextToShow.text = "Back To Home?";
                    }
                }
                exitToShow.SetActive(true);
                break;
            case PopupTypes.Image:
                imageToShow.gameObject.SetActive(true);
                break;
            case PopupTypes.Text:
                if(powerOfEachCharacter.ContainsKey(whichCharacterSelected))
                {
                    textToShow.text = whichCharacterSelected  + powerOfEachCharacter[whichCharacterSelected];
                    textToShow.gameObject.SetActive(true);
                }
                break;
            case PopupTypes.GameOver:
                gameOverNewTagToShow.SetActive(false);
                gameOverHighScoreToShow.text = "High Score : " +GameManager.Instance.highScore.getHighScore() ;
                gameOverScoreToShow.text = "Score : " + GameManager.Instance.highScore.getCurrentScore(); 
                if(GameManager.Instance.highScore.getIsHighScoreMade())
                {
                    GameManager.Instance.highScore.setIsHighScoreMade(false);
                    gameOverNewTagToShow.SetActive(true);
                }
                gameOverToShow.SetActive(true);
                break;
        }
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popup);
        popupObject.SetActive(true);
        if(popType == PopupTypes.GameOver)
        {
            gameStateManagerInstance.setCurrentGameState(GameStates.GameOver);
        }
        else
        {
            gameStateManagerInstance.setCurrentGameState(GameStates.Pause);
            GameManager.Instance.gameTimer.PauseTimer();
        }
        
    }
    public void onConfirmButtonClick()
    {
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.buttonClick);
        if(gameStateManagerInstance != null )
        {
            if(gameStateManagerInstance.getCurrentGameState() == GameStates.MainMenu)
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
    public void closePopup()
    {
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popup);
        resetAll();
        popupObject.SetActive(false);
        if(gameStateManagerInstance.getPreviousGameState() == GameStates.Playing)
        {
            GameManager.Instance.gameTimer.ResumeTimer();
        }
        gameStateManagerInstance.setCurrentGameState(gameStateManagerInstance.getPreviousGameState());
    }

    void resetAll()
    {
        isPopupOpen = false;
        textToShow.gameObject.SetActive(false);
        imageToShow.gameObject.SetActive(false);
        exitToShow.SetActive(false);
        gameOverToShow.SetActive(false);
    }
}
