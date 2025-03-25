using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimationController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // �}�E�X���{�^���ɏ�����Ƃ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("isHover", true);
    }

    // �}�E�X���{�^�����痣�ꂽ�Ƃ�
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("isHover", false);
    }

    // �{�^�����������Ƃ�
    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetBool("isPressed", true);
    }

    // �{�^���𗣂����Ƃ�
    public void OnPointerUp(PointerEventData eventData)
    {
        animator.SetBool("isPressed", false);
    }
}
