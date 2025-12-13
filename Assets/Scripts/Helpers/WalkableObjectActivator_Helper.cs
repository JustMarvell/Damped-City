using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableObjectActivator_Helper : MonoBehaviour
{
    public GameObject[] targets;
    public bool randomize = false;
    bool active = false;

    public bool triggerOnce = false;
    bool triggered = false;

    void Start()
    {
        foreach (GameObject target in targets)
            target.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (active)
                return;
            active = true;

            if (triggerOnce && triggered)
                return;
            triggered = true;

            if (randomize)
            {
                int i = Random.Range(0, targets.Length);
                targets[i].SetActive(true);
            }
            else
            {
                foreach (GameObject target in targets)
                    target.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            active = false;
            foreach (GameObject target in targets)
                target.SetActive(false);
        }
    }
}
