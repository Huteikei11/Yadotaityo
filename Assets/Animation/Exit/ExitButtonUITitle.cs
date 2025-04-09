using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ExitButtonUITitle : MonoBehaviour
{
    private Animator animator;
    private EventTrigger eventTrigger;
    private SpriteRenderer spriteRenderer; // �X�v���C�g���\���ɂ���p

    [SerializeField] private StartButtonUI startButtonUI;
    [SerializeField] private ScoreButtonUI scoreButtonUI;
    [SerializeField] private OptionButtonUI optionButtonUI;
    [SerializeField] private QuitButtonUI quitButtonUI;
    [SerializeField] private Animator Koishi;
    [SerializeField] private Animator TitleLogo;

    [SerializeField] private OkuuButtonUI okuuButtonUI;
    [SerializeField] private OrinButtonUI orinButtonUI;
    [SerializeField] private SatoriButtonUI satoriButtonUI;

    void Start()
    {
        animator = GetComponent<Animator>();
        eventTrigger = GetComponent<EventTrigger>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        DisableButton();
    }

    // �{�^����L���ɂ���
    public void EnableButton()
    {
        if (eventTrigger != null) eventTrigger.enabled = true;
        if (animator != null) animator.SetTrigger("Open");
        if (spriteRenderer != null) spriteRenderer.enabled = true; // �X�v���C�g��\��
    }

    // �{�^���𖳌��ɂ���i�N���b�N�ł��Ȃ����A�����Ȃ�����j
    public void DisableButton()
    {
        if (eventTrigger != null) eventTrigger.enabled = false;
        if (spriteRenderer != null) spriteRenderer.enabled = false; // �X�v���C�g���\��
    }

    // �}�E�X���G�ꂽ�Ƃ�
    public void OnPointerEnter(BaseEventData eventData)
    {
        if (eventTrigger.enabled)
        {
            animator.SetBool("isHovered", true);
        }
    }

    // �}�E�X�����ꂽ�Ƃ�
    public void OnPointerExit(BaseEventData eventData)
    {
        if (eventTrigger.enabled)
        {
            animator.SetBool("isHovered", false);
        }
    }

    // �N���b�N�����Ƃ�
    public void OnPointerClick(BaseEventData eventData)
    {
        if (eventTrigger.enabled)
        {
            animator.SetTrigger("isClicked");
            CloseButton();

            Invoke("Action", 1f);
        }
    }

    //���g���C�{�^���̒��g
    private void Action()
    {
        startButtonUI.EnableButton();
        scoreButtonUI.EnableButton();
        optionButtonUI.EnableButton();
        quitButtonUI.EnableButton();
    }

    private void CloseButton()
    {
        Koishi.SetTrigger("Close");
        TitleLogo.SetTrigger("Open");
        okuuButtonUI.OnMove();
        orinButtonUI.OnMove();
        satoriButtonUI.OnMove();
    }
    public void OnMove()
    {
        animator.SetTrigger("isMoved");
        Invoke("DisableButton", 0.5f);
    }
}
