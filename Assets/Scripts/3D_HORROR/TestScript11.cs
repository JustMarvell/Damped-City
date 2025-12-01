using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript11 : MonoBehaviour
{
    public string testName;
    public GameObject testPrefab;
    public Transform testTrasnform;
    public Image testImage;
    public float testValue = 1f;

    void Start()
    {
        Debug.Log("Test1KSKSKSK");
    }

    void Update()
    {
        
    }

    void OnValidate()
    {
        if (testImage != null)
            testImage.fillAmount = testValue / 10f;
    }
}
