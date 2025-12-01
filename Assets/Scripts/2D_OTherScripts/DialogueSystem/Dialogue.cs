using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public string name;
    public bool isEndGameDialogue = false;
    public float endGameTriggerTime = 5f;

    [Space]

    public bool isFinalGameDialogue = false;
    public float finalGameTriggerTime = 5f;

    [Space]

    public bool canDeleteObstacle = false;
    public GameObject[] ObstaclesToDelete;

    [Space]

    public bool canActivateObject = false;
    public GameObject[] objectToActivate;

    [Space]

    public bool haveNextDialogue = false;
    public DialogueTrigger nextDialogue;
    public float nextDialogueCooldown = 5f;

    [Space]

    

    [Space]
    public Sentences[] sentences;
}

[System.Serializable]
public class Sentences
{
    public string speaker;
    public Transform lookTarget;
    [TextArea(3, 10)]
    public string sentences;
}
