using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcstasyManager : MonoBehaviour
{
    public float ecstacyGage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CalEcstasy(int pattern,int who)
    {
        float addpoint = 0;
        switch (who)
        {
            case 0://‚¨‹ó‚¿‚á‚ñ
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
                        break;
                }
                break;

            case 1://‚¨‚è‚ñ‚¿‚á‚ñ
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

            case 2://‚³‚Æ‚è‚¿‚á‚ñ
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
        AddEcstacy(addpoint);
    }
    public void AddEcstacy(float delta) 
    {
        ecstacyGage = Mathf.Clamp(ecstacyGage + delta, 0, 100);
        //Debug.Log($"Ë¸ƒQ[ƒW{ecstacyGage}");
        if(ecstacyGage >= 100)//Ë¸‚·‚é‚©”»’è
        {
            Ecstasy();
        }
    }
    private void Ecstasy()//Ë¸‚Ì‰‰o
    {
        Debug.Log("Ë¸!");
    }
}
