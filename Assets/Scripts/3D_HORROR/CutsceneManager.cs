using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector startingCutscene;
    public PlayableDirector gameOverCutscene;
    public PlayableDirector gameWinCutscene;
    public PlayableDirector gameRetryCutscene;

    public static CutsceneManager instance;

    void Awake()
    {
        instance = this;
    }

    public void PlayGameRetryCutscene()
    {
        gameRetryCutscene.Stop();

        gameRetryCutscene.time = 0;

        gameRetryCutscene.Play();
    }

    public void PlayStartingCutscene()
    {
        startingCutscene.Stop();

        startingCutscene.time = 0;

        startingCutscene.Play();
    }

    public void PlayGameOverCutscene()
    {
        gameOverCutscene.Stop();

        gameOverCutscene.time = 0;

        gameOverCutscene.Play();
    }

    public void PlayWinGameCutscene()
    {
        gameWinCutscene.Stop();

        gameWinCutscene.time = 0;

        gameWinCutscene.Play();
    }
}
