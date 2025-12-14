using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayMenu : MonoBehaviour
{
    [System.Serializable]
    public class MonsterInfo
    {
        public GameObject prefab;      // Assign the 3D monster prefab here
        public string name;            // Monster name
        public string description;     // Description or other info
    }

    [Header("Monsters")]
    public MonsterInfo[] monsters;     // Array of monster data (assign in inspector)

    [Header("UI Elements")]
    public TextMeshProUGUI nameText;   // Assign the Name Text UI element
    public TextMeshProUGUI descText;   // Assign the Description Text UI element
    public Button nextButton;          // Assign the Next Button UI element

    [Header("Model Display")]
    public Transform modelSpawnPoint;  // Empty GameObject where model spawns (position/scale/rotation for display)
    public float rotationSpeed = 30f;  // Rotation speed in degrees per second (Y-axis for turntable effect)

    private GameObject currentModel;
    private int currentIndex = 0;

    void Start()
    {
        if (monsters == null || monsters.Length == 0)
        {
            Debug.LogError("No monsters assigned!");
            return;
        }

        // Hook up the next button
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(NextMonster);
        }

        // Show first monster
        ShowMonster(0);
    }

    void Update()
    {
        // Continuous rotation for turntable effect
        if (currentModel != null)
        {
            currentModel.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    public void NextMonster()
    {
        int nextIndex = (currentIndex + 1) % monsters.Length;
        ShowMonster(nextIndex);
    }

    private void ShowMonster(int index)
    {
        // Destroy previous model
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        // Instantiate new model as child of spawn point (inherits position/rotation/scale)
        currentModel = Instantiate(monsters[index].prefab, modelSpawnPoint);

        // Reset local transform to match spawn point perfectly
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.identity;
        currentModel.transform.localScale = Vector3.one;

        // Update UI
        UpdateUI(index);

        currentIndex = index;
    }

    private void UpdateUI(int index)
    {
        if (nameText != null)
            nameText.text = monsters[index].name;

        if (descText != null)
            descText.text = monsters[index].description;
    }

    // Optional: Call this to hide the display (e.g., when closing menu)
    public void HideDisplay()
    {
        if (currentModel != null)
        {
            Destroy(currentModel);
            currentModel = null;
        }
    }

    // Optional: Call this to show a specific monster by index (for testing)
    public void ShowSpecificMonster(int index)
    {
        if (index >= 0 && index < monsters.Length)
        {
            ShowMonster(index);
        }
    }

    void OnDestroy()
    {
        // Clean up button listener
        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(NextMonster);
        }
    }
}
