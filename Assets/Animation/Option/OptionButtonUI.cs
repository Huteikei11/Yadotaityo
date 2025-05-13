using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OptionButtonUI : MonoBehaviour
{
    private Animator animator;
    private EventTrigger eventTrigger;
    private SpriteRenderer spriteRenderer; // スプライトを非表示にする用

    void Start()
    {
        animator = GetComponent<Animator>();
        eventTrigger = GetComponent<EventTrigger>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        EnableButton();
    }

    // ボタンを有効にする
    public void EnableButton()
    {
        if (eventTrigger != null) eventTrigger.enabled = false;//本来はtrue　応急処置
        if (spriteRenderer != null) spriteRenderer.enabled = false; // スプライトを表示
        animator.SetTrigger("Open");
    }

    // ボタンを無効にする（クリックできなくし、見えなくする）
    public void DisableButton()
    {
        if (eventTrigger != null) eventTrigger.enabled = false;
        if (spriteRenderer != null) spriteRenderer.enabled = false; // スプライトを非表示
    }

    // マウスが触れたとき
    public void OnPointerEnter(BaseEventData eventData)
    {
        if (eventTrigger.enabled)
        {
            animator.SetBool("isHovered", true);
        }
    }

    // マウスが離れたとき
    public void OnPointerExit(BaseEventData eventData)
    {
        if (eventTrigger.enabled)
        {
            animator.SetBool("isHovered", false);
        }
    }

    // クリックしたとき
    public void OnPointerClick(BaseEventData eventData)
    {
        if (eventTrigger.enabled)
        {
            animator.SetTrigger("isClicked");
            Invoke("Action", 1.5f);
        }
    }

    //リトライボタンの中身
    private void Action()
    {

    }

    public void OnMove()
    {
        animator.SetTrigger("isMoved");
        Invoke("DisableButton", 0.5f);
    }
}
