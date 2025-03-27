using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image whiteScreen; // �z���C�g�A�E�g�E�z���C�g�C���p�̃C���[�W
    [SerializeField] private Animator cutInAnimator; // �J�b�g�C���A�j���[�V�����p�̃A�j���[�^�[
    [SerializeField] private RectTransform gameScreen; // �Q�[����ʂ�UI�I�u�W�F�N�g
    [SerializeField] private RectTransform resultScreen; // ���U���g��ʂ�UI�I�u�W�F�N�g
    [SerializeField] private TextMeshProUGUI clearTimeText; // �N���A�^�C����\������e�L�X�g
    [SerializeField] private List<GameObject> gameObjects; // �Q�[�����̃I�u�W�F�N�g���X�g

    [SerializeField] private TimerController timerController;
    [SerializeField] private HighScoreManager highScoreManager;

    [SerializeField] private RetryButton RetryButton;
    [SerializeField] private ExitButtonUI exitButtonUI;

    void Start()
    {
        //�^�C�}�[�X�^�[�g
        timerController.StartTimer();
    }
    public void FinishGame()
    {
        timerController.StopTimerAndSave();
        highScoreManager.UpdateScoreDisplay();
        StartCoroutine(ResultSequence());
    }

    private IEnumerator ResultSequence()
    {
        // 1. �z���C�g�A�E�g�ƃJ�b�g�C�����o
        yield return StartCoroutine(FinishDirection());

        // 2. �Q�[����ʂ̃I�u�W�F�N�g���\���ɂ���
        HideGameObjects();

        // 3. ��ʃX�N���[�����o
        yield return StartCoroutine(Scroll());

        //�{�^����\��
        RetryButton.EnableButton();
        exitButtonUI.EnableButton();

        // 4. �N���A�^�C���\��
        yield return StartCoroutine(ClearTime());
    }

    private IEnumerator FinishDirection()
    {
        // �J�b�g�C���A�j���[�V�������Đ�
        cutInAnimator.SetTrigger("Show");

        // �z���C�g�A�E�g�J�n
        whiteScreen.gameObject.SetActive(true);
        for (float t = 0; t < 1f; t += Time.deltaTime*3)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }

        yield return new WaitForSeconds(1f); // �A�j���[�V�����̑ҋ@����

        // �z���C�g�C���J�n�i��ʂ𔒂���t�F�[�h�A�E�g�j
        for (float t = 1f; t > 0; t -= Time.deltaTime*2)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }
        whiteScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f); // �A�j���[�V�����̑ҋ@����
    }

    private void HideGameObjects()
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(false); // �Q�[���I�u�W�F�N�g���\��
        }
    }

    private IEnumerator Scroll()
    {
        float duration = 1.5f; // �X�N���[�����o�̏��v����
        float time = 0;
        Vector3 startGamePos = gameScreen.anchoredPosition;
        Vector3 startResultPos = resultScreen.anchoredPosition;
        Vector3 endGamePos = startGamePos + Vector3.left * 550; // �Q�[����ʂ����Ɉړ�
        Vector3 endResultPos = startResultPos + Vector3.left * 550; // ���U���g��ʂ����Ɉړ��i�E������o���j

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            gameScreen.anchoredPosition = Vector3.Lerp(startGamePos, endGamePos, t);
            resultScreen.anchoredPosition = Vector3.Lerp(startResultPos, endResultPos, t);
            yield return null;
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
}
