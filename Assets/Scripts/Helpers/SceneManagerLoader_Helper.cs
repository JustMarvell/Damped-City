using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

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

        LevelMusic_Helper.instance.StopMusic();

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

        if (SaveSystem.instance.HasSaveFile())
        {
            GameData data = SaveSystem.instance.LoadGame();
            if (data != null)
            {
                // Restore collected items
                if (Inventory.instance != null && GameMaster.instance != null)
                {
                    int current = Inventory.instance.CheckCollectedItemNumber(GameMaster.instance.itemToCollect);
                    int toAdd = data.collectedItemCount - current;
                    for (int i = 0; i < toAdd; i++)
                        Inventory.instance.Add(GameMaster.instance.itemToCollect);
                        
                    CollectableManager.instance.itemCollectUI.SetActive(false);
                }

                // Restore retry chances
                if (GameMaster.instance != null)
                    GameMaster.instance.currentRetryChange = data.currentRetryChance;

                // Restore player position
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.gameObject.GetComponent<StateMachine_3D>().enabled = false;
                    player.gameObject.GetComponent<CharacterController>().enabled = false;

                    player.transform.position = data.playerPosition;
                    player.transform.rotation = data.playerRotation;

                    player.gameObject.GetComponent<StateMachine_3D>().enabled = false;
                    player.gameObject.GetComponent<CharacterController>().enabled = false;
                }

                // enable enemy spawns
                if (EnemyManager.instance != null)
                {
                    EnemyManager.instance.ResetAndEnableEnemy();
                }

                // Restore activated/deleted objects
                foreach (string path in data.activatedObjectPaths)
                {
                    GameObject obj = FindObjectByHierarchyPath(path);
                    if (obj != null)
                    {
                        obj.SetActive(true);
                        Debug.Log("Activated: " + path);
                    }
                    else
                    {
                        Debug.LogWarning("Could not find activated object: " + path);
                    }
                }

                foreach (string path in data.deletedObstaclePaths)
                {
                    GameObject obj = FindObjectByHierarchyPath(path);
                    if (obj != null)
                    {
                        Destroy(obj);
                        Debug.Log("Destroyed obstacle: " + path);
                    }
                    else
                    {
                        Debug.LogWarning("Could not find obstacle to destroy: " + path + " (might already be destroyed or path changed)");
                    }
                }

                GameSaveTracker.instance.hasPlayedStartingCutscene = data.hasPlayedStartingCutscene;

                if (data.hasPlayedStartingCutscene)
                {
                    // Skip starting cutscene
                    CutsceneManager.instance.SkipStartingCutscene();

                    yield return new WaitForSeconds(0.3f);

                    if (player != null)
                    {
                        player.gameObject.GetComponent<StateMachine_3D>().enabled = false;
                        player.gameObject.GetComponent<CharacterController>().enabled = false;
                        player.gameObject.GetComponent<PlayerInput>().enabled = false;
                        player.gameObject.GetComponent<CharacterInputHandler>().enabled = false;

                        player.transform.position = data.playerPosition;
                        player.transform.rotation = data.playerRotation;

                        player.gameObject.GetComponent<StateMachine_3D>().enabled = true;
                        player.gameObject.GetComponent<CharacterController>().enabled = true;
                        player.gameObject.GetComponent<PlayerInput>().enabled = true;
                        player.gameObject.GetComponent<CharacterInputHandler>().enabled = true;

                        player.gameObject.GetComponent<PlayerInteraction_3D>().enabled = true;
                        player.gameObject.GetComponent<C_CameraController>().enabled = true;
                    }

                    CollectableManager.instance.itemCollectUI.SetActive(false);

                    loadingScreenObject.SetActive(false); 
                    
                    yield break;
                }
            }
        }

        // Normal starting cutscene
        loadingScreenObject.SetActive(false);
        CutsceneManager.instance.PlayStartingCutscene();
        GameSaveTracker.instance.hasPlayedStartingCutscene = true; // Mark as played
    }

    GameObject FindObjectByHierarchyPath(string path)
    {
        string[] parts = path.Split('/');
        if (parts.Length == 0) return null;

        // Start from all root objects in the current scene
        foreach (GameObject rootObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (rootObj.name == parts[0])
            {
                if (parts.Length == 1) return rootObj;

                Transform current = rootObj.transform;
                for (int i = 1; i < parts.Length; i++)
                {
                    Transform child = current.Find(parts[i]);
                    if (child == null) return null;
                    current = child;
                }
                return current.gameObject;
            }
        }
        return null;
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
