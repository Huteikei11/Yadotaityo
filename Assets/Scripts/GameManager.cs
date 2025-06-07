using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image whiteScreen; // 繝帙Ρ繧､繝医い繧ｦ繝医・繝帙Ρ繧､繝医う繝ｳ逕ｨ縺ｮ繧､繝｡繝ｼ繧ｸ
    [SerializeField] private SpriteRenderer KoishiCutIn; // 縺薙＞縺励・繧ｫ繝・ヨ繧､繝ｳ
    [SerializeField] private Animator cutInAnimator; // 繧ｫ繝・ヨ繧､繝ｳ繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ逕ｨ縺ｮ繧｢繝九Γ繝ｼ繧ｿ繝ｼ
    [SerializeField] private Animator mune; // 閭ｸ縺ｮ繧｢繝九Γ繝ｼ繧ｿ繝ｼ
    public List<RectTransform> uiScreens; // UI 縺ｮ RectTransform
    public List<Transform> sprites; // 繧ｹ繝励Λ繧､繝医・ Transform
    [SerializeField] private TextMeshProUGUI clearTimeText; // 繧ｯ繝ｪ繧｢繧ｿ繧､繝繧定｡ｨ遉ｺ縺吶ｋ繝・く繧ｹ繝・
    [SerializeField] private List<GameObject> hidegameObjects; // 繧ｲ繝ｼ繝蜀・・繧ｪ繝悶ず繧ｧ繧ｯ繝医Μ繧ｹ繝・

    [SerializeField] private TimerController timerController;
    [SerializeField] private HighScoreManager highScoreManager;

    [SerializeField] private RetryButton RetryButton;
    [SerializeField] private ExitButtonUI exitButtonUI;

    [SerializeField] private Sprite[] semensprites; // 繧ｹ繝励Λ繧､繝医ｒ0,1,2縺ｮ鬆・↓蜈･繧後※縺翫￥
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private TextCut textCut;
    [SerializeField] private Animator loadAnim;

    [SerializeField] private OppaiManager oppaiManager; // 閭ｸ縺ｮ繧｢繝九Γ繝ｼ繧ｿ繝ｼ

    public float startTime = 2;//繧ｲ繝ｼ繝髢句ｧ九∪縺ｧ縺ｮ譎る俣


    private int difficulty;
    private int loadNo;

    void Start()
    {
        // 繧ｳ繝ｫ繝ｼ繝√Φ繧貞他縺ｳ蜃ｺ縺励※1繝輔Ξ繝ｼ繝驕・ｻｶ縺輔○繧・
        StartCoroutine(DelayedLoadAnimation());

        //繧ｿ繧､繝槭・繧ｹ繧ｿ繝ｼ繝・
        difficulty = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
        spriteRenderer.enabled = false; // 譛蛻昴・髱櫁｡ｨ遉ｺ
        StartCoroutine(GameStart());
    }

    private IEnumerator DelayedLoadAnimation()
    {
        // 1繝輔Ξ繝ｼ繝蠕・ｩ・
        yield return null;

        // loadNo 縺ｮ蛟､縺ｫ蝓ｺ縺･縺・※繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ繧定ｨｭ螳・
        loadNo = loadManager.Instance != null ? loadManager.Instance.GetLoadNo() : 0;
        switch (loadNo)
        {
            case 0:
                loadAnim.SetTrigger("loadOutTrigger");
                break;
            case 1:
                loadAnim.SetTrigger("loadGameoverTrigger");
                break;
        }
    }
    private IEnumerator GameStart()
    {
        //縺薙＞縺励・繧ｹ繧ｿ繝ｼ繝医・繝医Λ繝ｳ繧ｸ繧ｷ繝ｧ繝ｳ縺ｨ譁・ｭ・
        yield return new WaitForSeconds(startTime);
        textCut.CutScene(textCut.Start, false);
        yield return new WaitForSeconds(1.5f);
        //閭ｸ繧帝幕縺・
        mune.SetTrigger("Open");

        yield return new WaitForSeconds(1.5f);
        timerController.StartTimer(); //繧ｲ繝ｼ繝髢句ｧ九∪縺ｧ縺ｮ蜃ｦ逅・
        oppaiManager.StartOppai();
    }

    public void FinishGame()
    {
        timerController.StopTimerAndSave(difficulty);
        highScoreManager.UpdateScoreDisplay(difficulty);
        StartCoroutine(ResultSequence());
    }


    private IEnumerator ResultSequence()
    {
        // 1. 繝帙Ρ繧､繝医い繧ｦ繝医→繧ｫ繝・ヨ繧､繝ｳ貍泌・
        yield return StartCoroutine(FinishDirection());

        // 2. 逕ｻ髱｢繧ｹ繧ｯ繝ｭ繝ｼ繝ｫ貍泌・
        yield return StartCoroutine(Scroll());

        // 3. 繧ｲ繝ｼ繝逕ｻ髱｢縺ｮ繧ｪ繝悶ず繧ｧ繧ｯ繝医ｒ髱櫁｡ｨ遉ｺ縺ｫ縺吶ｋ
        HideGameObjects();

        //繝懊ち繝ｳ繧定｡ｨ遉ｺ
        RetryButton.EnableButton();
        RetryButton.clearFlags();
        exitButtonUI.EnableButton();

        // 4. 繧ｯ繝ｪ繧｢繧ｿ繧､繝陦ｨ遉ｺ
        yield return StartCoroutine(ClearTime());
    }

    private IEnumerator FinishDirection()
    {
        //yield return new WaitForSeconds(2f); // 繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ縺ｮ蠕・ｩ滓凾髢・


        // 繝帙Ρ繧､繝医い繧ｦ繝磯幕蟋・
        whiteScreen.gameObject.SetActive(true);
        for (float t = 0; t < 1f; t += Time.deltaTime * 3)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }
        mune.SetBool("Finish", true);//閭ｸ繧呈ｭ｢繧√ｋ

        // 繝帙Ρ繧､繝医う繝ｳ髢句ｧ具ｼ育判髱｢繧堤區縺九ｉ繝輔ぉ繝ｼ繝峨い繧ｦ繝茨ｼ・
        for (float t = 1f; t > 0; t -= Time.deltaTime * 2)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }

        // カットインアニメーションを再生
        cutInAnimator.SetTrigger("Show");
        yield return new WaitForSeconds(2f); // 繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ縺ｮ蠕・ｩ滓凾髢・


        for (float t = 0; t < 1f; t += Time.deltaTime * 3)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            yield return null;
        }

        ShowSprite(difficulty);//邊ｾ豸ｲ縺後°縺九▲縺ｦ縺・ｋ
        cutInAnimator.SetTrigger("End");
        // 繝帙Ρ繧､繝医う繝ｳ髢句ｧ具ｼ育判髱｢繧堤區縺九ｉ繝輔ぉ繝ｼ繝峨い繧ｦ繝茨ｼ・
        for (float t = 1f; t > 0; t -= Time.deltaTime * 2)
        {
            whiteScreen.color = new Color(1, 1, 1, t);
            //KoishiCutIn.color = whiteScreen.color;
            yield return null;
        }



        whiteScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f); // 繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ縺ｮ蠕・ｩ滓凾髢・
    }

    private void HideGameObjects()
    {
        foreach (GameObject obj in hidegameObjects)
        {
            obj.SetActive(false); // 繧ｲ繝ｼ繝繧ｪ繝悶ず繧ｧ繧ｯ繝医ｒ髱櫁｡ｨ遉ｺ
        }
    }

    private IEnumerator Scroll()
    {
        float slideDistanceUI = 550f; // UI縺ｮ繧ｹ繝ｩ繧､繝芽ｷ晞屬・・nchoredPosition 逕ｨ・・
        float slideDistanceSprite = 1.61f; // 繧ｹ繝励Λ繧､繝医・繧ｹ繝ｩ繧､繝芽ｷ晞屬・医Ρ繝ｼ繝ｫ繝牙ｺｧ讓呻ｼ・
        float duration = 1.5f; // 繧ｹ繝ｩ繧､繝画凾髢・
        float time = 0;
        List<Vector2> startUIPositions = new List<Vector2>();
        List<Vector2> endUIPositions = new List<Vector2>();
        List<Vector3> startSpritePositions = new List<Vector3>();
        List<Vector3> endSpritePositions = new List<Vector3>();

        // UI 縺ｮ髢句ｧ倶ｽ咲ｽｮ縺ｨ邨ゆｺ・ｽ咲ｽｮ繧定ｨｭ螳・
        foreach (var screen in uiScreens)
        {
            startUIPositions.Add(screen.anchoredPosition);
            endUIPositions.Add(screen.anchoredPosition + (Vector2.left * slideDistanceUI));
        }

        // 繧ｹ繝励Λ繧､繝医・髢句ｧ倶ｽ咲ｽｮ縺ｨ邨ゆｺ・ｽ咲ｽｮ繧定ｨｭ螳・
        foreach (var sprite in sprites)
        {
            startSpritePositions.Add(sprite.position);
            endSpritePositions.Add(sprite.position + Vector3.left * slideDistanceSprite);
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // UI 繧偵せ繝ｩ繧､繝・
            for (int i = 0; i < uiScreens.Count; i++)
            {
                uiScreens[i].anchoredPosition = Vector2.Lerp(startUIPositions[i], endUIPositions[i], t);
            }

            // 繧ｹ繝励Λ繧､繝医ｒ繧ｹ繝ｩ繧､繝・
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].position = Vector3.Lerp(startSpritePositions[i], endSpritePositions[i], t);
            }

            yield return null;
        }

        // 譛邨ゆｽ咲ｽｮ繧定｣懈ｭ｣
        for (int i = 0; i < uiScreens.Count; i++)
        {
            uiScreens[i].anchoredPosition = endUIPositions[i];
        }

        for (int i = 0; i < sprites.Count; i++)
        {
            sprites[i].position = endSpritePositions[i];
        }
    }

    private IEnumerator ClearTime()
    {
        clearTimeText.text = ""; // 蛻晄悄蛹・
        string timeStr = timerController.GetTimeString();

        // 1譁・ｭ励★縺､陦ｨ遉ｺ縺吶ｋ
        foreach (char c in timeStr)
        {
            clearTimeText.text += c;
            yield return new WaitForSeconds(0.1f); // 譁・ｭ励・陦ｨ遉ｺ髢馴囈
        }
    }

    public void ShowSprite(int mode)
    {
        if (mode >= 0 && mode < semensprites.Length)
        {
            spriteRenderer.sprite = semensprites[mode];
            spriteRenderer.enabled = true; // 陦ｨ遉ｺ
        }
        else
        {
            Debug.LogWarning("荳肴ｭ｣縺ｪ繝｢繝ｼ繝臥分蜿ｷ縺ｧ縺・ " + mode);
        }
    }
}
