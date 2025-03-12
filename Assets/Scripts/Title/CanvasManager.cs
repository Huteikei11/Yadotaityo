using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public int ActiveCanvas = 0;
    public Canvas top;
    public Canvas gamestart;
    public Canvas option;
    // Start is called before the first frame update
    void Start()
    {
        InActiveCanvas();
        Top_Active();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InActiveCanvas()
    {
        ActiveCanvas = 0;
        top.enabled = false;
        gamestart.enabled = false;
        option.enabled = false;
    }
    public void Top_Active()
    {
        ActiveCanvas = 1;
        top.enabled = !top.enabled;
        gamestart.enabled = false;
        option.enabled = false;
    }

    public void Gamestart_Active()
    {
        ActiveCanvas = 2;
        top.enabled = false; ;
        gamestart.enabled = !gamestart.enabled;
        option.enabled = false;
    }

    public void Option_Active()
    {
        ActiveCanvas = 3;
        top.enabled = false;
        gamestart.enabled = false;
        option.enabled = !option.enabled;
    }

    public void QuitGame()
    {
      #if UNITY_EDITOR
        // Unityエディターでの動作
        UnityEditor.EditorApplication.isPlaying = false;
      #else
        // 実際のゲーム終了処理
        Application.Quit();
      #endif
    }
}
