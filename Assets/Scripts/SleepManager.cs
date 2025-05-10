using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepManager : MonoBehaviour
{
    public float sleepDeep;
    private int who;
    public float wakeup;
    public int facePattern;
    public OppaiManager oppaiManager;
    private Coroutine faceCoroutine;
    public Animator anim;
    public Animator oppai;
    public Animator gameover;

    [SerializeField] private Image whiteScreen; // ホワイトアウト・ホワイトイン用のイメージ
    [SerializeField] private RetryButton RetryButton;
    [SerializeField] private ExitButtonUI exitButtonUI;
    [SerializeField] private OzyamaFall OzyamaFall;
    [SerializeField] private GameoverSprite gameoversprite;
    [SerializeField] private TextCut textCut;

    // Inspector から編集可能な値
    [Header("お空 (okuu) の睡眠度増加値")]
    public float[] okuuAddPoints = { 0.01f, 0.015f, 0.02f, 0.04f, 0.05f };

    [Header("おりん (orin) の睡眠度増加値")]
    public float[] orinAddPoints = { 0.01f, 0.02f, 0.03f, 0.05f, 0.07f };

    [Header("さとり (satori) の睡眠度増加値")]
    public float[] satoriAddPoints = { 0.002f, 0.03f, 0.04f, 0.06f, 0.08f };

    [Header("PlusSleepDeepNotHolding の設定")]
    [SerializeField] private float finalStageIncrease = 0.03f; // 最終段階以上で増加する値
    [SerializeField] private float halfStageIncrease = 0.02f;  // 半分以上で増加する値
    [SerializeField] private float belowHalfDecrease = -0.002f; // 半分以下で減少する値

    [Header("WatchBool の設定")]
    [SerializeField] private float wakeUpDuration = 3f; // 起きたままでいる時間

    void Start()
    {
        // whiteScreen の初期化を強制
        if (whiteScreen != null)
        {
            whiteScreen.color = new Color(134 / 255f, 0, 142 / 255f, 90 / 255f);
            Debug.Log($"whiteScreen initialized to: {whiteScreen.color}");
        }
        who = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
        anim.SetInteger("difficult", who);

        StartFace();
    }

    public void StartFace() // 顔の定期更新をする
    {
        faceCoroutine = StartCoroutine(FaceMethod());
    }

    public void Stopface()
    {
        if (faceCoroutine != null)
        {
            StopCoroutine(faceCoroutine);
            faceCoroutine = null;
        }
    }

    IEnumerator FaceMethod() // 顔の定期更新する中身
    {
        while (true)
        {
            int newFacePattern = 0;

            if ((wakeup / 2) >= sleepDeep)
                newFacePattern = 0;
            else if ((wakeup / 1.5) >= sleepDeep)
                newFacePattern = 1;
            else if ((wakeup / 1.3) >= sleepDeep)
                newFacePattern = 2;
            else if ((wakeup / 1.2) >= sleepDeep)
                newFacePattern = 3;
            else if ((wakeup / 1.1) >= sleepDeep)
                newFacePattern = 4;
            else
                newFacePattern = 4;

            if (newFacePattern != facePattern)
            {
                facePattern = newFacePattern;
                anim.SetInteger("face", facePattern);
                Debug.Log($"SleepDeep: {sleepDeep}, Wakeup: {wakeup}, New FacePattern: {facePattern}");
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    public void CalSleepDeepOppai(int pattern, int who) // 睡眠度をおっぱいで足す
    {
        float addpoint = 0;
        switch (who)
        {
            case 0: // お空 (okuu)
                if (pattern >= 0 && pattern < okuuAddPoints.Length)
                    addpoint = okuuAddPoints[pattern];
                break;

            case 1: // おりん (orin)
                if (pattern >= 0 && pattern < orinAddPoints.Length)
                    addpoint = orinAddPoints[pattern];
                break;

            case 2: // さとり (satori)
                if (pattern >= 0 && pattern < satoriAddPoints.Length)
                    addpoint = satoriAddPoints[pattern];
                break;
        }
        AddSleepDeep(addpoint);
    }

    public void PlusSleepDeepFallObj(float value) // ものが落ちてきたときに睡眠度を足す
    {
        AddSleepDeep(value);
    }

    public void PlusSleepDeepNotHolding() // おっぱいに触れていないとき
    {
        float addpoint = 0;
        if (sleepDeep >= (wakeup / 1.1))
        {
            addpoint += finalStageIncrease; // 最終段階以上でさらに増加
        }
        else if (sleepDeep >= 50)
        {
            addpoint += halfStageIncrease; // 睡眠ゲージが半分以上で増加
        }
        else
        {
            addpoint += belowHalfDecrease; // 睡眠ゲージが半分以下で減少
        }
        AddSleepDeep(addpoint);
    }

    public void WakeUpChara() // キャラクターが起きる時の処理
    {
        OzyamaFall.isAllow = false;
        Debug.Log("おきた");
        sleepDeep = 0;
        StartCoroutine(WatchBool());

    }

    IEnumerator WatchBool()
    {
        OzyamaFall.SetFallDuringBool(true);
        // OzyamaFall.anim.SetBool("fallBool", OzyamaFall.GetFallDuringBool());

        // 参照先に存在していないのでとりあえずコメントアウト
        anim.SetTrigger("up");
        yield return new WaitForSeconds(0.4f);

        Debug.Log("監視開始");

        float timer = 0f;
        bool becameTrue = false;
        while (timer < wakeUpDuration) // 起きたままの時間を比較
        {
            if (oppaiManager.isHolding || OzyamaFall.GetSurprisedBool())//つかむかお邪魔にひっかかったか
            {
                becameTrue = true;
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        if (!becameTrue)
        {
            Debug.Log("1秒間、targetBoolは一度もtrueになりませんでした！");
            OzyamaFall.isAllow = true;
            anim.SetTrigger("Sleep");
            OzyamaFall.SetFallDuringBool(false);


            // OzyamaFall.anim.SetBool("fallBool", OzyamaFall.GetFallDuringBool());
            // 参照先に存在していないのでとりあえずコメントアウト
        }
        else
        {
            Debug.Log("1秒間のうちにtargetBoolがtrueになりました！");
            StartCoroutine(Failed());
        }
    }

    IEnumerator Failed()
    {
        OzyamaFall.isAllow = false;
        oppaiManager.StopOppai();

        StartCoroutine(FadeOut());

        yield return new WaitForSeconds(2f);// 4秒待つ
        textCut.CutScene(textCut.Failed, true);
        yield return new WaitForSeconds(2f);// 3秒待つ
        oppaiManager.isTouch = false;
        gameoversprite.gameover();

        RetryButton.EnableButton();
        exitButtonUI.EnableButton();
    }

    private void AddSleepDeep(float value) // 直接睡眠度を足し起きるか判定
    {
        sleepDeep = Mathf.Clamp(sleepDeep + value, 0, wakeup);
        if (sleepDeep == wakeup)
        {
            WakeUpChara(); // おはようございます
        }
    }

    IEnumerator FadeOut()
    {
        float startAlpha = 50f; // 初期アルファ値 (0~255 の範囲)
        float duration = 5f; // フェードアウトにかける時間（秒）
        float elapsed = 0f; // 経過時間

        // duration 秒間かけてフェードアウト
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime; // 経過時間を加算
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration); // アルファ値を補間 (0~255 の範囲)
            float normalizedAlpha = alpha / 255f; // 0~1 の範囲に変換
            whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, normalizedAlpha); // 色を更新
            yield return 3f; // 次のフレームまで待機
        }

        // 最後に完全に透明に設定
        whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, 0f);
    }
}
