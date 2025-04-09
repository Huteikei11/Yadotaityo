using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HighScoreDisplay : MonoBehaviour
{
    [Header("難易度ごとのスコア表示用TextMeshPro (0:Easy, 1:Normal, 2:Hard)")]
    public TextMeshProUGUI[] easyTexts = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] normalTexts = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] hardTexts = new TextMeshProUGUI[3];

    [Header("背景オブジェクト")]
    public GameObject background;

    private bool isVisible = false;

    // 外部から呼び出して表示を更新＆表示する
    public void ShowHighScores()
    {
        UpdateScoreUI(0, easyTexts);
        UpdateScoreUI(1, normalTexts);
        UpdateScoreUI(2, hardTexts);

        background.SetActive(true);
        SetTextVisibility(true);
        isVisible = true;
    }

    private void Update()
    {
        if (isVisible && Input.GetMouseButtonDown(0))
        {
            background.SetActive(false);
            SetTextVisibility(false);
            isVisible = false;
        }
    }

    // テキストUIの表示・非表示をまとめて制御
    private void SetTextVisibility(bool visible)
    {
        foreach (var text in easyTexts)
            if (text != null) text.gameObject.SetActive(visible);

        foreach (var text in normalTexts)
            if (text != null) text.gameObject.SetActive(visible);

        foreach (var text in hardTexts)
            if (text != null) text.gameObject.SetActive(visible);
    }

    // スコアをUIに反映（難易度指定）
    private void UpdateScoreUI(int difficulty, TextMeshProUGUI[] textFields)
    {
        List<int> scores = LoadHighScores(difficulty);

        for (int i = 0; i < textFields.Length; i++)
        {
            if (textFields[i] != null)
            {
                if (i < scores.Count)
                {
                    textFields[i].text = FormatTime(scores[i]);
                }
                else
                {
                    textFields[i].text = "--:--:--";
                }
            }
        }
    }

    // スコア読み込み（int型で）
    private List<int> LoadHighScores(int difficulty)
    {
        string key = $"HighScores_{difficulty}";
        if (ES3.KeyExists(key))
        {
            return ES3.Load<List<int>>(key);
        }
        return new List<int>();
    }

    // 表示フォーマット 例: 03:12:58
    private string FormatTime(int time)
    {
        int minutes = time / 60000;
        int seconds = (time / 1000) % 60;
        int milliseconds = (time % 1000) / 10;
        return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }
}
