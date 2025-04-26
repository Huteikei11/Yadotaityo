using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    private Animator animator;
    private EventTrigger eventTrigger;
    private SpriteRenderer spriteRenderer; // スプライトを非表示にする用

    [SerializeField] private ExitButtonUI exitButtonUI;
    [SerializeField] private Animator loadAnim; 

    private bool ClearFlags = false;

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

            Invoke("Action", 1.5f);
        }
    }

    //リトライボタンの中身
    private void Action()
    {
        StartCoroutine(GameLoad());
    }

    private IEnumerator GameLoad()
    {

        if (ClearFlags == false)
        {
            loadAnim.SetTrigger("loadGameoverTrigger");
            loadManager.Instance.StartGame("Main", 1);
        }
        else
        {
            loadAnim.SetTrigger("loadInTrigger");
            yield return new WaitForSeconds(0.01f);//animatorが反映されるまでの緩衝時間
           
            yield return new WaitForSeconds(loadAnim.GetCurrentAnimatorStateInfo(0).length);
            loadManager.Instance.StartGame("Main", 0);
        }
    }


    private void CloseButton()
    {
        exitButtonUI.OnMove();
    }
    public void OnMove()
    {
        animator.SetTrigger("isMoved");
        Invoke("DisableButton", 0.5f);
    }

    public void clearFlags()
    {
        ClearFlags = true;
    }
}
