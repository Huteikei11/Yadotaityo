using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image whiteScreen; // �z���C�g�A�E�g�E�z���C�g�C���p�̃C���[�W
    [SerializeField] private SpriteRenderer KoishiCutIn; // �������̃J�b�g�C��
    [SerializeField] private Animator cutInAnimator; // �J�b�g�C���A�j���[�V�����p�̃A�j���[�^�[
    [SerializeField] private Animator mune; // ���̃A�j���[�^�[
    public List<RectTransform> uiScreens; // UI �� RectTransform
    public List<Transform> sprites; // �X�v���C�g�� Transform
    [SerializeField] private TextMeshProUGUI clearTimeText; // �N���A�^�C����\������e�L�X�g
    [SerializeField] private List<GameObject> hidegameObjects; // �Q�[�����̃I�u�W�F�N�g���X�g

    [SerializeField] private TimerController timerController;
    [SerializeField] private HighScoreManager highScoreManager;

    [SerializeField] private RetryButton RetryButton;
    [SerializeField] private ExitButtonUI exitButtonUI;

    [SerializeField] private Sprite[] semensprites; // �X�v���C�g��0,1,2�̏��ɓ���Ă���
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private TextCut textCut;
    [SerializeField] private Animator loadAnim;
    public float startTime = 2;//�Q�[���J�n�܂ł̎���


    private int difficulty;
    private int loadNo;

    void Start()
    {
        //�^�C�}�[�X�^�[�g

        loadNo = loadManager.Instance != null ? loadManager.Instance.GetLoadNo() : 0;
        switch (loadNo)
        {
            case 0:
                loadAnim.SetTrigger("loadOutTrigger");
                break;
            case 1:
                loadAnim.SetTrigger("loadGameoverTrigger");
                break;
        }
        difficulty = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
        spriteRenderer.enabled = false; // �ŏ��͔�\��
        StartCoroutine(GameStart());
    }
    private IEnumerator GameStart()
    {
        //�������̃X�^�[�g�̃g�����W�V�����ƕ���
        yield return new WaitForSeconds(startTime);
        textCut.CutScene(textCut.Start, false);
        yield return new WaitForSeconds(1.5f);
        //�����J��
        mune.SetTrigger("Open");

        timerController.StartTimer(); //�Q�[���J�n�܂ł̏���
        mune.gameObject.GetComponent<OppaiManager>().StartOppai();
    }

    public void FinishGame()
    {
        timerController.StopTimerAndSave(difficulty);
        highScoreManager.UpdateScoreDisplay(difficulty);
        StartCoroutine(ResultSequence());
    }


    private IEnumerator ResultSequence()
    {
        // 1. �z���C�g�A�E�g�ƃJ�b�g�C�����o
        yield return StartCoroutine(FinishDirection());

        // 2. ��ʃX�N���[�����o
        yield return StartCoroutine(Scroll());

        // 3. �Q�[����ʂ̃I�u�W�F�N�g���\���ɂ���
        HideGameObjects();

        //�{�^����\��
        RetryButton.EnableButton();
        RetryButton.clearFlags();
        exitButtonUI.EnableButton();

        // 4. �N���A�^�C���\��
        yield return StartCoroutine(ClearTime());
    }

    private IEnumerator FinishDirection()
    {
        //yield return new WaitForSeconds(2f); // �A�j���[�V�����̑ҋ@����


        // �z���C�g�A�E�g�J�n
        whiteScreen.gameObject.SetActive(true);
        for (float t = 0; t < 1f; t += Time.deltaTime * 3)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }
        mune.SetBool("Finish", true);//�����~�߂�

        // �z���C�g�C���J�n�i��ʂ𔒂���t�F�[�h�A�E�g�j
        for (float t = 1f; t > 0; t -= Time.deltaTime * 2)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }
        // �J�b�g�C���A�j���[�V�������Đ�
        cutInAnimator.SetTrigger("Show");
        yield return new WaitForSeconds(2f); // �A�j���[�V�����̑ҋ@����


        for (float t = 0; t < 1f; t += Time.deltaTime * 3)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }

        ShowSprite(difficulty);//���t���������Ă���
        cutInAnimator.SetTrigger("End");
        // �z���C�g�C���J�n�i��ʂ𔒂���t�F�[�h�A�E�g�j
        for (float t = 1f; t > 0; t -= Time.deltaTime * 2)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            //KoishiCutIn.color = whiteScreen.color;
            yield return null;
        }



        whiteScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f); // �A�j���[�V�����̑ҋ@����
    }

    private void HideGameObjects()
    {
        foreach (GameObject obj in hidegameObjects)
        {
            obj.SetActive(false); // �Q�[���I�u�W�F�N�g���\��
        }
    }

    private IEnumerator Scroll()
    {
        float slideDistanceUI = 550f; // UI�̃X���C�h�����ianchoredPosition �p�j
        float slideDistanceSprite = 1.61f; // �X�v���C�g�̃X���C�h�����i���[���h���W�j
        float duration = 1.5f; // �X���C�h����
        float time = 0;
        List<Vector2> startUIPositions = new List<Vector2>();
        List<Vector2> endUIPositions = new List<Vector2>();
        List<Vector3> startSpritePositions = new List<Vector3>();
        List<Vector3> endSpritePositions = new List<Vector3>();

        // UI �̊J�n�ʒu�ƏI���ʒu��ݒ�
        foreach (var screen in uiScreens)
        {
            startUIPositions.Add(screen.anchoredPosition);
            endUIPositions.Add(screen.anchoredPosition + (Vector2.left * slideDistanceUI));
        }

        // �X�v���C�g�̊J�n�ʒu�ƏI���ʒu��ݒ�
        foreach (var sprite in sprites)
        {
            startSpritePositions.Add(sprite.position);
            endSpritePositions.Add(sprite.position + Vector3.left * slideDistanceSprite);
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // UI ���X���C�h
            for (int i = 0; i < uiScreens.Count; i++)
            {
                uiScreens[i].anchoredPosition = Vector2.Lerp(startUIPositions[i], endUIPositions[i], t);
            }

            // �X�v���C�g���X���C�h
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].position = Vector3.Lerp(startSpritePositions[i], endSpritePositions[i], t);
            }

            yield return null;
        }

        // �ŏI�ʒu��␳
        for (int i = 0; i < uiScreens.Count; i++)
        {
            uiScreens[i].anchoredPosition = endUIPositions[i];
        }

        for (int i = 0; i < sprites.Count; i++)
        {
            sprites[i].position = endSpritePositions[i];
        }
    }

    private IEnumerator ClearTime()
    {
        clearTimeText.text = ""; // ������
        string timeStr = timerController.GetTimeString();

        // 1�������\������
        foreach (char c in timeStr)
        {
            clearTimeText.text += c;
            yield return new WaitForSeconds(0.1f); // �����̕\���Ԋu
        }
    }

    public void ShowSprite(int mode)
    {
        if (mode >= 0 && mode < semensprites.Length)
        {
            spriteRenderer.sprite = semensprites[mode];
            spriteRenderer.enabled = true; // �\��
        }
        else
        {
            Debug.LogWarning("�s���ȃ��[�h�ԍ��ł�: " + mode);
        }
    }
}
