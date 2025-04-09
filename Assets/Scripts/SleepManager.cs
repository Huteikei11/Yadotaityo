using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class SleepManager : MonoBehaviour
{
    public float sleepDeep;
    private int who;
    public float wakeup;
    public int facePattern;
    public OppaiManager oppaiManager;
    private Coroutine faceCoroutine;
    public Animator anim;
    public Animator oppai;
    public Animator gameover;

    [SerializeField] private Image whiteScreen; // �z���C�g�A�E�g�E�z���C�g�C���p�̃C���[�W
    [SerializeField] private RetryButton RetryButton;
    [SerializeField] private ExitButtonUI exitButtonUI;
    [SerializeField] private OzyamaFall OzyamaFall;
    [SerializeField] private GameoverSprite gameoversprite;
    // Start is called before the first frame update
    void Start()
    {
        who = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
        anim.SetInteger("difficult", who);
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
                anim.SetInteger("face", facePattern);
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
                        addpoint = 0.015f;
                        break;

                    case 2:
                        addpoint = 0.02f;
                        break;

                    case 3:
                        addpoint = 0.04f;
                        break;
                    case 4:
                        addpoint = 0.05f;
                        break;
                }
                break;

            case 1://����񂿂��
                switch (pattern)
                {
                    case 0:
                        addpoint = 0.01f;
                        break;
                    case 1:
                        addpoint = 0.02f;
                        break;

                    case 2:
                        addpoint = 0.03f;
                        break;

                    case 3:
                        addpoint = 0.05f;
                        break;
                    case 4:
                        addpoint = 0.07f;
                        break;
                }
                break;

            case 2://���Ƃ肿���
                switch (pattern)
                {
                    case 0:
                        addpoint = 0.002f;
                        break;
                    case 1:
                        addpoint = 0.03f;
                        break;

                    case 2:
                        addpoint = 0.04f;
                        break;

                    case 3:
                        addpoint = 0.06f;
                        break;
                    case 4:
                        addpoint = 0.08f;
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
        if (sleepDeep >= (wakeup / 1.1))
        {
            addpoint += 0.03f;//�����Q�[�W���ŏI�i�K�ȏ�ł���ɑ���
        }
        else if (sleepDeep >= 50)
        {
            addpoint += 0.02f;//�����Q�[�W�������ȏ�ő���
        }
        else
        {
            addpoint -= 0.002f; //�����Q�[�W�������ȉ��Ō���
        }
        AddSleepDeep(addpoint);
    }

    public void WakeUpChara()//�L�����N�^�[���N���鎞�̏���
    {
        OzyamaFall.isAllow = false;
        Debug.Log("������");
        sleepDeep = 0;
        StartCoroutine(WatchBool());

        
        
    }

    IEnumerator WatchBool()
    {
        anim.SetTrigger("up");
        // �܂��҂�
        yield return new WaitForSeconds(0.4f);

        Debug.Log("�Ď��J�n");

        float timer = 0f;
        bool becameTrue = false;
        while (timer < 3f)//�O�b�������܂�
        {
            if (oppaiManager.isHolding|| Input.GetMouseButton(0))
            {
                becameTrue = true;
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }


        if (!becameTrue)
        {
            Debug.Log("1�b�ԁAtargetBool�͈�x��true�ɂȂ�܂���ł����I");
            //�܂��Q��
            OzyamaFall.isAllow = true;
            anim.SetTrigger("Sleep");
        }
        else
        {
            Debug.Log("1�b�Ԃ̂�����targetBool��true�ɂȂ�܂����I");
            StartCoroutine(Failed());
        }


    }

    IEnumerator Failed()
    {
        //oppai.speed = 0f;
        //yield return new WaitForSeconds(4f);
        OzyamaFall.isAllow = false;//���}���L�X�����~�߂�

        //���邭����
        whiteScreen.color = new Color(1, 1, 1, 22/255f);
        yield return new WaitForSeconds(0.2f);
        whiteScreen.color = new Color(0, 0, 0, 100/255f);
        yield return new WaitForSeconds(1f);

        // �z���C�g�A�E�g�J�n
        for (float t = 0; t < 22/255f; t += Time.deltaTime/5)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }

        //�Q�[���I�[�o�[��ʕ\��
        yield return new WaitForSeconds(3f);
        oppaiManager.isTouch = false;
        gameoversprite.gameover();

        //�{�^����\��
        RetryButton.EnableButton();
        exitButtonUI.EnableButton();
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
