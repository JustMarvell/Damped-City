using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;
using FMODUnity;

public class NotificationManager : MonoBehaviour
{
    public TextMeshProUGUI notificationText;

    [Space]

    public Animator animator;

    public EventInstance popUpSound;
    
    FMODEvents fMODEvents;
    AudioManager audioManager;
    Notification notification;
    bool haveNotification = false;

    public static NotificationManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        fMODEvents = FMODEvents.instance;
        audioManager = AudioManager.instance;
        haveNotification = false;
    }

    public void DisableNotification()
    {
        
    }

    public void EnableNotification()
    {
        
    }

    public void StartNotification(Notification _notification)
    {
        if (haveNotification)
            return;

        haveNotification = true;
        notification = _notification;

        audioManager.PlayOneShot(fMODEvents.NOTIFICATION_PopUp, Camera.main.transform.position);

        animator.SetTrigger("Display");
    }

    public void DisplayText()
    {
        notificationText.text = notification.notification;
    }

    public void Reset()
    {
        notificationText.text = "";
    }

    public void EndNotification()
    {
        haveNotification = false;

        if (notification.self != null)
            Destroy(notification.self);

        notification = null;
    }
}