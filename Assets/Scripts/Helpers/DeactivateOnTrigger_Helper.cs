using UnityEngine;

public class DeactivateOnTrigger_Helper : MonoBehaviour
{
    public GameObject[] objectToDeactivate;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject obj in objectToDeactivate)
            {
                obj.SetActive(false);
            }

            Invoke(nameof(Deactivate), .4f);
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
