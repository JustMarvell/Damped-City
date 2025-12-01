using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerTrigger_Helper : MonoBehaviour
{
    public TriggerType triggerType;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (triggerType)
            {
                case TriggerType.ResetAndDisable:
                    EnemyManager.instance.ResetAndDisableEnemy();
                break;
                case TriggerType.ResetAndEnable:
                    EnemyManager.instance.ResetAndEnableEnemy();
                break;
            }

            Invoke(nameof(Deactivate), .4f);
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

public enum TriggerType
{
    ResetAndDisable,
    ResetAndEnable
}
