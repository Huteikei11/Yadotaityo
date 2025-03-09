using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool isRunning = false;

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }

    public float GetTime()
    {
        return elapsedTime;
    }

    public string GetFormattedTime()
    {
        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);
        int centiseconds = (int)((elapsedTime * 100) % 100);
        return $"{minutes:00}:{seconds:00}:{centiseconds:00}";
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }
}
