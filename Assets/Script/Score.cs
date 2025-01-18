using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ConstVariable;

public class Score : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI score;

    int highScore = 0;

    int currentScore = 0;

    bool isHighScoreMade = false;
    void Start()
    {
        if(PlayerPrefs.HasKey(HighScoreValue))
        {
            highScore = PlayerPrefs.GetInt(HighScoreValue);
            showScore();
        }
        else
        {
            score.text = "High Score : 0";
            PlayerPrefs.SetInt(HighScoreValue, 0);
            showScore();
        }
    } 

    public bool getIsHighScoreMade()
    {
        return isHighScoreMade;
    }
    public void setIsHighScoreMade(bool value)
    {
        isHighScoreMade = value;
    }
    public int getCurrentScore()
    {
        return currentScore;
    }
    public int getHighScore()
    {
        return highScore;
    }
    void setNewHighScore()
    {
        isHighScoreMade = true;
        highScore = currentScore;
        PlayerPrefs.SetInt(HighScoreValue, currentScore);
    }

    void compareCurrentAndHighScore()
    {
        if(currentScore > highScore )
        {
            setNewHighScore();
        }
    }

    public void increaseCurrentScore()
    {
        currentScore++;
        showScore(false);
        compareCurrentAndHighScore();
    }

    public void resetCurrentScore()
    {
        currentScore = 0;
        showScore(false);
    }

    public void showScore(bool showHighScore = true)
    {
        if(showHighScore)
        {
            score.text = "High Score : " + highScore;
        }
        else
        {
            score.text = "Score : " + currentScore;
        }
    }
    

}
