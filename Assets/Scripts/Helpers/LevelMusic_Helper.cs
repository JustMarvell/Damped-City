using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMOD;

public class LevelMusic_Helper : MonoBehaviour
{
    EventInstance musicSound;
    PLAYBACK_STATE pLAYBACK_STATE;

    public static LevelMusic_Helper instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        musicSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.MUSIC_LevelScene);
        musicSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void StartMusic()
    {
        musicSound.getPlaybackState(out pLAYBACK_STATE);

        if (pLAYBACK_STATE.Equals(PLAYBACK_STATE.STOPPED))
        {
            musicSound.start();
        }
    }

    public void StopMusic()
    {
        musicSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    void OnDisable()
    {
        musicSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
