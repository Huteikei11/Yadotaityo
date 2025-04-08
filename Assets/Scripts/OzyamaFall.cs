using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OzyamaFall : MonoBehaviour
{
    private int ozyamaChara;
    private int motion;
    public Animator anim;
    public OppaiManager oppaiManager;

    [Tooltip("揺らす対象のオブジェクト")]
    public Transform[] targets;

    private Vector3[] initPositions;
    private Tweener[] punchTweeners;
    public Animator oppai;
    [SerializeField] EcstasyManager EcstasyManager;

    public bool isAllow = true;
    // Start is called before the first frame update

    void Awake()
    {
        initPositions = new Vector3[targets.Length];
        punchTweeners = new Tweener[targets.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != null)
                initPositions[i] = targets[i].position;
        }
    }

    void Start()
    {
        ScheduleNextNoise(); // 次のノイズスケジュールを設定
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //ノイズを加える時間
    private void ScheduleNextNoise()
    {
        float nextTime = Random.Range(5f, 7f); // 1〜5秒のランダムな時間を設定
        Invoke("CheckAndExecuteNoise", nextTime);
    }


    private void CheckAndExecuteNoise()
    {

        if (Random.value > 0.4f) // 確率で実行(Radom.Valueは0~1.0)
        {
            if (isAllow)
            {
                FallObject();
            }
        }
        ScheduleNextNoise(); // 次のノイズスケジュールを設定
    }

    public void FallObject()
    {
        //キャラを選択
        ozyamaChara = GetRandomChoice(50);
        //モーションを選択
        motion = GetRandomChoice(100);
        anim.SetInteger("Chara", ozyamaChara);
        anim.SetInteger("Motion", motion);
        anim.SetTrigger("Entry");
        if(motion == 1)
        {
            StartCoroutine(WatchBool());
        }
    }

    IEnumerator WatchBool()
    {
        // まず待つ
        yield return new WaitForSeconds(2.5f);

        Debug.Log("監視開始（1秒間）");

        float timer = 0f;
        bool becameTrue = false;

        while (timer < 0.1f)
        {
            if (oppaiManager.isHolding)
            {
                becameTrue = true;
            }
            if (becameTrue)
            {
                oppai.speed = 0f;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        StartPunch(new Vector3(0.5f, 0f, 0f), 1f, 10, 1f);

        if (!becameTrue)
        {
            Debug.Log("1秒間、targetBoolは一度もtrueになりませんでした！");
            // ここに必要な処理を追加
        }
        else
        {
            Debug.Log("1秒間のうちにtargetBoolがtrueになりました！");
            StartCoroutine(Surprised());
        }
    }

    IEnumerator Surprised()
    {
        EcstasyManager.isAllowCal = false;
        yield return new WaitForSeconds(4f);
        oppai.speed = 1f;
        EcstasyManager.isAllowCal = true;
    }

    /// <summary>
    /// 横方向のパンチ（押される）揺れ開始
    /// </summary>
    /// <param name="punch">揺れの強さ（X方向）</param>
    /// <param name="duration">時間</param>
    /// <param name="vibrato">振動の回数</param>
    /// <param name="elasticity">跳ね返り度合い(0〜1)</param>
    public void StartPunch(Vector3 punch, float duration, int vibrato, float elasticity)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue;

            if (punchTweeners[i] != null && punchTweeners[i].IsActive())
            {
                punchTweeners[i].Kill();
                targets[i].position = initPositions[i];
            }

            punchTweeners[i] = targets[i].DOPunchPosition(punch, duration, vibrato, elasticity);
        }
    }

    // 揺れを止めてすべてのオブジェクトを初期位置に戻す（必要なら）
    public void ResetAll()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (punchTweeners[i] != null && punchTweeners[i].IsActive())
                punchTweeners[i].Kill();

            if (targets[i] != null)
                targets[i].position = initPositions[i];
        }
    }

    private int GetRandomChoice(float chanceForOne)
    {
        float rand = Random.Range(0f, 100f);
        return rand < chanceForOne ? 1 : 0;
    }


}


