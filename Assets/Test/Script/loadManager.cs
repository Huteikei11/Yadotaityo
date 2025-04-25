using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadManager : MonoBehaviour
{
    public static loadManager Instance { get; private set; }

    private int selectedLoad = 0; // デフォルトは Easy (0)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移しても破棄されない
        }
        else
        {
            Destroy(gameObject); // すでに存在する場合は削除
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
