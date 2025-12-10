using UnityEngine;

public class StartingNotificationTrigger : MonoBehaviour
{
    public Notification notification;

    void Start()
    {
        NotificationManager.instance.StartNotification(notification);
    }
}
