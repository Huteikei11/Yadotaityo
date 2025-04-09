using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ExitButtonUITitle : MonoBehaviour
{
    private Animator animator;
    private EventTrigger eventTrigger;
    private SpriteRenderer spriteRenderer; // スプライトを非表示にする用

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

    // ボタンを有効にする
    public void EnableButton()
    {
        if (eventTrigger != null) eventTrigger.enabled = true;
        if (animator != null) animator.SetTrigger("Open");
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

            Invoke("Action", 1f);
        }
    }

    //リトライボタンの中身
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
