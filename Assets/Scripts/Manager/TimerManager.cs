using System;
using UnityEngine;

namespace Manager
{
    // Class to manage the timer in the game
    public class TimerManager : MonoBehaviour
    {
        // Private
        private static float _startTime;
        
        // Public
        public static TimerManager Instance;
        public float elapsedTime;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }
        
        private void FixedUpdate()
        {
            // If we are playing a level then we calculate the elapsed time and we update the displayed elapsed time
            if (GameManager.Instance.gameStatus == GameStatus.Playing)
            {
                elapsedTime = Time.time - _startTime;
                UIManager.Instance.TimerText.text = UIManager.Instance.FormatTimer(elapsedTime);
            }
        }
        
        // Start the timer
        public void StartTimer()
        {
            _startTime = Time.time;
        }

        // Reset the timer
        public void ResetTimer()
        {
            elapsedTime = 0f;
        }
    }
}