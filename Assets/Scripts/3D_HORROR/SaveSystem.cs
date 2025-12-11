using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    private string saveFilePath;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "gamesave.json");
    }

    public void SaveGame()
    {
        GameData data = new GameData();

        // 1. Collected items
        if (Inventory.instance != null)
            data.collectedItemCount = Inventory.instance.CheckCollectedItemNumber(GameMaster.instance.itemToCollect);

        // 2. Retry chances
        if (GameMaster.instance != null)
            data.currentRetryChance = GameMaster.instance.currentRetryChange;

        // 3. Player position/rotation
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            data.playerPosition = player.position;
            data.playerRotation = player.rotation;
        }

        // 4. Activated objects from dialogues (we'll track via a static list - see below)
        data.activatedObjectPaths = GameSaveTracker.instance.GetActivatedObjects();

        // 5. Deleted obstacles
        data.deletedObstaclePaths = GameSaveTracker.instance.GetDeletedObstacles();

        // 6. Starting cutscene played
        data.hasPlayedStartingCutscene = GameSaveTracker.instance.hasPlayedStartingCutscene;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved to: " + saveFilePath);
    }

    public GameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game loaded from: " + saveFilePath);
            return data;
        }
        else
        {
            Debug.Log("No save file found.");
            return null;
        }
    }

    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted.");
        }
    }

    public bool HasSaveFile()
    {
        return File.Exists(saveFilePath);
    }
}