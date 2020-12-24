using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance = null;
    public bool DontNext = false;
    
    private int CurrentScene;
    private int CountScenes;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init()
    {
        CountScenes = SceneManager.sceneCountInBuildSettings;
        CurrentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void NextScene()
    {
        if (DontNext)
        {
            RestartGame();
            return;
        }

        CurrentScene++;
        if (CurrentScene > CountScenes-1)
        {
            CurrentScene = 0;
        }
        
        PlayerPrefs.SetInt("CurrentScene", CurrentScene);
        DOTween.KillAll();

        SceneManager.LoadScene(CurrentScene);
    }

    private int GetRandomScene()
    {
        return UnityEngine.Random.Range(1, CountScenes);
    }

    public void RestartGame()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(CurrentScene);
    }
}
