using UnityEngine;

[CreateAssetMenu(fileName ="Difficulty", menuName ="Difficulty/New Difficulty")]
public class DificultySettings : ScriptableObject
{
    public new string name;

    [Header("Enemy Settings")]

    public int enemyCount = 2;
    public float enemyNormalSpeed = 1f;
    public float enemySprintSpeed = 5.5f;
    public float enemyLineOfSightDistance = 15f;
    public float enemyDetectionRadius = 15f;

    [Header("Objective Settings")]
    public int collectableCount = 15;

    [Header("Player Settings")]
    public float playerNormalSpeed = 5f;
    public float playerSprintSpeed = 9f;
    public float staminaRegenRate = 15f;
    public float staminaConsumtionRate = 20f;
}