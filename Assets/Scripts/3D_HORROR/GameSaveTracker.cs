using UnityEngine;
using System.Collections.Generic;

public class GameSaveTracker : MonoBehaviour
{
    public static GameSaveTracker instance;

    private HashSet<string> activatedObjects = new HashSet<string>();
    private HashSet<string> deletedObstacles = new HashSet<string>();
    public bool hasPlayedStartingCutscene = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterActivatedObject(GameObject obj)
    {
        if (obj != null)
            activatedObjects.Add(GetPath(obj.transform));
    }

    public void RegisterDeletedObstacle(GameObject obj)
    {
        if (obj != null)
            deletedObstacles.Add(GetPath(obj.transform));
    }

    public List<string> GetActivatedObjects() => new List<string>(activatedObjects);
    public List<string> GetDeletedObstacles() => new List<string>(deletedObstacles);

    private string GetPath(Transform t)
    {
        string path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}