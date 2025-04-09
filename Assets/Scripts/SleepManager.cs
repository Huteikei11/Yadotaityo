using System.Collections;
using System.Collections.Generic;
using System.Net;
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
    // Start is called before the first frame update
    void Start()
    {
        who = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
        anim.SetInteger("difficult", who);
        StartFace();
    }

    public void StartFace()//顔の定期更新をする
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

    IEnumerator FaceMethod() //顔の定期更新する中身
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

    public void CalSleepDeepOppai(int pattern,int who) //睡眠度をおっぱいで足す
    {//おっぱい触ってないときは呼ばれないはずなので大丈夫
        float addpoint = 0;
        switch (who) 
        {
            case 0://お空ちゃん
                switch (pattern)
                {
                    case 0:
                        addpoint = 0.01f;
                        break;
                    case 1:
                        addpoint = 0.015f;
                        break;

                    case 2:
                        addpoint = 0.02f;
                        break;

                    case 3:
                        addpoint = 0.04f;
                        break;
                    case 4:
                        addpoint = 0.05f;
                        break;
                }
                break;

            case 1://おりんちゃん
                switch (pattern)
                {
                    case 0:
                        addpoint = 0.01f;
                        break;
                    case 1:
                        addpoint = 0.02f;
                        break;

                    case 2:
                        addpoint = 0.03f;
                        break;

                    case 3:
                        addpoint = 0.05f;
                        break;
                    case 4:
                        addpoint = 0.07f;
                        break;
                }
                break;

            case 2://さとりちゃん
                switch (pattern)
                {
                    case 0:
                        addpoint = 0.002f;
                        break;
                    case 1:
                        addpoint = 0.03f;
                        break;

                    case 2:
                        addpoint = 0.04f;
                        break;

                    case 3:
                        addpoint = 0.06f;
                        break;
                    case 4:
                        addpoint = 0.08f;
                        break;
                }
                break;
        }
        AddSleepDeep(addpoint);
    }
    public void PlusSleepDeepFallObj(float value)//ものが落ちてきたときに睡眠度を足す
    {
        AddSleepDeep(value);
    }

    public void PlusSleepDeepNotHolding()//おっぱいに触れていないとき
    {
        float addpoint = 0;
        if (sleepDeep >= (wakeup / 1.1))
        {
            addpoint += 0.03f;//睡眠ゲージが最終段階以上でさらに増加
        }
        else if (sleepDeep >= 50)
        {
            addpoint += 0.02f;//睡眠ゲージが半分以上で増加
        }
        else
        {
            addpoint -= 0.002f; //睡眠ゲージが半分以下で減少
        }
        AddSleepDeep(addpoint);
    }

    public void WakeUpChara()//キャラクターが起きる時の処理
    {
        OzyamaFall.isAllow = false;
        Debug.Log("おきた");
        sleepDeep = 0;
        StartCoroutine(WatchBool());

        
        
    }

    IEnumerator WatchBool()
    {
        anim.SetTrigger("up");
        // まず待つ
        yield return new WaitForSeconds(0.4f);

        Debug.Log("監視開始");

        float timer = 0f;
        bool becameTrue = false;
        while (timer < 3f)//三秒おきたまま
        {
            if (oppaiManager.isHolding|| Input.GetMouseButton(0))
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
            //また寝る
            OzyamaFall.isAllow = true;
            anim.SetTrigger("Sleep");
        }
        else
        {
            Debug.Log("1秒間のうちにtargetBoolがtrueになりました！");
            StartCoroutine(Failed());
        }


    }

    IEnumerator Failed()
    {
        //oppai.speed = 0f;
        //yield return new WaitForSeconds(4f);
        OzyamaFall.isAllow = false;//ヤマメキスメを止める

        //明るくする
        whiteScreen.color = new Color(1, 1, 1, 22/255f);
        yield return new WaitForSeconds(0.2f);
        whiteScreen.color = new Color(0, 0, 0, 100/255f);
        yield return new WaitForSeconds(1f);

        // ホワイトアウト開始
        for (float t = 0; t < 22/255f; t += Time.deltaTime/5)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }

        //ゲームオーバー画面表示
        yield return new WaitForSeconds(3f);
        oppaiManager.isTouch = false;
        gameoversprite.gameover();

        //ボタンを表示
        RetryButton.EnableButton();
        exitButtonUI.EnableButton();
    }

    private void AddSleepDeep(float value)//直接睡眠度を足し起きるか判定
    {
        sleepDeep = Mathf.Clamp(sleepDeep + value, 0, wakeup);
        if (sleepDeep == wakeup)
        {
            WakeUpChara();//おはようございます
        }
    }
}
