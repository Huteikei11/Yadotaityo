using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText; // タイマー表示用のTextMeshPro
    private float elapsedTime = 0f;
    private bool isRunning = false;
    private HighScoreManager highScoreManager; // HighScoreManagerの参照

    void Start()
    {
        highScoreManager = FindObjectOfType<HighScoreManager>(); // シーン内の HighScoreManager を取得
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    // タイマー開始
    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    // タイマー停止
    public void StopTimer()
    {
        isRunning = false;
    }

    // タイマー再開
    public void ResumeTimer()
    {
        isRunning = true;
    }

    // タイマー停止 & 記録保存
    public void StopTimerAndSave(int difficulty = 0)
    {
        StopTimer(); // タイマーを停止
        int timeInMilliseconds = (int)(elapsedTime * 1000); // ミリ秒単位に変換

        if (highScoreManager != null)
        {
            highScoreManager.SaveNewTime(timeInMilliseconds,difficulty); // 記録を保存
        }
        else
        {
            Debug.LogWarning("HighScoreManager が見つかりません！");
        }
    }

    // タイマーの文字列取得 (例: "03:20:95")
    public string GetTimeString()
    {
        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);
        int milliseconds = (int)((elapsedTime * 100) % 100);
        return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }

    // タイマーの数値取得 (ミリ秒単位)
    public int GetTimeNumeric()
    {
        return (int)(elapsedTime * 1000);
    }

    // タイマーの表示更新
    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = GetTimeString();
        }
    }
}
