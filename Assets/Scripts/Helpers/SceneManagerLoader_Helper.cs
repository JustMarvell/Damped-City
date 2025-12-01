using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SceneManagerLoader_Helper : MonoBehaviour
{
    public static SceneManagerLoader_Helper instance;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnNewSceneLoaded;
        SceneManager.sceneUnloaded += OnCurrentSceneUnloaded;
    }

    public void OnNewSceneLoaded(Scene newScene, LoadSceneMode loadSceneMode)
    {
        
    }

    public void OnCurrentSceneUnloaded(Scene sceneName)
    {
        
    }

    public string mainMenuSceneName = "3D_Menu";
    public string levelOneSceneName = "Scene_A 1";
    public GameObject loadingScreenObject;
    public Slider progressBar;
    private float totalSceneProgress = 0;
    private float totalAllProgress = 0;

    public void SwitchToLevelOneScene()
    {
        Time.timeScale = 1;

        loadingScreenObject.SetActive(true);
        progressBar.value = 0;

        newScene = SceneManager.LoadSceneAsync(levelOneSceneName);

        StartCoroutine(GetSceneAndSpawnLoadProgress());
        StartCoroutine(GetTotalSpawnProgress());
    }

    public void SwitchToMenuScene()
    {
        Time.timeScale = 1;

        loadingScreenObject.SetActive(true);
        progressBar.value = 0;

        SwitchToSceneAsync(mainMenuSceneName);
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    AsyncOperation newScene;

    void SwitchToSceneAsync(string sceneName)
    {
        newScene = SceneManager.LoadSceneAsync(sceneName);

        StartCoroutine(GetSceneLoadProgress());
    }

    IEnumerator GetTotalSpawnProgress()
    {
        float totalSpawnProgress = 0;

        while ((EnemyManager.instance == null || !EnemyManager.instance.enemySpawned) && (CollectableManager.instance == null || !CollectableManager.instance.isSpawned))
        {
            if (EnemyManager.instance == null && CollectableManager.instance == null)
            {
                totalSpawnProgress = 0;
            }
            else
            {
                float enemySpawnProgress = EnemyManager.instance.spawnProgress;
                float collectableProgress = CollectableManager.instance.spawnProgress;

                float tPrgs = (enemySpawnProgress + collectableProgress) / 2f;
                totalSpawnProgress = tPrgs;
            }

            yield return null;
        }

        totalAllProgress = (totalSceneProgress + totalSpawnProgress) / 2f;
        progressBar.value = totalAllProgress;

        yield return new WaitForSeconds(.2f);
        loadingScreenObject.SetActive(false);
        CutsceneManager.instance.PlayStartingCutscene();
    }

    
    
    public IEnumerator GetSceneLoadProgress()
    {
        while (!newScene.isDone)
        {
            totalSceneProgress = 0;

            totalSceneProgress = newScene.progress;
            progressBar.value = totalSceneProgress;

            yield return null;
        }

        yield return new WaitForSeconds(.2f);
        loadingScreenObject.SetActive(false);
    }

    public IEnumerator GetSceneAndSpawnLoadProgress()
    {
        totalSceneProgress = 0;

        while (!newScene.isDone)
        {
            totalSceneProgress = newScene.progress;

            yield return null;
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else 
        Application.Quit();
#endif
    }
}
