using UnityEngine;
using System.Collections.Generic;

public class CollectableManager : MonoBehaviour
{
    public static CollectableManager instance;

    void Awake()
    {
        instance = this;
    }

    public Item itemToCollect;
    public int numberOfItemToCollect = 15;
    public bool isCollectedAll = false;
    public GameObject[] activateOnAllCollected;
    public GameObject[] deactivateOnAllCollected;

    [Space]

    public Transform[] spawnPoints;

    [Header("Debug")]
    public bool showGizmos = true;

    private HashSet<int> usedIndices = new HashSet<int>();
    private List<int> availableIndices = new List<int>();

    public bool isSpawned;
    public float spawnProgress = 0;

    Inventory inventory;

    void Start()
    {
        if (Inventory.instance != null)
        {
            inventory = Inventory.instance;
            inventory.onItemChangedCallback += UpdateItemNumber;
        }

        SpawnCollectable();
    }

    void UpdateItemNumber()
    {
        if (inventory.CheckCollectedItemNumber(itemToCollect) == numberOfItemToCollect && !isCollectedAll)
        {
            isCollectedAll = true;
            foreach (GameObject _ in activateOnAllCollected)
                _.SetActive(true);

            foreach (GameObject _ in deactivateOnAllCollected)
                _.SetActive(false);
        }
    }
    
    void SpawnCollectable()
    {
        if (!isSpawned)
        {
            // reset bru kse siap index yg mdi pke
            availableIndices.Clear();
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                availableIndices.Add(i);
            }

            usedIndices.Clear();

            // acak n pilih index random
            for (int i = 0; i < numberOfItemToCollect; i++)
            {
                if (availableIndices.Count == 0) break;

                int randomIndex = Random.Range(0, availableIndices.Count);
                int spawnPointIndex = availableIndices[randomIndex];
                availableIndices.RemoveAt(randomIndex);
                usedIndices.Add(spawnPointIndex);

                // Instantiate at spawn point
                Transform spawnPoint = spawnPoints[spawnPointIndex];
                Instantiate(itemToCollect.itemPrefab, spawnPoint.position, spawnPoint.rotation);

                spawnProgress = i / numberOfItemToCollect;
            }

            isSpawned = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos || spawnPoints == null) return;

        foreach (Transform point in spawnPoints)
        {
            if (point == null) continue;

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(point.position, 0.2f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(point.position, Vector3.one * 0.3f);
        }
    }
}