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
        who = oppaiManager.GetChara();//�L�������N�����擾
        StartFace();
    }

    public void StartFace()//��̒���X�V������
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

    IEnumerator FaceMethod() //��̒���X�V���钆�g
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

    public void CalSleepDeepOppai(int pattern,int who) //�����x�������ς��ő���
    {//�����ς��G���ĂȂ��Ƃ��͌Ă΂�Ȃ��͂��Ȃ̂ő��v
        float addpoint = 0;
        switch (who) 
        {
            case 0://���󂿂��
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

            case 1://����񂿂��
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

            case 2://���Ƃ肿���
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
    public void PlusSleepDeepFallObj(float value)//���̂������Ă����Ƃ��ɐ����x�𑫂�
    {
        AddSleepDeep(value);
    }

    public void PlusSleepDeepNotHolding()//�����ς��ɐG��Ă��Ȃ��Ƃ�
    {
        float addpoint = 0;
        if (sleepDeep >= 50)
        {
            addpoint += 0.005f;//�����Q�[�W�������ȏ�ő���
        }
        else
        {
            addpoint -= 0.0015f; //�����Q�[�W�������ȉ��Ō���
        }
        AddSleepDeep(addpoint);
    }

    public void WakeUpChara()//�L�����N�^�[���N���鎞�̏���
    {
        Debug.Log("������");
        sleepDeep = 0;
    }

    private void AddSleepDeep(float value)//���ڐ����x�𑫂��N���邩����
    {
        sleepDeep = Mathf.Clamp(sleepDeep + value, 0, wakeup);
        if (sleepDeep == wakeup)
        {
            WakeUpChara();//���͂悤�������܂�
        }
    }
}
