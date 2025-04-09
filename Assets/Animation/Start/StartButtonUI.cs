using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartButtonUI : MonoBehaviour
{
    private Animator animator;
    private EventTrigger eventTrigger;
    private SpriteRenderer spriteRenderer; // スプライトを非表示にする用

    [SerializeField] private OkuuButtonUI okuuButtonUI;
    [SerializeField] private OrinButtonUI orinButtonUI;
    [SerializeField] private SatoriButtonUI satoriButtonUI;
    [SerializeField] private ExitButtonUITitle exitButtonUItitle;

    [SerializeField] private ScoreButtonUI scoreButtonUI;
    [SerializeField] private OptionButtonUI optionButtonUI;
    [SerializeField] private QuitButtonUI quitButtonUI;
    [SerializeField] private Animator Koishi;
    [SerializeField] private Animator Titlelogo;

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
        if (eventTrigger != null) eventTrigger.enabled = true;
        if (spriteRenderer != null) spriteRenderer.enabled = true; // スプライトを表示
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
            Koishi.SetTrigger("Open");
            Titlelogo.SetTrigger("Move");
            animator.SetTrigger("isClicked");
            CloseButton();

            Invoke("Action", 1f);
        }
    }

    //ボタンの中身
    private void Action()
    {
        okuuButtonUI.EnableButton();
        orinButtonUI.EnableButton();
        satoriButtonUI.EnableButton();
        exitButtonUItitle.EnableButton();
        DisableButton();
    }

    private void CloseButton()
    {
        scoreButtonUI.OnMove();
        optionButtonUI.OnMove();
        quitButtonUI.OnMove();
    }

    public void OnMove()
    {
        animator.SetTrigger("isMoved");
        Invoke("DisableButton", 0.5f);
    }
}
