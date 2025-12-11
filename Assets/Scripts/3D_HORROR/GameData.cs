using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    // Core data
    public int collectedItemCount;
    public int currentRetryChance;
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    // storyline progress
    public List<string> activatedObjectPaths = new List<string>(); 
    public List<string> deletedObstaclePaths = new List<string>();
    public bool hasPlayedStartingCutscene = false;
}