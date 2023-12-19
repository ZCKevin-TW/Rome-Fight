using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 60;
    // private bool timerIsRunning = false;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameController gameController;
    private void Start()
    {
        // Starts the timer automatically
        // timerIsRunning = true;
    }
    void Update()
    {
        if (gameController.battling)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                // timerIsRunning = false;
                gameController.GameFinished(true);
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}