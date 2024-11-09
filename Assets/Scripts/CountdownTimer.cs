using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TargetSpawner targetSpawner;
    private float timeRemaining = 30.0f;
    private int previousSecond;
    private bool hasPlayedTimeUpSound = false;
    private bool countdownStarted = false;
    private float delayBeforeStart = 3.0f;

    void Start()
    {
        previousSecond = Mathf.FloorToInt(timeRemaining);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!countdownStarted || (timeRemaining <= 0 && hasPlayedTimeUpSound))
            {
                ResetTimer();
                countdownStarted = true;
                delayBeforeStart = 3.0f;
                SoundManager.Instance.countDown.Play();
            }
        }

        if (countdownStarted && delayBeforeStart > 0)
        {
            delayBeforeStart -= Time.deltaTime;
            return;
        }

        if (countdownStarted && delayBeforeStart <= 0 && !targetSpawner.TargetSpawnerActive)
        {
            targetSpawner.TargetSpawnerActive = true;
            targetSpawner.SpawnTargetsAtCenters();
        }

        if (countdownStarted && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
            CheckForBeep(Mathf.FloorToInt(timeRemaining));
        }
        else if (countdownStarted && timeRemaining <= 0)
        {
            timeRemaining = 0;
            DisplayTime(timeRemaining);

            if (!hasPlayedTimeUpSound)
            {
                SoundManager.Instance.beepTone.Play();
                SoundManager.Instance.timeUp.Play();
                targetSpawner.StopSpawning();
                targetSpawner.DestroyAllTargets();
                hasPlayedTimeUpSound = true;
                countdownStarted = false;
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
        if ((currentSecond == 2 || currentSecond == 1 || currentSecond == 0) && currentSecond != previousSecond)
        {
            SoundManager.Instance.beepTone.Play();
            previousSecond = currentSecond;
        }
    }

    void ResetTimer()
    {
        timeRemaining = 30.0f;
        previousSecond = Mathf.FloorToInt(timeRemaining);
        hasPlayedTimeUpSound = false;
        countdownStarted = false;
        delayBeforeStart = 3.0f;
        targetSpawner.DestroyAllTargets();
        targetSpawner.ResetScore();
        targetSpawner.TargetSpawnerActive = false;
    }
}