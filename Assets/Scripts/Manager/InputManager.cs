using UnityEngine;

// Script which detect mouse click and decide who will take input Ball or Camera
namespace Manager
{
    public class InputManager : MonoBehaviour
    {   
        // Private
        // Distance between the ball and the mouse that activates
        [SerializeField] private float distanceBetweenBallAndMouseClickLimit = 1.5f;
    
        // For tracking distance between the ball and the mouse
        private float _distanceBetweenBallAndMouseClick;
        private bool _canRotate;
    
        // Public

        private void Update()
        {
            // If the game is not playing return immediately
            if (GameManager.Instance.gameStatus != GameStatus.Playing) return;
            // If the player presses the escape key
            // the game is paused
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.gameStatus = GameStatus.Pause;
                UIManager.Instance.GameResult();
            }

            Time.timeScale = GameManager.Instance.gameStatus == GameStatus.Pause ? 0f : 1f;
            
            // If the mouse button is clicked and It's not possible to rotate
            // Get the distance between the ball and the mouse click
            // Set the rotation possible
            if (Input.GetMouseButtonDown(0) && !_canRotate)
            {
                GetDistance();
                _canRotate = true;

                // If distance is less than the limit allowed
                // We can control the ball
                if (_distanceBetweenBallAndMouseClick <= distanceBetweenBallAndMouseClickLimit)
                    Ball.Instance.MouseDownMethod();
            }
            
            // If it's impossible to rotate do nothing
            if (!_canRotate) return;
            // If the mouse is clicked and the distance is less than the limit
            // We can control the ball
            // Otherwise we can rotate the camera
            if (Input.GetMouseButton(0) && _distanceBetweenBallAndMouseClick <= distanceBetweenBallAndMouseClickLimit)                                                  
                Ball.Instance.MouseNormalMethod();
            else
                CameraRotation.instance.RotateCamera(Input.GetAxis("Mouse X"));

            // If the mouse still down
            // We can't rotate the camera
            if (!Input.GetMouseButtonUp(0)) return;
            _canRotate = false;
        
            // If distance is less than the limit allowed
            // We handle the ball movement
            if (_distanceBetweenBallAndMouseClick <= distanceBetweenBallAndMouseClickLimit)
                Ball.Instance.MouseUpMethod();
        }

        private void GetDistance()
        {
            // We create a plane whose mid point is at ball position and whose normal is toward Camera
            Plane plane = new Plane(Camera.main.transform.forward, Ball.Instance.transform.position);
            // We create a ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            // We check if the plane and the ray intersect
            if (plane.Raycast(ray, out float distance))
            {
                // We get the point of intersection
                Vector3 v3Pos = ray.GetPoint(distance);
                // And calculate the distance
                _distanceBetweenBallAndMouseClick = Vector3.Distance(v3Pos, Ball.Instance.transform.position);
            }
        }
    }
}
