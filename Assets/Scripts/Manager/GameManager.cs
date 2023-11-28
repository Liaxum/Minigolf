using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        // Private
    
        // Public
        public static GameManager Instance;
        [HideInInspector] public int currentLevelIndex;
        [HideInInspector] public GameStatus gameStatus = GameStatus.None;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    [System.Serializable]
    public enum GameStatus
    {
        None,
        Playing,
        Failed,
        Complete
    }
}