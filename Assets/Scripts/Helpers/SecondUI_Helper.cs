using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondUI_Helper : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject gameWinMenu;

    public void BackToMenu()
    {
        SaveSystem.instance.SaveGame();

        SceneManagerLoader_Helper.instance.SwitchToMenuScene();
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CutsceneManager.instance.PlayGameRetryCutscene();
    }

    public void GameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void GameWinMenu()
    {
        gameWinMenu.SetActive(true);
    }
}
