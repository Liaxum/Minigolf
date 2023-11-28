using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script which makes camera follow ball
public class CameraFollow : MonoBehaviour
{
    // Private
    [SerializeField] private ActiveVectors activeVectors;
    
    // Target reference to follow
    private GameObject followTarget;
    // Offset between the camera and the target
    private Vector3 offset;
    // Save of the camera position
    private Vector3 changePos;
    
    // Public
    // Class which decide which axis to follow the target
    public static CameraFollow instance;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    
    public void SetTarget(GameObject target)
    {
        // Set the target
        followTarget = target;
        // Calculate the offset
        offset = followTarget.transform.position - transform.position;
        // Save the position of the camera
        changePos = transform.position;
    }
    
    private void LateUpdate()
    {
        // If we don't have a target do nothing
        if (!followTarget) return;
        
        // If the X axis is followed
        // Then calculate the new position of the camera on X
        if (activeVectors.x)
            changePos.x = followTarget.transform.position.x - offset.x;
        
        // If the Y axis is followed
        // Then calculate the new position of the camera on Y
        if (activeVectors.y)
            changePos.y = followTarget.transform.position.y - offset.y;
        
        // If the Z axis is followed
        // Then calculate the new position of the camera on Z
        if (activeVectors.z)
            changePos.z = followTarget.transform.position.z - offset.z;
        
        // Change the camera position
        transform.position = changePos;
    }
}

[System.Serializable]
public class ActiveVectors
{
    public bool x, y, z;
}
