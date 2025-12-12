using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;


    public GameObject[] enemyPrefab;
    public Transform[] enemySpawnPoint;
    public List<EnemyAI_Horror> enemies;

    [Space]

    public bool isChasingPlayer;
    public bool isCurious;
    public bool hasAttackedPlayer = false;

    public float chaseSoundExitDelay = 3f;
    public float curiousSoundExitDelay = 3f;

    EventInstance chasingSound;
    EventInstance attackingSound;
    EventInstance curiousSound;
    PLAYBACK_STATE pLAYBACK_STATE;
    PLAYBACK_STATE CURIOUS_PLAYBACK_STATE;

    public bool enemySpawned = false;
    public float spawnProgress = 0;

    public int chasingCount = 0;
    public int curiousCount = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        chasingSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.ENEMY_Chasing);
        chasingSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        attackingSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.ENEMY_Attacking);
        attackingSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        curiousSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.ENEMY_Curious);
        curiousSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void StartChasing()
    {
        chasingCount++;

        if (chasingCount > 0)
        {
            print("Chasingggg");
            isChasingPlayer = true;
            OnEnemyChase();
        }
    }

    public void StartCurious()
    {
        curiousCount++;

        if (curiousCount > 0)
        {
            print("Curious!");
            isCurious = true;
            OnEnemyCurious();
        }
    }

    public void StopChasing()
    {
        chasingCount--;

        if (chasingCount <= 0)
        {
            print("Stoped chasing");
            chasingCount = 0;
            isChasingPlayer = false;
            OnEnemyChase();
        }
    }

    public void StopCurious()
    {
        curiousCount--;

        if (curiousCount <= 0)
        {
            print("Stop Curious");
            curiousCount = 0;
            isCurious = false;
            OnEnemyCurious();
        }
    }

    public void ActivateEnemy()
    {
        foreach (EnemyAI_Horror enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }

    public void OnEnemyChase()
    {
        if (isChasingPlayer)
        {
            chasingSound.getPlaybackState(out pLAYBACK_STATE);

            if (pLAYBACK_STATE.Equals(PLAYBACK_STATE.STOPPED))
                PlayStopChase();
            else
                Invoke(nameof(PlayStopChase), chaseSoundExitDelay);
        }
        else
        {
            chasingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    void PlayStopChase()
    {
        if (pLAYBACK_STATE.Equals(PLAYBACK_STATE.STOPPED))
        {
            chasingSound.start();
            curiousSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void OnEnemyCurious()
    {
        if (isCurious)
        {
            curiousSound.getPlaybackState(out CURIOUS_PLAYBACK_STATE);

            if (CURIOUS_PLAYBACK_STATE.Equals(PLAYBACK_STATE.STOPPED))
                PlayStopCurious();
            else
                Invoke(nameof(PlayStopCurious), curiousSoundExitDelay);
        }
        else
        {
            curiousSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    void PlayStopCurious()
    {
        if (CURIOUS_PLAYBACK_STATE.Equals(PLAYBACK_STATE.STOPPED))
        {
            curiousSound.start();
            chasingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void OnEnemyAttack()
    {
        if (!hasAttackedPlayer)
        {
            chasingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            hasAttackedPlayer = true;
            attackingSound.start();
        }
    }

    public void StopChasingSound()
    {
        chasingSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void StopCuriousSound()
    {
        curiousSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void SpawnEnemy(int enemyCount)
    {
        if (!enemySpawned)
        {
            GameObject rAlert = GameObject.FindGameObjectWithTag("runAlert");

            // do stuff
            for (int y = 0; y < enemyCount; y++)
            {
                int r = Random.Range(0, enemyPrefab.Length);
                enemies.Add(Instantiate(enemyPrefab[r]).GetComponent<EnemyAI_Horror>());
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].runAlert = rAlert;

                enemies[i].transform.parent = enemySpawnPoint[i];
                enemies[i].transform.position = Vector3.zero;
                enemies[i].transform.localPosition = Vector3.zero;
                enemies[i].gameObject.SetActive(false);

                spawnProgress = i / enemies.Count;
            }


            rAlert.SetActive(false);
            // finished doing stuff
            enemySpawned = true;
        }
    }

    public void ResetAndDisableEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.position = Vector3.zero;
            enemies[i].transform.localPosition = Vector3.zero;
            enemies[i].hasAttacked = false;

            enemies[i].gameObject.SetActive(false);
        }
    }

    public void ResetAndEnableEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.position = Vector3.zero;
            enemies[i].transform.localPosition = Vector3.zero;

            enemies[i].gameObject.SetActive(true);
        }
    }

    public void MayhemMode()
    {
        enemies[0].runAlert.SetActive(true);
        foreach (EnemyAI_Horror _ in enemies)
        {
            _.useLineOfSight = false;
            _.obstacleMask = 0;
            _.fieldOfViewAngle = 360;
            _.lineOfSightDistance = 300;
        }
    }
}