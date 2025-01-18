using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Slider progressBar;

    float startTime,pausedTime, timerDuration = 3f;

    bool timerRunning = false, timerPaused = false;
    void Update()
    {
        if(timerRunning && !timerPaused && GameStateManager.gameStateManagerInstance.getCurrentGameState() == GameStateManager.GameStates.Playing)
        {
            SetProgress();   
        }
    }

    public void toggleTimer(bool show)
    {
        progressBar.gameObject.SetActive(show);
    }

    public void startTimer()
    {
        progressBar.value = 1;
        startTime = Time.time;
        timerRunning = true;
        timerPaused = false;
    }
    public void PauseTimer()
    {
       if (timerRunning && !timerPaused)
        {
            timerPaused = true;
            pausedTime = Time.time - startTime; 
        }
    }
    public void ResumeTimer()
    {
        if (timerRunning && timerPaused)
        {
            timerPaused = false;
            startTime = Time.time - pausedTime; 
        }
    }
    public void resetTimer()
    {
        timerRunning = false;
        timerPaused = false;
    }
    void SetProgress()
    {

        float elapsedTime = Time.time - startTime;

        float newProgress = Mathf.Lerp(1f, 0f, elapsedTime / timerDuration);

        progressBar.value = newProgress;

        if (elapsedTime >= timerDuration)
        {
            if(GameStateManager.gameStateManagerInstance != null)
            {
                GameStateManager.gameStateManagerInstance.setCurrentGameState(GameStateManager.GameStates.Result);
            }
            StartCoroutine(GameManager.Instance.setResultStage(string.Empty,0f ));
            resetTimer();
        }
    }
}
