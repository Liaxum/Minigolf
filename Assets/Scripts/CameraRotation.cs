using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script which makes camera rotate around ball
public class CameraRotation : MonoBehaviour
{
    // Private
    [SerializeField] private float rotationSpeed = 0.2f;
    
    // Public
    public static CameraRotation instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    
    public void RotateCamera(float xaxisRotation)           
    {
        // Rotate the camera
        transform.Rotate(Vector3.down, -xaxisRotation * rotationSpeed); //rotate the camera
    }
}
