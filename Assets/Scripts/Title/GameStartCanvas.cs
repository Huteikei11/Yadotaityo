using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartCanvas : MonoBehaviour
{

    public void Oku()
    {
        //ステージの値を渡す必要あり

        //場面遷移
        StartCoroutine(LoadSceneAsync("LoadScene"));
        //SceneManager.LoadScene("Main");
    }
    public void Orin()
    {
        //実行可能か判定する処理

        //ステージの値を渡す必要あり

        //場面遷移
        StartCoroutine(LoadSceneAsync("LoadScene"));
        //SceneManager.LoadScene("Main");
    }

    public void Satori()
    {
        //実行可能か判定する処理

        //ステージの値を渡す必要あり

        //場面遷移
        StartCoroutine(LoadSceneAsync("LoadScene")); 
        //SceneManager.LoadScene("Main");
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
