using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OzyamaFall : MonoBehaviour
{
    private int ozyamaChara;
    private int motion;
    public Animator anim;
    public OppaiManager oppaiManager;
    // Start is called before the first frame update
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
            FallObject();
        }
        ScheduleNextNoise(); // 次のノイズスケジュールを設定
    }

    public void FallObject()
    {
        //キャラを選択
        ozyamaChara = GetRandomChoice(50);
        //モーションを選択
        motion = GetRandomChoice(50);
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
        yield return new WaitForSeconds(0.5f);

        Debug.Log("監視開始（1秒間）");

        float timer = 0f;
        bool becameTrue = false;

        while (timer < 1f)
        {
            if (oppaiManager.isHolding)
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
            // ここに必要な処理を追加
        }
        else
        {
            Debug.Log("1秒間のうちにtargetBoolがtrueになりました！");
        }
    }

    private void Surprised()
    {

    }

    private int GetRandomChoice(float chanceForOne)
    {
        float rand = Random.Range(0f, 100f);
        return rand < chanceForOne ? 1 : 0;
    }
}
