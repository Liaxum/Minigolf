using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Windmill : MonoBehaviour
{
    // Private
    [SerializeField] private float rotationSpeed = 50f;
    
    // Public

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
    }
}
