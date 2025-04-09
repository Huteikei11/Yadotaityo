using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OppaiManager : MonoBehaviour
{
    public bool isTouch; //おっぱいを触る許可
    private float pressTime = 0f;
    public bool isHolding = false; //おっぱいをさわっているか
    public float holdThreshold = 0.5f; // 長押し判定の閾値（秒）
    public int animePattern;//
    public int whoChara; //キャラクターが誰なのか
    [SerializeField] private SleepManager sleepManager;
    [SerializeField] private EcstasyManager ecstasyManager;
    public Animator anim;
    [SerializeField] private Image whiteScreen;

    private void Start()
    {
        whoChara = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
        anim.SetInteger("difficult", whoChara);
    }
    void Update()
    {
        if (isTouch)
        {
            if (Input.GetMouseButtonDown(0))//左クリック
            {
                pressTime = Time.time; // 押した瞬間の時間を記録
            }

            if (Input.GetMouseButtonDown(1))//右クリック
            {
                StartCoroutine(RightClick());
            }

            if (Input.GetMouseButton(0))
            {
                float duration = Time.time - pressTime;
                if (duration >= holdThreshold)
                {
                    if (!isHolding)
                    {
                        isHolding = true;
                        OnHoldStart(); // 長押し開始時の処理
                    }
                    OnHolding(); // 長押し中の処理
                }
                else
                {
                    NotHoding();
                }
            }
            else
            {
                NotHoding();//おっぱい触ってないときの処理
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isHolding)
                {
                    OnHoldEnd(); // 長押し終了時の処理
                    isHolding = false;
                }
                else
                {
                    OnClick(); // 通常クリック処理
                }
            }
        }
    }

    void OnClick()
    {
        Debug.Log("通常クリック");
    }

    void OnHoldStart()
    {
        anim.SetTrigger("Start");
        Debug.Log("長押し開始");
    }

    void OnHolding()
    {
        Debug.Log("長押し中...");
        sleepManager.CalSleepDeepOppai(animePattern, whoChara);
        ecstasyManager.CalEcstasy(animePattern, whoChara);
    }

    public void OnHoldEnd()
    {
        anim.SetTrigger("Close");
        Debug.Log("長押し終了");
    }

    void NotHoding()
    {
        sleepManager.PlusSleepDeepNotHolding();
        ecstasyManager.AddEcstacy(-0.001f);//射精度下がる
    }

    public void StartOppai()//操作を許可
    {
        isTouch = true;
    }
    public void StopOppai() //操作を許可しない
    {
        isTouch = false;
    }

    private int ChangeAnimation(int pattern)
    {
        int nextpattern = (pattern + 1) % 4;
        Debug.Log($"アニメパターン{nextpattern}");
        return nextpattern;
    }
    public int GetChara()
    {
        return whoChara;
    }

    private IEnumerator RightClick()
    {

        if (isHolding)
        {
            // ホワイトアウト開始
            whiteScreen.gameObject.SetActive(true);
            for (float t = 0; t < 1f; t += Time.deltaTime * 4)
            {
                whiteScreen.color = new Color(0.3098039f, 0.1882353f, 0.2980392f, t);
                yield return null;
            }
            animePattern = ChangeAnimation(animePattern);//アニメのpatternを変える処理
            anim.SetInteger("animePattern", animePattern);
            // ホワイトイン開始（画面を白からフェードアウト）
            for (float t = 1f; t > 0; t -= Time.deltaTime * 4)
            {
                whiteScreen.color = new Color(0.3098039f, 0.1882353f, 0.2980392f, t);
                yield return null;
            }
        }

    }
}
