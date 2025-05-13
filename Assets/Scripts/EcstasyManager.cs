using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcstasyManager : MonoBehaviour
{
    public float ecstacyGage;
    public float adjustEcstacyGage;
    [SerializeField] private GameManager gameManager;
    private bool isEcstasy = false;
    [SerializeField] private OzyamaFall OzyamaFall;
    public bool isAllowCal;

    // Inspector から編集可能な値
    [Header("お空 (okuu) のエクスタシー増加値")]
    public float[] okuuAddPoints = { 0.002f, 0.003f, 0.004f, 0.005f, 0.01f };

    [Header("おりん (orin) のエクスタシー増加値")]
    public float[] orinAddPoints = { 0.0025f, 0.0035f, 0.0045f, 0.006f, 0.011f };

    [Header("さとり (satori) のエクスタシー増加値")]
    public float[] satoriAddPoints = { 0.0025f, 0.035f, 0.005f, 0.007f, 0.015f };

    // Update is called once per frame
    void Update()
    {
    }

    public void CalEcstasy(int pattern, int who)
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

        if (isAllowCal)
        {
            AddEcstacy(addpoint);
        }
    }

    public void AddEcstacy(float delta)
    {
        if (!isEcstasy) // 射精していたら100のまま
        {
            // Time.deltaTime を掛けてフレームレートに依存しない増加量にする
            ecstacyGage = Mathf.Clamp(ecstacyGage + delta * Time.deltaTime * adjustEcstacyGage, 0, 100);
            if (ecstacyGage >= 100) // 射精するか判定
            {
                isEcstasy = true;
                Ecstasy();
            }
        }
    }

    private void Ecstasy() // 射精の演出
    {
        OzyamaFall.isAllow = false;
        Debug.Log("射精!");
        gameManager.FinishGame();
    }
}
