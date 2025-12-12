using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    public DificultySettings dificultySettings;
    public string sceneNameToUse;

    List<EnemyAI_Horror> _enemies;
    StateMachine_3D _player;
    GameMaster _gm;
    CollectableManager _cm;

    public static DifficultyManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetDifficulty(DificultySettings dificulty)
    {
        dificultySettings = dificulty;
    }

    void SetupDifficulty(List<EnemyAI_Horror> enemies, StateMachine_3D player, GameMaster gm, CollectableManager cm)
    {
        foreach (EnemyAI_Horror enemy in enemies)
        {
            enemy.normalSpeed = dificultySettings.enemyNormalSpeed;
            enemy.sprintSpeed = dificultySettings.enemySprintSpeed;
            enemy.lineOfSightDistance = dificultySettings.enemyLineOfSightDistance;
            enemy.maxWalkRadius = dificultySettings.enemyDetectionRadius;
        }

        player.normalSpeed = dificultySettings.playerNormalSpeed;
        player.sprintSpeed = dificultySettings.playerSprintSpeed;
        player.staminaRegenRate = dificultySettings.staminaRegenRate;
        player.staminaConsumtionRate = dificultySettings.staminaConsumtionRate;

        gm.numberOfItemToCollect = dificultySettings.collectableCount;
        cm.numberOfItemToCollect = dificultySettings.collectableCount;
    }

    void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.isLoaded)
        {
            if (scene.name == sceneNameToUse)
            {
                _enemies = EnemyManager.instance.enemies;
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<StateMachine_3D>();
                _gm = GameMaster.instance;
                _cm = CollectableManager.instance;

                EnemyManager.instance.SpawnEnemy(dificultySettings);

                SetupDifficulty(_enemies, _player, _gm, _cm);
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }
}
