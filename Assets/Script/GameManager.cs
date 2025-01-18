using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameStateManager;
using static ConstVariable;
public class GameManager : MonoBehaviour
{
    
    public enum GameChoice
	{
        None = 5,
        Rock = 0,
		Paper = 1,
        Scissors = 2,
        Lizard = 3,
        Spock = 4,

	}


    float rotationSpeed = 50f, alignmentSpeed = 5f,alignmentDuration = 1f,collapsetDuration = 2f;
    Vector3 alignedPosition = new Vector3(-420, 0, 0);

    GameChoice playerChoice, opponentChoice;

    public Timer gameTimer;

    [SerializeField]
    GameObject allButtons;
    
    [SerializeField]
    GameObject playButton; 

    [SerializeField]
    GameObject[] revolvingObjects;  

    [SerializeField]
    Animator startGameAnimator;

    [SerializeField]
    GameObject myChoiceObject;

    [SerializeField]
    GameObject opponentChoiceObject;

    [SerializeField]
    Image opponentChoiceObjectImage;

    [SerializeField]
    Image myChoiceObjectImage;

    [SerializeField]
    public Sprite[] spriteList; 

    [SerializeField]
    GameObject tapToContinueObj;

    [SerializeField]
    TextMeshProUGUI winText;

    [SerializeField]
    GameObject verticalLine;
    public Score highScore;

    private static GameManager _instance;

    public static GameManager Instance
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

