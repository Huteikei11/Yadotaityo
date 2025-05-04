using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class scoreBoardRecord : MonoBehaviour
{
    private const string SaveKeyPrefix = "HighScores_"; // ��Փx���Ƃ̃L�[�̃v���t�B�b�N�X
    private Dictionary<int, List<int>> highScores = new Dictionary<int, List<int>>()
    {
        { 0, new List<int>() }, // Easy
        { 1, new List<int>() }, // Normal
        { 2, new List<int>() }  // Hard
    };

    public TextMeshProUGUI[] scoreTexts = new TextMeshProUGUI[3];

    void Start()
    {
        LoadHighScores(); // �N�����ɑS��Փx�̃f�[�^�����[�h
        for (int i = 0;i < scoreTexts.Length;i++)
        {
            UpdateScoreDisplay(i);
        }
    }

    private void LoadHighScores()
    {
        List<int> difficultyLevels = new List<int>(highScores.Keys); // �L�[�̃R�s�[���쐬

        foreach (int difficulty in difficultyLevels)
        {
            string key = SaveKeyPrefix + difficulty;
            if (ES3.KeyExists(key))
            {
                highScores[difficulty] = ES3.Load<List<int>>(key);
            }
        }
    }

    // �w�肵����Փx�̃����L���O�𕶎��񃊃X�g�Ƃ��Ď擾
    public List<string> GetHighScoresAsString(int difficulty = 0)
    {
        List<string> scoreStrings = new List<string>();

        if (!highScores.ContainsKey(difficulty)) return scoreStrings;

        foreach (int time in highScores[difficulty])
        {
            int minutes = (time / 60000);
            int seconds = (time / 1000) % 60;
            int milliseconds = (time % 1000) / 10;
            scoreStrings.Add($"{minutes:00}:{seconds:00}:{milliseconds:00}");
        }

        return scoreStrings;
    }

    // UI�ɃX�R�A��\���i��Փx���ƁA�O������Ăяo���\�j
    public void UpdateScoreDisplay(int difficulty = 0)
    {
        List<string> scores = GetHighScoresAsString(difficulty);
        scoreTexts[difficulty].text = "";
        for (int i = 0; i < scores.Count; i++)
        {
                if (scoreTexts[i] != null)
                {
                    scoreTexts[difficulty].text += scores[i]+"\n"; // �X�R�A��\��
                }
                else
                {
                    scoreTexts[difficulty].text += "--:--:--\n"; // �f�[�^�Ȃ��̂Ƃ�
                }
        }
    }
}
