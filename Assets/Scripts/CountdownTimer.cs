using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeRemaining = 30f;
    private int previousSecond;
    private bool hasPlayedTimeUpSound = false;
    private bool countdownStarted = false;
    private float delayBeforeStart = 3f;

    void Start()
    {
        previousSecond = Mathf.FloorToInt(timeRemaining);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (timeRemaining <= 0 && hasPlayedTimeUpSound)
            {
                ResetTimer();
            }
            countdownStarted = true;
            SoundManager.Instance.countDown.Play();
        }

        if (countdownStarted && delayBeforeStart > 0)
        {
            delayBeforeStart -= Time.deltaTime;
            return;
        }

        if (countdownStarted && delayBeforeStart <= 0)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                CheckForBeep(Mathf.FloorToInt(timeRemaining));
            }
            else
            {
                timeRemaining = 0;
                DisplayTime(timeRemaining);

                if (!hasPlayedTimeUpSound)
                {
                    SoundManager.Instance.timeUp.Play();
                    hasPlayedTimeUpSound = true;
                }
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay = Mathf.Max(0, timeToDisplay);
        int seconds = Mathf.FloorToInt(timeToDisplay);
        int hundredths = Mathf.FloorToInt((timeToDisplay % 1) * 100);

        timerText.text = string.Format("{0:00}.{1:00}", seconds, hundredths);
    }

    void CheckForBeep(int currentSecond)
    {
        if ((currentSecond == 3 || currentSecond == 2 || currentSecond == 1 || currentSecond == 0) && currentSecond != previousSecond)
        {
            SoundManager.Instance.beepTone.Play();
            previousSecond = currentSecond;
        }
    }

    void ResetTimer()
    {
        timeRemaining = 30f;
        previousSecond = Mathf.FloorToInt(timeRemaining);
        hasPlayedTimeUpSound = false;
        countdownStarted = false;
        delayBeforeStart = 3f;
    }
}
