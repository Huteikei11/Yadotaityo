using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image whiteScreen; // ホワイトアウト・ホワイトイン用のイメージ
    [SerializeField] private SpriteRenderer KoishiCutIn; // こいしのカットイン
    [SerializeField] private Animator cutInAnimator; // カットインアニメーション用のアニメーター
    [SerializeField] private Animator mune; // 胸のアニメーター
    public List<RectTransform> uiScreens; // UI の RectTransform
    public List<Transform> sprites; // スプライトの Transform
    [SerializeField] private TextMeshProUGUI clearTimeText; // クリアタイムを表示するテキスト
    [SerializeField] private List<GameObject> hidegameObjects; // ゲーム内のオブジェクトリスト

    [SerializeField] private TimerController timerController;
    [SerializeField] private HighScoreManager highScoreManager;

    [SerializeField] private RetryButton RetryButton;
    [SerializeField] private ExitButtonUI exitButtonUI;

    [SerializeField] private Sprite[] semensprites; // スプライトを0,1,2の順に入れておく
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private TextCut textCut;
    [SerializeField] private Animator loadAnim;
    public float startTime = 2;//ゲーム開始までの時間


    private int difficulty;
    private int loadNo;

    void Start()
    {
        //タイマースタート

        loadNo = loadManager.Instance != null ? loadManager.Instance.GetLoadNo() : 0;
        switch (loadNo)
        {
            case 0:
                loadAnim.SetTrigger("loadOutTrigger");
                break;
            case 1:
                loadAnim.SetTrigger("loadGameoverTrigger");
                break;
        }
        difficulty = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
        spriteRenderer.enabled = false; // 最初は非表示
        StartCoroutine(GameStart());
    }
    private IEnumerator GameStart()
    {
        //こいしのスタートのトランジションと文字
        yield return new WaitForSeconds(startTime);
        textCut.CutScene(textCut.Start, false);
        yield return new WaitForSeconds(1.5f);
        //胸を開く
        mune.SetTrigger("Open");

        timerController.StartTimer(); //ゲーム開始までの処理
        mune.gameObject.GetComponent<OppaiManager>().StartOppai();
    }

    public void FinishGame()
    {
        timerController.StopTimerAndSave(difficulty);
        highScoreManager.UpdateScoreDisplay(difficulty);
        StartCoroutine(ResultSequence());
    }


    private IEnumerator ResultSequence()
    {
        // 1. ホワイトアウトとカットイン演出
        yield return StartCoroutine(FinishDirection());

        // 2. 画面スクロール演出
        yield return StartCoroutine(Scroll());

        // 3. ゲーム画面のオブジェクトを非表示にする
        HideGameObjects();

        //ボタンを表示
        RetryButton.EnableButton();
        RetryButton.clearFlags();
        exitButtonUI.EnableButton();

        // 4. クリアタイム表示
        yield return StartCoroutine(ClearTime());
    }

    private IEnumerator FinishDirection()
    {
        //yield return new WaitForSeconds(2f); // アニメーションの待機時間


        // ホワイトアウト開始
        whiteScreen.gameObject.SetActive(true);
        for (float t = 0; t < 1f; t += Time.deltaTime * 3)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }
        mune.SetBool("Finish", true);//胸を止める

        // ホワイトイン開始（画面を白からフェードアウト）
        for (float t = 1f; t > 0; t -= Time.deltaTime * 2)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }
        // カットインアニメーションを再生
        cutInAnimator.SetTrigger("Show");
        yield return new WaitForSeconds(2f); // アニメーションの待機時間


        for (float t = 0; t < 1f; t += Time.deltaTime * 3)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }

        ShowSprite(difficulty);//精液がかかっている
        cutInAnimator.SetTrigger("End");
        // ホワイトイン開始（画面を白からフェードアウト）
        for (float t = 1f; t > 0; t -= Time.deltaTime * 2)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            //KoishiCutIn.color = whiteScreen.color;
            yield return null;
        }



        whiteScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f); // アニメーションの待機時間
    }

    private void HideGameObjects()
    {
        foreach (GameObject obj in hidegameObjects)
        {
            obj.SetActive(false); // ゲームオブジェクトを非表示
        }
    }

    private IEnumerator Scroll()
    {
        float slideDistanceUI = 550f; // UIのスライド距離（anchoredPosition 用）
        float slideDistanceSprite = 1.61f; // スプライトのスライド距離（ワールド座標）
        float duration = 1.5f; // スライド時間
        float time = 0;
        List<Vector2> startUIPositions = new List<Vector2>();
        List<Vector2> endUIPositions = new List<Vector2>();
        List<Vector3> startSpritePositions = new List<Vector3>();
        List<Vector3> endSpritePositions = new List<Vector3>();

        // UI の開始位置と終了位置を設定
        foreach (var screen in uiScreens)
        {
            startUIPositions.Add(screen.anchoredPosition);
            endUIPositions.Add(screen.anchoredPosition + (Vector2.left * slideDistanceUI));
        }

        // スプライトの開始位置と終了位置を設定
        foreach (var sprite in sprites)
        {
            startSpritePositions.Add(sprite.position);
            endSpritePositions.Add(sprite.position + Vector3.left * slideDistanceSprite);
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // UI をスライド
            for (int i = 0; i < uiScreens.Count; i++)
            {
                uiScreens[i].anchoredPosition = Vector2.Lerp(startUIPositions[i], endUIPositions[i], t);
            }

            // スプライトをスライド
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].position = Vector3.Lerp(startSpritePositions[i], endSpritePositions[i], t);
            }

            yield return null;
        }

        // 最終位置を補正
        for (int i = 0; i < uiScreens.Count; i++)
        {
            uiScreens[i].anchoredPosition = endUIPositions[i];
        }

        for (int i = 0; i < sprites.Count; i++)
        {
            sprites[i].position = endSpritePositions[i];
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

    public void ShowSprite(int mode)
    {
        if (mode >= 0 && mode < semensprites.Length)
        {
            spriteRenderer.sprite = semensprites[mode];
            spriteRenderer.enabled = true; // 表示
        }
        else
        {
            Debug.LogWarning("不正なモード番号です: " + mode);
        }
    }
}
