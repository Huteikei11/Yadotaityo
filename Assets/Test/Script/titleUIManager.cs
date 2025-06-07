using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleUIManager : MonoBehaviour
{
    [SerializeField] private Animator screen;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject score;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject koishi;

    [SerializeField] private ButtonMove UI_start;
    [SerializeField] private ButtonMove UI_score;
    [SerializeField] private ButtonMove UI_option;
    [SerializeField] private ButtonMove UI_tutorial;
    [SerializeField] private ButtonMove UI_quit;
    [SerializeField] private ButtonMove UI_utsuho;
    [SerializeField] private ButtonMove UI_rin;
    [SerializeField] private ButtonMove UI_satori;
    [SerializeField] private ButtonMove UI_back;
    [SerializeField] private tutorialPicture tutorialpicture;


    public float UIopenTime = 0.1f;
    public int page = 1;
    public bool tutorialPage = false;
    public bool scoreBoardPage = false;
    private IEnumerator caution()
    {
        screen.SetTrigger("cautionTrigger");
        yield return null;
        yield return new WaitForSeconds(screen.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(UIpage1open());
    }
    private IEnumerator loadIn(int difficult)
    {

        screen.SetTrigger("loadInTrigger");
        yield return null;//animatorが反映されるまでの緩衝時間
        yield return new WaitForSeconds(screen.GetCurrentAnimatorStateInfo(0).length);
        DifficultyManager.Instance.StartGame("Main", difficult);
    }
    private IEnumerator loadOut()
    {
        screen.SetTrigger("loadOutTrigger");
        yield return null;
        yield return new WaitForSeconds(screen.GetCurrentAnimatorStateInfo(0).length);
    }
    void Start()
    {
        StartCoroutine(caution());
    }
    void Update()
    {
        tutorialmode();
        scoreBoardmode();
    }
    void FixedUpdate()
    {
        if (page == 1)
        {
            if (UI_start.clickBool)
            {
                StartCoroutine(UIpage2open());
                StartCoroutine(UIpage1close());
                UI_start.clickBool = false;
            }
            if (UI_score.clickBool)
            {
                scoreBoardPage = true;
                screen.SetBool("scoreBoardBool",true);
                StartCoroutine(UIpage1close());
                UI_score.clickBool = false;
            }
            if (UI_tutorial.clickBool)
            {
                tutorialPage = true;
                tutorialpicture.pageReset();
                screen.SetBool("tutorialBool", true);
                StartCoroutine(UIpage1close());
                UI_tutorial.clickBool = false;
            }
            if (UI_quit.clickBool)
            {
                StartCoroutine(UIpage1close());
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
                #else
                    Application.Quit();//ゲームプレイ終了
                #endif
            }
        }else if (page == 2)
        {
            if (UI_utsuho.clickBool)
            {
                StartCoroutine(UIpage2close());
                UI_utsuho.clickBool = false;
                StartCoroutine(loadIn(0));
            }
            if (UI_rin.clickBool)
            {
                StartCoroutine(UIpage2close());
                UI_rin.clickBool = false;
                StartCoroutine(loadIn(1));
            }
            if (UI_satori.clickBool)
            {
                StartCoroutine(UIpage2close());
                UI_satori.clickBool = false;
                StartCoroutine(loadIn(2));
            }
            if (UI_back.clickBool)
            {
                StartCoroutine(UIpage2close());
                UI_back.clickBool = false;
                StartCoroutine(UIpage1open());
            }
        }
    }
    private IEnumerator UIpage1open()
    {
        page = 1;
        screen.SetInteger("UIpage",page);
        UI_start.ButtonOpen();
        yield return new WaitForSeconds(UIopenTime);
        UI_score.ButtonOpen();
        yield return new WaitForSeconds(UIopenTime);
        UI_tutorial.ButtonOpen();
        yield return new WaitForSeconds(UIopenTime);
        UI_quit.ButtonOpen();
    }
    private IEnumerator UIpage2open()
    {
        page = 2;
        screen.SetInteger("UIpage", page);
        UI_utsuho.ButtonOpen();
        yield return new WaitForSeconds(UIopenTime);
        UI_rin.ButtonOpen();
        yield return new WaitForSeconds(UIopenTime);
        UI_satori.ButtonOpen();
        yield return new WaitForSeconds(UIopenTime);
        UI_back.ButtonOpen();
    }
    private IEnumerator UIpage1close()
    {
        UI_start.ButtonClose();
        yield return new WaitForSeconds(UIopenTime);
        UI_score.ButtonClose();
        yield return new WaitForSeconds(UIopenTime);
        UI_tutorial.ButtonClose();
        yield return new WaitForSeconds(UIopenTime);
        UI_quit.ButtonClose();
    }
    private IEnumerator UIpage2close()
    {
        UI_utsuho.ButtonClose();
        yield return new WaitForSeconds(UIopenTime);
        UI_rin.ButtonClose();
        yield return new WaitForSeconds(UIopenTime);
        UI_satori.ButtonClose();
        yield return new WaitForSeconds(UIopenTime);
        UI_back.ButtonClose();
    }
    public void tutorialmode()
    {
        if (tutorialPage == true)
        {
            if((tutorialPage = tutorialpicture.pageTurn()) == false)
            {
                screen.SetBool("tutorialBool", false);
                StartCoroutine(UIpage1open());
            }
        }
        
    }
    public void scoreBoardmode()
    {
        if (scoreBoardPage == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                screen.SetBool("scoreBoardBool", false);
                scoreBoardPage = false;
                StartCoroutine(UIpage1open());
            }
        }
    }
}
