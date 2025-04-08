using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcstasyManager : MonoBehaviour
{
    public float ecstacyGage;
    [SerializeField] private GameManager gameManager;
    private bool isEcstasy = false;
    [SerializeField] private OzyamaFall OzyamaFall;
    public bool isAllowCal;

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CalEcstasy(int pattern,int who)
    {
        float addpoint = 0;
        switch (who)
        {
            case 0://お空ちゃん
                switch (pattern)
                {
                    case 0:
                        addpoint = 0.001f;
                        break;
                    case 1:
                        addpoint += 0.0015f;
                        break;

                    case 2:
                        addpoint += 0.002f;
                        break;

                    case 3:
                        addpoint += 0.005f;
                        break;
                    case 4:
                        addpoint += 0.01f;
                        addpoint += 60.01f;
                        break;
                }
                break;

            case 1://おりんちゃん
                switch (who)
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
                switch (who)
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
        if (isAllowCal)
        {
           AddEcstacy(addpoint);
        }
    }
    public void AddEcstacy(float delta) 
    {
        if (!isEcstasy)//射精していたら100のまま
        {
            ecstacyGage = Mathf.Clamp(ecstacyGage + delta, 0, 100);
            //Debug.Log($"射精ゲージ{ecstacyGage}");
            if (ecstacyGage >= 100)//射精するか判定
            {
                isEcstasy = true;
                Ecstasy();
            }
        }
    }
    private void Ecstasy()//射精の演出
    {
        OzyamaFall.isAllow = false;
        Debug.Log("射精!");
        gameManager.FinishGame();
    }
}
