using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivateOnTrigger_Helper : MonoBehaviour
{
    public GameObject[] objectToActivate;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject _ in objectToActivate)
            {
                _.SetActive(true);
            }
            EnemyManager.instance.ActivateEnemy();

            Destroy(gameObject, .4f);
        }
    }
}
