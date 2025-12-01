using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnTrigger_Helper : MonoBehaviour
{
    public GameObject[] objectToActivate;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject obj in objectToActivate)
            {
                obj.SetActive(true);
            }

            Invoke(nameof(Deactivate), .4f);
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
