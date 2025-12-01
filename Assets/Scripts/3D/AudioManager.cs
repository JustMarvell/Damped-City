using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using System;

public class AudioManager : MonoBehaviour
{
    [Header("Volume Controls")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float sfxVolume = 1;
    [Range(0, 1)]
    public float uiVolume = 1;

    [Header("Volume Settings")]
    public VolumeSettings masterSetting;
    public VolumeSettings musicSetting;
    public VolumeSettings sfxSetting;
    public VolumeSettings uiSetting;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;
    private Bus uiBus;

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    public static AudioManager instance;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        eventInstances = new();
        eventEmitters = new();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        uiBus = RuntimeManager.GetBus("bus:/UI");
    }

    void Start()
    {
        masterVolume = masterSetting.value;
        musicVolume = masterSetting.value;
        sfxVolume = sfxSetting.value;
        uiVolume = uiSetting.value;

        masterBus.setVolume(masterVolume);
        masterSetting.SetValue(masterVolume);

        musicBus.setVolume(musicVolume);
        musicSetting.SetValue(musicVolume);

        sfxBus.setVolume(sfxVolume);
        sfxSetting.SetValue(sfxVolume);

        uiBus.setVolume(uiVolume);
        uiSetting.SetValue(uiVolume);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void SetVolume()
    {
        masterBus.setVolume(masterVolume);
        masterSetting.SetValue(masterVolume);

        musicBus.setVolume(musicVolume);
        musicSetting.SetValue(musicVolume);

        sfxBus.setVolume(sfxVolume);
        sfxSetting.SetValue(sfxVolume);

        uiBus.setVolume(uiVolume);
        uiSetting.SetValue(uiVolume);
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void Cleanup()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    void OnDestroy()
    {
        Cleanup();
    }
}
