using Manager;
using UnityEngine;

// Script which controls the ball
// Need of a RigidBody
[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    // Private
    // Reference to the line render (Line to see the direction between the ball and the mouse)
    [SerializeField] private LineRenderer lineRenderer;
    // Max strength that can be applied to the ball
    [SerializeField] private float maxStrength;
    [SerializeField] private float strengthModifier = 0.5f;
    // Layer allowed to be detected by the ray
    [SerializeField] private LayerMask rayLayer;
    
    // Strength which is applied to the ball
    private float _strength;
    private Rigidbody _rgBody;
    private Vector3 _startPos, _endPos;
    // Bool to make shooting stop the ball easier
    private bool _canShoot, _ballIsStatic = true;
    // Direction in which the ball will be shot
    private Vector3 _direction;
    
    // Public
    public static Ball Instance;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Get the reference of the rigidbody
        _rgBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // If the velocity is not zero and the ball is not static do nothing
        if (_rgBody.velocity != Vector3.zero || _ballIsStatic) return; 
        // Otherwise the ball is not moving
        _ballIsStatic = true;
        // Notify the Level Manager of the shot is done
        LevelManager.Instance.ShotTaken();
        // Set the angular velocity to zero
        _rgBody.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        // If its impossible to shoot do nothing do nothing
        if (!_canShoot) return;
        // Otherwise shoot
        _canShoot = false;
        _ballIsStatic = false;
        // Get the direction between from start and end position
        _direction = _startPos - _endPos;
        // Add the force to the ball in the given direction
        _rgBody.AddForce(_direction * _strength, ForceMode.Impulse);
        // Reset the power bar to zero
        UIManager.Instance.PowerBar.fillAmount = 0;
        // Reset the strength to zero
        _strength = 0;
        // Set the new stating position
        _startPos = _endPos = Vector3.zero;
    } 
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            // If the object name is Destroyer
            // Then the level is failed
            case "Destroyer":
                LevelManager.LevelFailed();
                break;
            // If the object name is Hole
            // Then the level is complete
            case "Hole":
                LevelManager.Instance.LevelComplete();
                break;
        }
    }
    
    // Method used to convert the mouse position to the world position in respect to Level
    private Vector3 ClickedPoint()
    {
        Vector3 position = Vector3.zero;
        // Create a ray in the camera to the mouse position
        Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
        // If the ray hit the ray layer then the we se the position
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, rayLayer))
            position = hit.point;
        
        return position;
    }

    // Method called when the mouse is down by the Input Manager
    public void MouseDownMethod()
    {
        // If the ball is moving do nothing
        if(!_ballIsStatic) return;
        // Get the start position in world space
        _startPos = ClickedPoint();
        // Active the lineRenderer to see where too shoot
        lineRenderer.gameObject.SetActive(true);
        // Set the first position of the line renderer 
        lineRenderer.SetPosition(0, lineRenderer.transform.localPosition);
    }

    // Method called by the Input Manager
    public void MouseNormalMethod()
    {
        // If the ball is moving do nothing
        if(!_ballIsStatic) return;
        // Get the end position in world space
        _endPos = ClickedPoint();
        // Calculate the the force with the distance between the start position and end position multiplied by the modifier factor,
        // and verify it not greater than the max strength
        _strength = Mathf.Clamp(Vector3.Distance(_endPos, _startPos) * strengthModifier, 0, maxStrength);
        // Fill the power bar
        UIManager.Instance.PowerBar.fillAmount = _strength / maxStrength;
        // We set the second position of the line renderer
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(_endPos));
    }

    // Method called when the mouse is up by the Input Manager
    public void MouseUpMethod()
    {
        // If the ball is moving do nothing
        if(!_ballIsStatic) return;
        // The ball is moving to the new position
        _canShoot = true;
        // Deactivate the the line renderer
        lineRenderer.gameObject.SetActive(false);
    }



#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

#endif

}
