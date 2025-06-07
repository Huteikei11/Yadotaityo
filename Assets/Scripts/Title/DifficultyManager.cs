using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    private int selectedDifficulty = 0; // �f�t�H���g�� Easy (0)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[���J�ڂ��Ă��j������Ȃ�
        }
        else
        {
            Destroy(gameObject); // ���łɑ��݂���ꍇ�͍폜
        }
    }

    // ��Փx��ݒ�i0: Easy, 1: Normal, 2: Hard�j
    public void SetDifficulty(int difficulty)
    {
        selectedDifficulty = Mathf.Clamp(difficulty, 0, 2);
        Debug.Log("��Փx�ݒ�: " + selectedDifficulty);
    }

    // ���݂̓�Փx���擾
    //int difficulty = DifficultyManager.Instance.GetDifficulty();
    public int GetDifficulty()
    {
        return Instance != null ? Instance.selectedDifficulty : 0;
    }

    // ��Փx��ݒ肵�A�Q�[���V�[���ֈړ�
    public void StartGame(string sceneName, int difficulty)
    {
        SetDifficulty(difficulty);
        StartCoroutine(LoadSceneAsync(sceneName));

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
