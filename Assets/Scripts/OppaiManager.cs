using UnityEngine;

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
                animePattern = ChangeAnimation(animePattern);//アニメのpatternを変える処理
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
        Debug.Log("長押し開始");
    }

    void OnHolding()
    {
        Debug.Log("長押し中...");
        sleepManager.CalSleepDeepOppai(animePattern, whoChara);
        ecstasyManager.CalEcstasy(animePattern,whoChara);
    }

    void OnHoldEnd()
    {
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
        int nextpattern = (pattern + 1) % 5;
        Debug.Log($"アニメパターン{nextpattern}");
        return nextpattern;
    }
    public int GetChara()
    {
        return whoChara;
    }
}
