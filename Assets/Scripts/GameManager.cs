using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image whiteScreen; // ホワイトアウト�Eホワイトイン用のイメージ
    [SerializeField] private SpriteRenderer KoishiCutIn; // こいし�EカチE��イン
    [SerializeField] private Animator cutInAnimator; // カチE��インアニメーション用のアニメーター
    [SerializeField] private Animator mune; // 胸のアニメーター
    public List<RectTransform> uiScreens; // UI の RectTransform
    public List<Transform> sprites; // スプライト�E Transform
    [SerializeField] private TextMeshProUGUI clearTimeText; // クリアタイムを表示するチE��スチE
    [SerializeField] private List<GameObject> hidegameObjects; // ゲーム冁E�EオブジェクトリスチE

    [SerializeField] private TimerController timerController;
    [SerializeField] private HighScoreManager highScoreManager;

    [SerializeField] private RetryButton RetryButton;
    [SerializeField] private ExitButtonUI exitButtonUI;

    [SerializeField] private Sprite[] semensprites; // スプライトを0,1,2の頁E��入れておく
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private TextCut textCut;
    [SerializeField] private Animator loadAnim;

    [SerializeField] private OppaiManager oppaiManager; // 胸のアニメーター

    public float startTime = 2;//ゲーム開始までの時間


    private int difficulty;
    private int loadNo;

    void Start()
    {
        // コルーチンを呼び出して1フレーム遁E��させめE
        StartCoroutine(DelayedLoadAnimation());

        //タイマ�EスターチE
        difficulty = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
        spriteRenderer.enabled = false; // 最初�E非表示
        StartCoroutine(GameStart());
    }

    private IEnumerator DelayedLoadAnimation()
    {
        // 1フレーム征E��E
        yield return null;

        // loadNo の値に基づぁE��アニメーションを設宁E
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
    }
    private IEnumerator GameStart()
    {
        //こいし�Eスタート�Eトランジションと斁E��E
        yield return new WaitForSeconds(startTime);
        textCut.CutScene(textCut.Start, false);
        yield return new WaitForSeconds(1.5f);
        //胸を開ぁE
        mune.SetTrigger("Open");

        yield return new WaitForSeconds(1.5f);
        timerController.StartTimer(); //ゲーム開始までの処琁E
        oppaiManager.StartOppai();
    }

    public void FinishGame()
    {
        timerController.StopTimerAndSave(difficulty);
        highScoreManager.UpdateScoreDisplay(difficulty);
        StartCoroutine(ResultSequence());
    }


    private IEnumerator ResultSequence()
    {
        // 1. ホワイトアウトとカチE��イン演�E
        yield return StartCoroutine(FinishDirection());

        // 2. 画面スクロール演�E
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
        //yield return new WaitForSeconds(2f); // アニメーションの征E��時閁E


        // ホワイトアウト開姁E
        whiteScreen.gameObject.SetActive(true);
        for (float t = 0; t < 1f; t += Time.deltaTime * 3)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }
        mune.SetBool("Finish", true);//胸を止める

        // ホワイトイン開始（画面を白からフェードアウト！E
        for (float t = 1f; t > 0; t -= Time.deltaTime * 2)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }

        // �J�b�g�C���A�j���[�V�������Đ�
        cutInAnimator.SetTrigger("Show");
        yield return new WaitForSeconds(2f); // アニメーションの征E��時閁E


        for (float t = 0; t < 1f; t += Time.deltaTime * 3)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }

        ShowSprite(difficulty);//精液がかかってぁE��
        cutInAnimator.SetTrigger("End");
        // ホワイトイン開始（画面を白からフェードアウト！E
        for (float t = 1f; t > 0; t -= Time.deltaTime * 2)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            //KoishiCutIn.color = whiteScreen.color;
            yield return null;
        }



        whiteScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f); // アニメーションの征E��時閁E
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
        float slideDistanceUI = 550f; // UIのスライド距離�E�EnchoredPosition 用�E�E
        float slideDistanceSprite = 1.61f; // スプライト�Eスライド距離�E�ワールド座標！E
        float duration = 1.5f; // スライド時閁E
        float time = 0;
        List<Vector2> startUIPositions = new List<Vector2>();
        List<Vector2> endUIPositions = new List<Vector2>();
        List<Vector3> startSpritePositions = new List<Vector3>();
        List<Vector3> endSpritePositions = new List<Vector3>();

        // UI の開始位置と終亁E��置を設宁E
        foreach (var screen in uiScreens)
        {
            startUIPositions.Add(screen.anchoredPosition);
            endUIPositions.Add(screen.anchoredPosition + (Vector2.left * slideDistanceUI));
        }

        // スプライト�E開始位置と終亁E��置を設宁E
        foreach (var sprite in sprites)
        {
            startSpritePositions.Add(sprite.position);
            endSpritePositions.Add(sprite.position + Vector3.left * slideDistanceSprite);
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // UI をスライチE
            for (int i = 0; i < uiScreens.Count; i++)
            {
                uiScreens[i].anchoredPosition = Vector2.Lerp(startUIPositions[i], endUIPositions[i], t);
            }

            // スプライトをスライチE
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
        clearTimeText.text = ""; // 初期匁E
        string timeStr = timerController.GetTimeString();

        // 1斁E��ずつ表示する
        foreach (char c in timeStr)
        {
            clearTimeText.text += c;
            yield return new WaitForSeconds(0.1f); // 斁E���E表示間隔
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
            Debug.LogWarning("不正なモード番号でぁE " + mode);
        }
    }
}
