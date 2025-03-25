using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText; // �^�C�}�[�\���p��TextMeshPro
    private float elapsedTime = 0f;
    private bool isRunning = false;
    private HighScoreManager highScoreManager; // HighScoreManager�̎Q��

    void Start()
    {
        highScoreManager = FindObjectOfType<HighScoreManager>(); // �V�[������ HighScoreManager ���擾
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    // �^�C�}�[�J�n
    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    // �^�C�}�[��~
    public void StopTimer()
    {
        isRunning = false;
    }

    // �^�C�}�[�ĊJ
    public void ResumeTimer()
    {
        isRunning = true;
    }

    // �^�C�}�[��~ & �L�^�ۑ�
    public void StopTimerAndSave(int difficulty = 0)
    {
        StopTimer(); // �^�C�}�[���~
        int timeInMilliseconds = (int)(elapsedTime * 1000); // �~���b�P�ʂɕϊ�

        if (highScoreManager != null)
        {
            highScoreManager.SaveNewTime(timeInMilliseconds,difficulty); // �L�^��ۑ�
        }
        else
        {
            Debug.LogWarning("HighScoreManager ��������܂���I");
        }
    }

    // �^�C�}�[�̕�����擾 (��: "03:20:95")
    public string GetTimeString()
    {
        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);
        int milliseconds = (int)((elapsedTime * 100) % 100);
        return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }

    // �^�C�}�[�̐��l�擾 (�~���b�P��)
    public int GetTimeNumeric()
    {
        return (int)(elapsedTime * 1000);
    }

    // �^�C�}�[�̕\���X�V
    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = GetTimeString();
        }
    }
}
