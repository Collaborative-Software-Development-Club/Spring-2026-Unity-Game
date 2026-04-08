using UnityEngine;
using TMPro;

public class Stopwatch : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float elapsedTime;
    private bool isRunning = false;

    void Start()
    {
        ResetStopwatch();
        StartStopwatch();
    }
    void Update()
    {
        if (isRunning)
        {
            // Add the time passed since the last frame
            elapsedTime += Time.deltaTime;
            UpdateDisplay();
        }
    }

    void UpdateDisplay()
    {
        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 100) % 100);

        // Format the string as 00:00:00
        timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public void StartStopwatch() => isRunning = true;
    public void PauseStopwatch() => isRunning = false;
    public void ResetStopwatch()
    {
        elapsedTime = 0;
        isRunning = false;
        UpdateDisplay();
    }
}