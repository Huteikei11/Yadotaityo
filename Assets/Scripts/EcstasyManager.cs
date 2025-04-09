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
                        addpoint = 0.002f;
                        break;
                    case 1:
                        addpoint += 0.003f;
                        break;

                    case 2:
                        addpoint += 0.004f;
                        break;

                    case 3:
                        addpoint += 0.005f;
                        break;
                    case 4:
                        addpoint += 0.01f;
                        break;
                }
                break;

            case 1://おりんちゃん
                switch (who)
                {
                    case 0:
                        addpoint = 0.0025f;
                        break;
                    case 1:
                        addpoint += 0.0035f;
                        break;

                    case 2:
                        addpoint += 0.0045f;
                        break;

                    case 3:
                        addpoint += 0.006f;
                        break;
                    case 4:
                        addpoint += 0.011f;
                        break;
                }
                break;

            case 2://さとりちゃん
                switch (who)
                {
                    case 0:
                        addpoint = 0.0025f;
                        break;
                    case 1:
                        addpoint += 0.035f;
                        break;

                    case 2:
                        addpoint += 0.005f;
                        break;

                    case 3:
                        addpoint += 0.007f;
                        break;
                    case 4:
                        addpoint += 0.015f;
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
