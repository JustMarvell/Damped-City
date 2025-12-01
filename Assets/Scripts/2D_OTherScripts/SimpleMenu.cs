using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleMenu : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        GameMaster.instance.PauseGame();
    }

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        GameMaster.instance.ResumeGame();
    }

    public void QuitGame()
    {
        GameMaster.instance.QuitGame();
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
