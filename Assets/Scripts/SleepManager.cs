using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private TextCut textCut;

    // Inspector ����ҏW�\�Ȓl
    [Header("���� (okuu) �̐����x�����l")]
    public float[] okuuAddPoints = { 0.01f, 0.015f, 0.02f, 0.04f, 0.05f };

    [Header("����� (orin) �̐����x�����l")]
    public float[] orinAddPoints = { 0.01f, 0.02f, 0.03f, 0.05f, 0.07f };

    [Header("���Ƃ� (satori) �̐����x�����l")]
    public float[] satoriAddPoints = { 0.002f, 0.03f, 0.04f, 0.06f, 0.08f };

    [Header("PlusSleepDeepNotHolding �̐ݒ�")]
    [SerializeField] private float finalStageIncrease = 0.03f; // �ŏI�i�K�ȏ�ő�������l
    [SerializeField] private float halfStageIncrease = 0.02f;  // �����ȏ�ő�������l
    [SerializeField] private float belowHalfDecrease = -0.002f; // �����ȉ��Ō�������l

    [Header("WatchBool �̐ݒ�")]
    [SerializeField] private float wakeUpDuration = 3f; // �N�����܂܂ł��鎞��

    void Start()
    {
        // whiteScreen �̏�����������
        if (whiteScreen != null)
        {
            whiteScreen.color = new Color(134 / 255f, 0, 142 / 255f, 90 / 255f);
            Debug.Log($"whiteScreen initialized to: {whiteScreen.color}");
        }
        who = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
        anim.SetInteger("difficult", who);

        StartFace();
    }

    public void StartFace() // ��̒���X�V������
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

    IEnumerator FaceMethod() // ��̒���X�V���钆�g
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

    public void CalSleepDeepOppai(int pattern, int who) // �����x�������ς��ő���
    {
        float addpoint = 0;
        switch (who)
        {
            case 0: // ���� (okuu)
                if (pattern >= 0 && pattern < okuuAddPoints.Length)
                    addpoint = okuuAddPoints[pattern];
                break;

            case 1: // ����� (orin)
                if (pattern >= 0 && pattern < orinAddPoints.Length)
                    addpoint = orinAddPoints[pattern];
                break;

            case 2: // ���Ƃ� (satori)
                if (pattern >= 0 && pattern < satoriAddPoints.Length)
                    addpoint = satoriAddPoints[pattern];
                break;
        }
        AddSleepDeep(addpoint);
    }

    public void PlusSleepDeepFallObj(float value) // ���̂������Ă����Ƃ��ɐ����x�𑫂�
    {
        AddSleepDeep(value);
    }

    public void PlusSleepDeepNotHolding() // �����ς��ɐG��Ă��Ȃ��Ƃ�
    {
        float addpoint = 0;
        if (sleepDeep >= (wakeup / 1.1))
        {
            addpoint += finalStageIncrease; // �ŏI�i�K�ȏ�ł���ɑ���
        }
        else if (sleepDeep >= 50)
        {
            addpoint += halfStageIncrease; // �����Q�[�W�������ȏ�ő���
        }
        else
        {
            addpoint += belowHalfDecrease; // �����Q�[�W�������ȉ��Ō���
        }
        AddSleepDeep(addpoint);
    }

    public void WakeUpChara() // �L�����N�^�[���N���鎞�̏���
    {
        OzyamaFall.isAllow = false;
        Debug.Log("������");
        sleepDeep = 0;
        StartCoroutine(WatchBool());

    }

    IEnumerator WatchBool()
    {
        OzyamaFall.SetFallDuringBool(true);
        OzyamaFall.anim.SetBool("fallBool", OzyamaFall.GetFallDuringBool());
        anim.SetTrigger("up");
        yield return new WaitForSeconds(0.4f);

        Debug.Log("�Ď��J�n");

        float timer = 0f;
        bool becameTrue = false;
        while (timer < wakeUpDuration) // �N�����܂܂̎��Ԃ��r
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
            Debug.Log("1�b�ԁAtargetBool�͈�x��true�ɂȂ�܂���ł����I");
            OzyamaFall.isAllow = true;
            anim.SetTrigger("Sleep");
            OzyamaFall.SetFallDuringBool(false);
            OzyamaFall.anim.SetBool("fallBool", OzyamaFall.GetFallDuringBool());
        }
        else
        {
            Debug.Log("1�b�Ԃ̂�����targetBool��true�ɂȂ�܂����I");
            StartCoroutine(Failed());
        }
    }

    IEnumerator Failed()
    {
        OzyamaFall.isAllow = false;

        for (float t = 50/255; t >= 0; t -= Time.deltaTime/4)
        {
            whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, t);
            yield return null;
        }
        textCut.CutScene(textCut.Failed, true);
        yield return new WaitForSeconds(3f);
        oppaiManager.isTouch = false;
        gameoversprite.gameover();

        RetryButton.EnableButton();
        exitButtonUI.EnableButton();
    }

    private void AddSleepDeep(float value) // ���ڐ����x�𑫂��N���邩����
    {
        sleepDeep = Mathf.Clamp(sleepDeep + value, 0, wakeup);
        if (sleepDeep == wakeup)
        {
            WakeUpChara(); // ���͂悤�������܂�
        }
    }
}
