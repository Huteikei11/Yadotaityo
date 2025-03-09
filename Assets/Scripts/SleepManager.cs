using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SleepManager : MonoBehaviour
{
    public float sleepDeep;
    private int who;
    public float wakeup;
    public int facePattern;
    public OppaiManager oppaiManager;
    private Coroutine faceCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        who = oppaiManager.GetChara();//キャラが誰かを取得
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
                        addpoint += 0.015f;
                        break;

                    case 2:
                        addpoint += 0.02f;
                        break;

                    case 3:
                        addpoint += 0.05f;
                        break;
                    case 4:
                        addpoint += 0.08f;
                        break;
                }
                break;

            case 1://おりんちゃん
                switch (pattern)
                {
                    case 0:
                        addpoint = 0.001f;
                        break;
                    case 1:
                        addpoint += 0.015f;
                        break;

                    case 2:
                        addpoint += 0.02f;
                        break;

                    case 3:
                        addpoint += 0.05f;
                        break;
                    case 4:
                        addpoint += 0.05f;
                        break;
                }
                break;

            case 2://さとりちゃん
                switch (pattern)
                {
                    case 0:
                        addpoint = 0.001f;
                        break;
                    case 1:
                        addpoint += 0.015f;
                        break;

                    case 2:
                        addpoint += 0.02f;
                        break;

                    case 3:
                        addpoint += 0.05f;
                        break;
                    case 4:
                        addpoint += 0.05f;
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
        if (sleepDeep >= 50)
        {
            addpoint += 0.005f;//睡眠ゲージが半分以上で増加
        }
        else
        {
            addpoint -= 0.0015f; //睡眠ゲージが半分以下で減少
        }
        AddSleepDeep(addpoint);
    }

    public void WakeUpChara()//キャラクターが起きる時の処理
    {
        Debug.Log("おきた");
        sleepDeep = 0;
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
