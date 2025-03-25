using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HighScoreManager : MonoBehaviour
{
    private const string SaveKeyPrefix = "HighScores_"; // 難易度ごとのキーのプレフィックス
    private Dictionary<int, List<int>> highScores = new Dictionary<int, List<int>>()
    {
        { 0, new List<int>() }, // Easy
        { 1, new List<int>() }, // Normal
        { 2, new List<int>() }  // Hard
    };

    public TextMeshProUGUI[] scoreTexts = new TextMeshProUGUI[3];

    void Start()
    {
        LoadHighScores(); // 起動時に全難易度のデータをロード
    }

    // 新しいスコアを保存 (難易度ごと) デフォルトは Easy (0)
    public void SaveNewTime(int newTime, int difficulty = 0)
    {
        if (!highScores.ContainsKey(difficulty)) return;

        highScores[difficulty].Add(newTime);
        highScores[difficulty].Sort(); // 小さい順に並び替え

        if (highScores[difficulty].Count > 3)
        {
            highScores[difficulty].RemoveAt(3); // 4位以降を削除
        }

        SaveHighScores(difficulty); // 指定の難易度で保存
    }

    // スコアをロード（難易度ごと）
    private void LoadHighScores()
    {
        List<int> difficultyLevels = new List<int>(highScores.Keys); // キーのコピーを作成

        foreach (int difficulty in difficultyLevels)
        {
            string key = SaveKeyPrefix + difficulty;
            if (ES3.KeyExists(key))
            {
                highScores[difficulty] = ES3.Load<List<int>>(key);
            }
        }
    }

    // スコアを保存（難易度ごと）
    private void SaveHighScores(int difficulty)
    {
        string key = SaveKeyPrefix + difficulty;
        ES3.Save(key, highScores[difficulty]);
    }

    // 指定した難易度のランキングを文字列リストとして取得
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

    // UIにスコアを表示（難易度ごと、外部から呼び出し可能）
    public void UpdateScoreDisplay(int difficulty = 0)
    {
        List<string> scores = GetHighScoresAsString(difficulty);
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (scoreTexts[i] != null)
            {
                if (i < scores.Count)
                {
                    scoreTexts[i].text = scores[i]; // スコアを表示
                }
                else
                {
                    scoreTexts[i].text = "--:--:--"; // データなしのとき
                }
            }
        }
    }
}
