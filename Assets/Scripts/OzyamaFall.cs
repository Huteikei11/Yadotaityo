using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OzyamaFall : MonoBehaviour
{
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
        Debug.Log("落下!");
    }
}
