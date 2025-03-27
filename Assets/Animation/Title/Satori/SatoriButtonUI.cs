using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SatoriButtonUI : MonoBehaviour
{
    private Animator animator;
    private EventTrigger eventTrigger;
    private SpriteRenderer spriteRenderer; // スプライトを非表示にする用

    [SerializeField] private OkuuButtonUI okuuButtonUI;
    [SerializeField] private OrinButtonUI orinButtonUI;
    void Start()
    {
        animator = GetComponent<Animator>();
        eventTrigger = GetComponent<EventTrigger>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // ボタンを有効にする
    public void EnableButton()
    {
        if (eventTrigger != null) eventTrigger.enabled = true;
        if (spriteRenderer != null) spriteRenderer.enabled = true; // スプライトを表示
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
            CloseButton();

            Invoke("Action", 1.5f);
        }
    }

    //リトライボタンの中身
    private void Action()
    {
        DifficultyManager.Instance.StartGame("Main", 2);
    }

    private void CloseButton()
    {
        okuuButtonUI.OnMove();
        orinButtonUI.OnMove();
    }

    public void OnMove()
    {
        animator.SetTrigger("isMoved");
    }
}
