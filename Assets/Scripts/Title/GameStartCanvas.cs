using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartCanvas : MonoBehaviour
{

    public void Oku()
    {
        //�X�e�[�W�̒l��n���K�v����

        //��ʑJ��
        StartCoroutine(LoadSceneAsync("LoadScene"));
        //SceneManager.LoadScene("Main");
    }
    public void Orin()
    {
        //���s�\�����肷�鏈��

        //�X�e�[�W�̒l��n���K�v����

        //��ʑJ��
        StartCoroutine(LoadSceneAsync("LoadScene"));
        //SceneManager.LoadScene("Main");
    }

    public void Satori()
    {
        //���s�\�����肷�鏈��

        //�X�e�[�W�̒l��n���K�v����

        //��ʑJ��
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
