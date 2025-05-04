using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonMove : MonoBehaviour
{
    Animator animator;
    private EventTrigger eventTrigger;
    public bool clickBool;

    // Start is called before the first frame update
    void Awake()
    {
        animator = this.GetComponent<Animator>();
        eventTrigger = GetComponent<EventTrigger>();
    }

    public void ButtonOpen()
    {
        animator.SetTrigger("OpenTrigger");
        clickBool = false;
    }
    public void ButtonClose()
    {
        animator.SetTrigger("CloseTrigger");
    }
    public void OnPointerEnter(BaseEventData eventData)
    {
            animator.SetBool("isHovered", true);
    }

    // マウスが離れたとき
    public void OnPointerExit(BaseEventData eventData)
    {
            animator.SetBool("isHovered", false);
    }

    // クリックしたとき
    public void OnPointerClick(BaseEventData eventData)
    {
        animator.SetTrigger("ClickTrigger");
        clickBool = true;
    }
}