    void Start()
    {
        if(gameStateManagerInstance != null)
        {
            gameStateManagerInstance.setCurrentGameState(GameStates.MainMenu);
        }   
        playerChoice = GameChoice.None;
        opponentChoice = GameChoice.None;
        opponentChoiceObject.SetActive(false);
        myChoiceObject.SetActive(false);
        opponentChoiceObjectImage = opponentChoiceObject.GetComponentsInChildren<Image>()[1];
        myChoiceObjectImage = myChoiceObject.GetComponentsInChildren<Image>()[1];
    }
    void Update()
    {
        if(gameStateManagerInstance.getCurrentGameState() != GameStates.Pause)
        {
            if(gameStateManagerInstance.getCurrentGameState() == GameStates.MainMenu)
            {
                RotateObjects();
            }
            else if(gameStateManagerInstance.getCurrentGameState() == GameStates.Playing)
            {
                showRandomImagesForOpponent();
            } 
        }
    }
    // Rotating objects on main screen 
    private void RotateObjects()
    {
        foreach (var obj in revolvingObjects)
        {
            obj.transform.RotateAround(playButton.transform.position, Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
    }

    // on clicking play button on main screen
    public void OnPlayButtonClicked()
    {
       if(gameStateManagerInstance.getCurrentGameState() == GameStates.MainMenu)
       {
            highScore.showScore(false);
            SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.buttonClick);
            startGameAnimator.Play("ShowGameAnimation");
            
            StartCoroutine(AlignObjects());
            for (int i = 0; i < revolvingObjects.Length; i++)
            {
                revolvingObjects[i].transform.localRotation  = Quaternion.identity;
            }
       }
    }

    // Align button into straight line from any position
    private IEnumerator AlignObjects()
    {
        float elapsedTime = 0f;
        if(gameStateManagerInstance != null)
        {
            gameStateManagerInstance.setCurrentGameState(GameStates.Playing);
        }

        while (elapsedTime < alignmentDuration)
        {
            for (int i = 0; i < revolvingObjects.Length; i++)
            {
                // Lerp each object to the aligned position
                Vector3 targetPosition = new Vector3(alignedPosition.x + i * 210f, alignedPosition.y - 350f, 0f);
                Vector3 newLerpPosition = Vector3.Lerp(
                    revolvingObjects[i].transform.localPosition,
                    targetPosition,
                    alignmentSpeed * Time.deltaTime
                );
                
                revolvingObjects[i].transform.localPosition = newLerpPosition;//new Vector3(newLerpPosition.x,newLerpPosition.y);
            }
            // Update elapsed time
            elapsedTime += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }
        for (int i = 0; i < revolvingObjects.Length; i++)
        {
            revolvingObjects[i].transform.localPosition  = new Vector3(alignedPosition.x + i * 210f, alignedPosition.y - 350f, 0f);
        }
        
        opponentChoiceObject.SetActive(true);
        
        gameTimer.startTimer();        
    }

    // showin grandom images for oppponent
    public void showRandomImagesForOpponent()
    {
        opponentChoiceObjectImage.sprite = spriteList[currentSpriteIndex];
        currentSpriteIndex = (currentSpriteIndex + 1) % spriteList.Length;
    } 

    // On making any choice  of character
    public void OnButtonClicked(GameObject obj)
    {
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.buttonClick);
        if(gameStateManagerInstance.getCurrentGameState() == GameStates.Playing)
        {
            if(gameStateManagerInstance != null)
            {
                gameStateManagerInstance.setCurrentGameState(GameStates.Result);
            }
            gameTimer.toggleTimer(false);
            StartCoroutine(collapseAllObjects());
            StartCoroutine(setResultStage(obj.name,1f ));
        }
        else if(gameStateManagerInstance.getCurrentGameState() == GameStates.MainMenu)
        {
            if(Popup.Instance != null)
            {
                Popup.Instance.openPopup(Popup.PopupTypes.Text, obj.name);
            }
        }
    }
    // Animation after choosing one character
    private IEnumerator collapseAllObjects()
    {
        float elapsedTime = 0f;
        while (elapsedTime < collapsetDuration)
        {
            for (int i = 0; i < revolvingObjects.Length; i++)
            {
                Vector3 targetPosition = new Vector3(0, - 350f, 0f);
                Vector3 newLerpPosition = Vector3.Lerp(
                    revolvingObjects[i].transform.localPosition,
                    targetPosition,
                    alignmentSpeed * Time.deltaTime
                );
                
                revolvingObjects[i].transform.localPosition = newLerpPosition;
            }
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        

        for (int i = 0; i < revolvingObjects.Length; i++)
        {
            revolvingObjects[i].transform.localPosition  = new Vector3(0,  - 350f, 0f);
        }
        
    }
    // After making choice or timer finished setting the result 
    public IEnumerator setResultStage(string name, float secToWait)
    {
        yield return new WaitForSeconds(secToWait);
        if(!string.IsNullOrEmpty(name))
        {
            Enum.TryParse(name, true, out playerChoice);
            myChoiceObjectImage.sprite = spriteList[(int)playerChoice];
            myChoiceObject.SetActive(true);
        }
        else
        {
            playerChoice = GameChoice.None;
        }

        determineOpponentChoice();
        determineWinner();
    }
   
    int currentSpriteIndex = 0;    
    // Randomly make opponent choice 
    void determineOpponentChoice()
    {
        int choice = UnityEngine.Random.Range(0, 5);
        opponentChoice = (GameChoice)Enum.ToObject(typeof(GameChoice), choice);
        opponentChoiceObjectImage.sprite = spriteList[choice];
    }

    // Determining winner
    void determineWinner()
    {
        verticalLine.SetActive(false);
        
        winText.gameObject.SetActive(true);
        winText.text = "";
        if(playerChoice == GameChoice.None)
        {
            openGameOverPopup();
            highScore.resetCurrentScore();
            winText.text =  "You Lose!";   
            tapToContinueObj.SetActive(true);     
            return;
        }

        if (playerChoice == opponentChoice)
        {
            winText.text = "It's a tie!";           
        }
        else
        {
            if (
                (playerChoice == GameChoice.Rock && (opponentChoice == GameChoice.Scissors || opponentChoice == GameChoice.Lizard)) ||
                (playerChoice == GameChoice.Paper && (opponentChoice == GameChoice.Rock || opponentChoice == GameChoice.Spock)) ||
                (playerChoice == GameChoice.Scissors && (opponentChoice == GameChoice.Paper || opponentChoice == GameChoice.Lizard)) ||
                (playerChoice == GameChoice.Lizard && (opponentChoice == GameChoice.Spock || opponentChoice == GameChoice.Paper)) ||
                (playerChoice == GameChoice.Spock && (opponentChoice == GameChoice.Scissors || opponentChoice == GameChoice.Rock))
            )
            {
                highScore.increaseCurrentScore();
                winText.text = "You Wins!";
            }
            else
            {
                openGameOverPopup();
                highScore.resetCurrentScore();
                winText.text =  "You Lose!";
            }

        }

        tapToContinueObj.SetActive(true);
    }

    // Show gameover popup if lose.
    void openGameOverPopup()
    {
        Popup.Instance.openPopup(Popup.PopupTypes.GameOver);

    }
    
    // reset game to play new round
    public void ResetGame()
    {
        if(!Popup.Instance.getIsPopupOpen())
        {
            tapToContinueObj.SetActive(false);
            verticalLine.SetActive(true);
            winText.gameObject.SetActive(false);
            myChoiceObject.SetActive(false);
            gameTimer.toggleTimer(true);
            gameTimer.resetTimer();
            StartCoroutine(AlignObjects());
        }
    }
   

    // You can also reset the revolving objects if needed, for example, when starting a new game
    public void ResetObjects()
    {
        foreach (var obj in revolvingObjects)
        {
            obj.transform.position = playButton.transform.position + new Vector3(0, 0, 0);  // Set to original position
        }
    }

    

}
