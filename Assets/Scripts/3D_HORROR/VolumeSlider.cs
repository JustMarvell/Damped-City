using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MASTER,
        MUSIC,
        SFX,
        UI
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;
    private Slider volumeSlider;

    void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
    }

    void Start()
    {
        AudioManager.instance.SetVolume();

        switch (volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = AudioManager.instance.masterVolume;
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = AudioManager.instance.musicVolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = AudioManager.instance.sfxVolume;
                break;
            case VolumeType.UI:
                volumeSlider.value = AudioManager.instance.uiVolume;
                break;
            default:
                Debug.LogWarning("VolumeType not supported!");
                break;
        }
    }

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.instance.masterVolume = volumeSlider.value;
                break;
            case VolumeType.MUSIC:
                AudioManager.instance.musicVolume = volumeSlider.value;
                break;
            case VolumeType.SFX:
                AudioManager.instance.sfxVolume = volumeSlider.value;
                break;
            case VolumeType.UI:
                AudioManager.instance.uiVolume = volumeSlider.value;
                break;
            default:
                Debug.LogWarning("VolumeType not supported!");
                break;
        }

        AudioManager.instance.SetVolume();
    }
}