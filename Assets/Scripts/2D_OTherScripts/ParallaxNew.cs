using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParallaxNew : MonoBehaviour
{
    public float multiplier;
    public bool horizontalOnly;
    public bool calculateInfiniteHorizontalPosition;
    public bool calculateInfiniteVerticalPosition;
    public bool isInfinite;

    public GameObject cam;
    private Vector3 startPosition;
    private Vector3 startCameraPosition;
    private float length;

    void Start()
    {
        startPosition = transform.position;
        startCameraPosition = cam.transform.position;

        if (isInfinite)
        {
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        CalculateStartPosition();
    }

    void CalculateStartPosition()
    {
        float distX = (cam.transform.position.x - transform.position.x) * multiplier;
        float distY = (cam.transform.position.y - transform.position.y) * multiplier;
        Vector3 tmp = new Vector3(startPosition.x, startPosition.y);

        if (calculateInfiniteHorizontalPosition)
        {
            tmp.x = transform.position.x + distX;
        }
        if (calculateInfiniteVerticalPosition)
        {
            tmp.y = transform.position.y + distY;
        }
    }

    void FixedUpdate()
    {
        Vector3 position = startPosition;

        if (horizontalOnly)
            position.x += multiplier * (cam.transform.position.x - startCameraPosition.x);
        else
            position += multiplier * (cam.transform.position - startCameraPosition);

        transform.position = position;

        if (isInfinite)
        {
            float tmp = cam.transform.position.x * (1 - multiplier);
            if (tmp > startPosition.x + length)
            {
                startPosition.x += length;
            }
            else if (tmp < startPosition.x - length)
            {
                startPosition.x -= length;
            }
        }
    }
}
