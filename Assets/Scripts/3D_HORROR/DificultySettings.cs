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

    [Header("Enemy Type Settings")]
    [Header("Mutant Zombie")]
    public float zombie_normalSpeed = 2.5f;
    public float zombie_sprintSpeed = 4f;
    public float zombie_fieldOfViewAngle = 120f;
    public float zombie_lineOfSightDistance = 8f;
    public float zombie_maxWalkRadius = 20f;
    public float zombie_minIdleTime = 5f;
    public float zombie_maxIdleTime = 8f;
    public bool zombie_useLineOfSight = true;
    public float zombie_chaseMemoryTime = 10f;
    public float zombie_navAgentAcceleration = 4f;
    public float zombie_navAgentAngularSpeed = 90f;

    [Header("Mutant Skull")]
    public float skull_normalSpeed = 5f;
    public float skull_sprintSpeed = 8f;
    public float skull_fieldOfViewAngle = 45f;
    public float skull_lineOfSightDistance = 18f;
    public float skull_maxWalkRadius = 10f;
    public float skull_minIdleTime = 1f;
    public float skull_maxIdleTime = 3f;
    public bool skull_useLineOfSight = true;
    public float skull_chaseMemoryTime = 4f;
    public float skull_navAgentAcceleration = 12f;
    public float skull_navAgentAngularSpeed = 360f;

    [Header("Scavanger")]
    public float scavanger_normalSpeed = 4f;
    public float scavanger_sprintSpeed = 7f;
    public float scavanger_fieldOfViewAngle = 80f;
    public float scavanger_lineOfSightDistance = 12f;
    public float scavanger_maxWalkRadius = 15f;
    public float scavanger_minIdleTime = 2f;
    public float scavanger_maxIdleTime = 6f;
    public bool scavanger_useLineOfSight = false;  // Hears player (distance-only detection)
    public float scavanger_chaseMemoryTime = 7f;
    public float scavanger_navAgentAcceleration = 8f;
    public float scavanger_navAgentAngularSpeed = 180f;

    [Header("Objective Settings")]
    public int collectableCount = 15;

    [Header("Player Settings")]
    public float playerNormalSpeed = 5f;
    public float playerSprintSpeed = 9f;
    public float staminaRegenRate = 15f;
    public float staminaConsumtionRate = 20f;
}