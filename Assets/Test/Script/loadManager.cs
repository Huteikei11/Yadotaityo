using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadManager : MonoBehaviour
{
    public static loadManager Instance { get; private set; }

    private int selectedLoad = 0; // �f�t�H���g�� Easy (0)

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
    
    public void SetLoad(int loadNo)
    {
        selectedLoad = Mathf.Clamp(loadNo, 0, 2);
    }
    
    public int GetLoadNo()
    {
        return Instance != null ? Instance.selectedLoad : 0;
    }
    
    public void StartGame(string sceneName, int loadNo)
    {
        SetLoad(loadNo);
        SceneManager.LoadScene(sceneName);
    }
}
