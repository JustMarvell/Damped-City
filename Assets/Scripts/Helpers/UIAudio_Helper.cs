using UnityEngine;
using UnityEngine.EventSystems;

public class UIAudio_Helper : MonoBehaviour
{
    GameObject mainCamera;
    FMODEvents fMODEvents;
    AudioManager audioManager;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        fMODEvents = FMODEvents.instance;
        audioManager = AudioManager.instance;
    }

    public void OnPointerClick()
    {
        audioManager.PlayOneShot(fMODEvents.UI_ButtonClick, mainCamera.transform.position);
    }

    public void OnPointerEnter()
    {
        audioManager.PlayOneShot(fMODEvents.UI_ButtonHover, mainCamera.transform.position);
    }

    public void OnPointerMove()
    {
        audioManager.PlayOneShot(fMODEvents.UI_SliderChanged, mainCamera.transform.position);
    }
}