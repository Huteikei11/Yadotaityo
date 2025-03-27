using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image whiteScreen; // ホワイトアウト・ホワイトイン用のイメージ
    [SerializeField] private Animator cutInAnimator; // カットインアニメーション用のアニメーター
    [SerializeField] private RectTransform gameScreen; // ゲーム画面のUIオブジェクト
    [SerializeField] private RectTransform resultScreen; // リザルト画面のUIオブジェクト
    [SerializeField] private TextMeshProUGUI clearTimeText; // クリアタイムを表示するテキスト
    [SerializeField] private List<GameObject> gameObjects; // ゲーム内のオブジェクトリスト

    [SerializeField] private TimerController timerController;
    [SerializeField] private HighScoreManager highScoreManager;

    [SerializeField] private RetryButton RetryButton;
    [SerializeField] private ExitButtonUI exitButtonUI;

    void Start()
    {
        //タイマースタート
        timerController.StartTimer();
    }
    public void FinishGame()
    {
        timerController.StopTimerAndSave();
        highScoreManager.UpdateScoreDisplay();
        StartCoroutine(ResultSequence());
    }

    private IEnumerator ResultSequence()
    {
        // 1. ホワイトアウトとカットイン演出
        yield return StartCoroutine(FinishDirection());

        // 2. ゲーム画面のオブジェクトを非表示にする
        HideGameObjects();

        // 3. 画面スクロール演出
        yield return StartCoroutine(Scroll());

        //ボタンを表示
        RetryButton.EnableButton();
        exitButtonUI.EnableButton();

        // 4. クリアタイム表示
        yield return StartCoroutine(ClearTime());
    }

    private IEnumerator FinishDirection()
    {
        // カットインアニメーションを再生
        cutInAnimator.SetTrigger("Show");

        // ホワイトアウト開始
        whiteScreen.gameObject.SetActive(true);
        for (float t = 0; t < 1f; t += Time.deltaTime*3)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }

        yield return new WaitForSeconds(1f); // アニメーションの待機時間

        // ホワイトイン開始（画面を白からフェードアウト）
        for (float t = 1f; t > 0; t -= Time.deltaTime*2)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }
        whiteScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f); // アニメーションの待機時間
    }

    private void HideGameObjects()
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(false); // ゲームオブジェクトを非表示
        }
    }

    private IEnumerator Scroll()
    {
        float duration = 1.5f; // スクロール演出の所要時間
        float time = 0;
        Vector3 startGamePos = gameScreen.anchoredPosition;
        Vector3 startResultPos = resultScreen.anchoredPosition;
        Vector3 endGamePos = startGamePos + Vector3.left * 550; // ゲーム画面を左に移動
        Vector3 endResultPos = startResultPos + Vector3.left * 550; // リザルト画面を左に移動（右側から出現）

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            gameScreen.anchoredPosition = Vector3.Lerp(startGamePos, endGamePos, t);
            resultScreen.anchoredPosition = Vector3.Lerp(startResultPos, endResultPos, t);
            yield return null;
        }
    }

    private IEnumerator ClearTime()
    {
        clearTimeText.text = ""; // 初期化
        string timeStr = timerController.GetTimeString();

        // 1文字ずつ表示する
        foreach (char c in timeStr)
        {
            clearTimeText.text += c;
            yield return new WaitForSeconds(0.1f); // 文字の表示間隔
        }
    }
}
