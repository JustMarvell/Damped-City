using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableNotificationTrigger : MonoBehaviour
{
    public Notification notification;

    public void TriggerNotification()
    {
        NotificationManager.instance.StartNotification(notification);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerNotification();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            TriggerNotification();
    }
}
