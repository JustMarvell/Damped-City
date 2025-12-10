using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public static bool IsInInteraction;
    public static bool IsPaused;

    public MonoBehaviour[] objectToDeactivateOnInteraction;
    public GameObject[] deactivateOnFinished;

    public bool isUsingInventory = false;
    public Item itemToCollect;
    public int numberOfItemToCollect = 15;
    public bool isCollectedAll = false;

    [Space]

    public bool useRunAlert = false;

    Inventory inventory;

    [Space]

    public InputAction pauseAction;
    public bool pauseInput;

    [Space]

    public bool usePauseMenu = false;
    public bool stopTimeOnPause = false;
    public GameObject[] activateOnPause;

    [Space]

    public GameObject gameOverMenu;
    public GameObject gameWinMenu;
    public GameObject gameRetryMenu;

    [Space]

    public int maxRetryChange = 2;
    public int currentRetryChange;

    void Awake()
    {
        IsPaused = false;
        instance = this;
    }

    void Start()
    {
        if (Inventory.instance != null)
        {
            inventory = Inventory.instance;
            inventory.onItemChangedCallback += UpdateItemNumber;
        }

        isGameOver = false;
        currentRetryChange = maxRetryChange;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        pauseInput = pauseAction.triggered;

        if (pauseInput)
        {
            if (!IsPaused)
            {
                PauseGame_nt();
            }
        }
    }

    void UpdateItemNumber()
    {
        if (inventory.CheckCollectedItemNumber(itemToCollect) == numberOfItemToCollect && !isCollectedAll)
        {
            isCollectedAll = true;
            foreach (GameObject obj in deactivateOnFinished)
            {
                obj.SetActive(false);
            }

            if (useRunAlert)
            {
                EnemyManager.instance.MayhemMode();
            }
        }
    }

    public void ActivateObjects()
    {
        foreach (MonoBehaviour obj in objectToDeactivateOnInteraction)
        {
            obj.enabled = true;
        }
    }

    public void DeactivateObjects()
    {
        foreach (MonoBehaviour obj in objectToDeactivateOnInteraction)
        {
            obj.enabled = false;
        }
    }

    public void PauseGame()
    {
        if (stopTimeOnPause)
            Time.timeScale = 0f;
        IsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DeactivateObjects();
    }

    public void PauseGame_nst()
    {
        IsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DeactivateObjects();
    }

    public void PauseGame_nt()
    {
        if (stopTimeOnPause)
            Time.timeScale = 0f;
        IsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        foreach (GameObject _ in activateOnPause)
        {
            _.SetActive(true);
        }

        DeactivateObjects();
    }

    bool isGameOver = false;

    public void GameOver()
    {
        if (currentRetryChange <= 0 && !isGameOver)
        {
            isGameOver = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            CutsceneManager.instance.PlayGameOverCutscene();
        }
        else if (currentRetryChange > 0)
        {
            currentRetryChange--;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;

            gameRetryMenu.SetActive(true);
        }
    }

    public void WinGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        CutsceneManager.instance.PlayWinGameCutscene();
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (stopTimeOnPause)
            Time.timeScale = 1f;
        IsPaused = false;
        Invoke(nameof(ActivateObjects), .1f);
    }

    public void ResumeGame_nst()
    {
        if (stopTimeOnPause)
            Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        IsPaused = false;
        ActivateObjects();
    }

    void OnEnable()
    {
        pauseAction.Enable();
    }

    void OnDisable()
    {
        pauseAction.Disable();
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
