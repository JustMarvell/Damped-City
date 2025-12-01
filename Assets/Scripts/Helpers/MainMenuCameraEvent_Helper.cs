using UnityEngine;

public class MainMenuCameraEvent_Helper : MonoBehaviour
{
    public GameObject leftGO;
    public GameObject rightGO;
    public GameObject midGO;
    public GameObject blackSc;

    public void ActivateBlackPower()
    {
        blackSc.SetActive(true);
    }

    public void ActivateLeft()
    {
        leftGO.SetActive(true);
    }

    public void ActivateRight()
    {
        rightGO.SetActive(true);
    }

    public void ActivateMid()
    {
        midGO.SetActive(true);
    }

    public void StartGame()
    {
        SceneManagerLoader_Helper.instance.SwitchToLevelOneScene();
    }

    public void Quit()
    {
        SceneManagerLoader_Helper.instance.QuitGame();
    }
}