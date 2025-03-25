using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimationController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // マウスがボタンに乗ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("isHover", true);
    }

    // マウスがボタンから離れたとき
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("isHover", false);
    }

    // ボタンを押したとき
    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetBool("isPressed", true);
    }

    // ボタンを離したとき
    public void OnPointerUp(PointerEventData eventData)
    {
        animator.SetBool("isPressed", false);
    }
}
