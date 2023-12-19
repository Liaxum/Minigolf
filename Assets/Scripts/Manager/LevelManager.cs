using UnityEngine;

// Script responsible for managing level, like spawning level, spawning balls, deciding game win/loss status and more
namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        // Private
        // To calculate the player's time for the level
        private float startTime;
        // Counter to store the available shots
        private int _shotCount;
        // Reference to the ball prefab
        [SerializeField] private GameObject ballPrefab;
        // Ball spawn position
        [SerializeField] private Vector3 ballSpawnPos;
        // List of hall available level
        [SerializeField] public LevelData[] levelDatas;
        
        // Public
        public static LevelManager Instance;
        
        // Getters
        public int ShotCount => _shotCount;


        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
        
        // Method to spawn level
        public void SpawnLevel(int levelIndex)
        {
            // Reset the timer every time the level is spawned
            TimerManager.Instance.ResetTimer();
            
            if (levelIndex >= levelDatas.Length) levelIndex = 0;
            // We instantiate the prefab level at required position
            Instantiate(levelDatas[levelIndex].levelPrefab, Vector3.zero, Quaternion.identity);
            // Set the available shots
            _shotCount = levelDatas[levelIndex].shotCount;
            // Display the number of remaining shots
            UIManager.Instance.ShotText.text = _shotCount.ToString();
            // Instantiate the ball at spawn point
            GameObject ball = Instantiate(ballPrefab, ballSpawnPos, Quaternion.identity);
            // Set the camera target
            CameraFollow.instance.SetTarget(ball);
            // Set the game status to playing
            GameManager.Instance.gameStatus = GameStatus.Playing;
            
            // Set the start time
            TimerManager.Instance.StartTimer();
        }
        

        // Method used to reduce shot
        public void ShotTaken()
        {
            // While the is still shot keep trying
            if (_shotCount <= 1)
            {
                LevelFailed();
                return;
            }
            // Reduce the remaining shot
            _shotCount--;
            // Update the UI
            UIManager.Instance.ShotText.text = "" + _shotCount;
        }

        // Method called when player failed the level
        public static void LevelFailed()
        {
            // If you are not playing you cant failed 
            if (GameManager.Instance.gameStatus != GameStatus.Playing )  return;
            // Otherwise you failed the level
            GameManager.Instance.gameStatus = GameStatus.Failed;
            // Display the game result
            UIManager.Instance.GameResult();
        }

        // Method called when player win the level
        public void LevelComplete()
        {
            // If you are not playing you cant win
            if (GameManager.Instance.gameStatus != GameStatus.Playing) return;
            // Otherwise you win the level
            // Checking if the current level is existing
            // Then pass to the next level
            if (GameManager.Instance.currentLevelIndex < levelDatas.Length) GameManager.Instance.currentLevelIndex++;
            // Otherwise restart to level 1
            else GameManager.Instance.currentLevelIndex = 0;
            // Change the game status to level completed
            GameManager.Instance.gameStatus = GameStatus.Complete;
            
            // Save the best score and timer
            int bestScore = PlayerPrefs.GetInt( "BestScore_" + GameManager.Instance.currentLevelIndex);
            float bestTimer = PlayerPrefs.GetFloat("BestTimer_" + GameManager.Instance.currentLevelIndex);
            
            if (bestScore == 0 || bestScore > _shotCount) PlayerPrefs.SetInt("BestScore_" + GameManager.Instance.currentLevelIndex, _shotCount);
            if (bestScore == 0f || bestTimer > TimerManager.Instance.elapsedTime) PlayerPrefs.SetFloat("BestTimer_" + GameManager.Instance.currentLevelIndex, TimerManager.Instance.elapsedTime);
            
            PlayerPrefs.Save();
            
            // Display the game result
            UIManager.Instance.GameResult();
        }
    }
}
