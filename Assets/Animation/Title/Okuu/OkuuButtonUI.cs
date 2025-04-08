using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OkuuButtonUI : MonoBehaviour
{
    private Animator animator;
    private EventTrigger eventTrigger;
    private SpriteRenderer spriteRenderer; // �X�v���C�g���\���ɂ���p

    [SerializeField] private OrinButtonUI orinButtonUI;
    [SerializeField] private SatoriButtonUI satoriButtonUI;
    void Start()
    {
        animator = GetComponent<Animator>();
        eventTrigger = GetComponent<EventTrigger>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        eventTrigger.enabled = false;
        spriteRenderer.enabled = false;
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

            Invoke("Action", 1.5f);
        }
    }

    //���g���C�{�^���̒��g
    private void Action()
    {
        DifficultyManager.Instance.StartGame("Main", 0);
    }

    private void CloseButton()
    {
        orinButtonUI.OnMove();
        satoriButtonUI.OnMove();
    }

    public void OnMove()
    {
        animator.SetTrigger("isMoved");
    }
}
