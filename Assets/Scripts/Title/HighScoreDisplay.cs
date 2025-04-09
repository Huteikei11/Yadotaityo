using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HighScoreDisplay : MonoBehaviour
{
    [Header("��Փx���Ƃ̃X�R�A�\���pTextMeshPro (0:Easy, 1:Normal, 2:Hard)")]
    public TextMeshProUGUI[] easyTexts = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] normalTexts = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] hardTexts = new TextMeshProUGUI[3];

    [Header("�w�i�I�u�W�F�N�g")]
    public GameObject background;

    private bool isVisible = false;

    // �O������Ăяo���ĕ\�����X�V���\������
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

    // �e�L�X�gUI�̕\���E��\�����܂Ƃ߂Đ���
    private void SetTextVisibility(bool visible)
    {
        foreach (var text in easyTexts)
            if (text != null) text.gameObject.SetActive(visible);

        foreach (var text in normalTexts)
            if (text != null) text.gameObject.SetActive(visible);

        foreach (var text in hardTexts)
            if (text != null) text.gameObject.SetActive(visible);
    }

    // �X�R�A��UI�ɔ��f�i��Փx�w��j
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

    // �X�R�A�ǂݍ��݁iint�^�Łj
    private List<int> LoadHighScores(int difficulty)
    {
        string key = $"HighScores_{difficulty}";
        if (ES3.KeyExists(key))
        {
            return ES3.Load<List<int>>(key);
        }
        return new List<int>();
    }

    // �\���t�H�[�}�b�g ��: 03:12:58
    private string FormatTime(int time)
    {
        int minutes = time / 60000;
        int seconds = (time / 1000) % 60;
        int milliseconds = (time % 1000) / 10;
        return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }
}
