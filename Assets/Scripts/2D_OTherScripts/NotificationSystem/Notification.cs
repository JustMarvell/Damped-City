using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Notification
{
    public float notificationTime = 3f;

    [Space]

    [TextArea(2, 10)]
    public string notification;

    public GameObject self;
}