using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Script to control game UI
namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        // Private
        // Reference to power bar
        [SerializeField] private Image powerBar;
        // Reference to remaining shot count
        [SerializeField] private Text shotText;
        // Important GameObject
        [SerializeField] private GameObject mainMenu, gameMenu, gameOverPanel, retryBtn, nextBtn;
        [SerializeField] private GameObject container, lvlBtnPrefab;
    
        // public
        public static UIManager Instance;
    
        // Getters
        public Text ShotText => shotText;
        public Image PowerBar => powerBar;
    
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            // Set the power bar to zero
            powerBar.fillAmount = 0;
        }

        private void Start()
        {
            switch (GameManager.Instance.gameStatus)
            {
                // if the game doesn't have a status
                // Then got to the main menu
                case GameStatus.None:
                    // Create level button
                    CreateLevelButtons();
                    break;
                // If the game is failed or completed
                // Then deactivate the main menu and activate the game menu
                // Spawn the selected level
                case GameStatus.Failed:
                case GameStatus.Complete:
                    mainMenu.SetActive(false);
                    gameMenu.SetActive(true);
                    LevelManager.Instance.SpawnLevel(GameManager.Instance.currentLevelIndex);
                    break;
            }
        }

        // Method which spawn levels button
        private void CreateLevelButtons()
        {
            // For all the level create a button
            for (int i = 0; i < LevelManager.Instance.levelDatas.Length; i++)
            {
                // Instantiate a button prefab for the current level
                GameObject buttonObj = Instantiate(lvlBtnPrefab, container.transform);
                buttonObj.transform.GetChild(0).GetComponent<Text>().text = "" + (i + 1);
                // Get the reference to the button
                Button button = buttonObj.GetComponent<Button>();
                // Add a Listener to the button
                button.onClick.AddListener(() => OnClick(button));                          //add listner to button
            }
        }

        // Method called when we click on button
        private void OnClick(Button btn)
        {
            // Deactivate the menu
            mainMenu.SetActive(false);
            // Activate the menu
            gameMenu.SetActive(true);
            // Set the current level equal to the current sibling index button
            GameManager.Instance.currentLevelIndex = btn.transform.GetSiblingIndex();
            // Spawn the level
            LevelManager.Instance.SpawnLevel(GameManager.Instance.currentLevelIndex);      //spawn level
        }

        // Method call after level fail or win
        public void GameResult()
        {
        
            switch (GameManager.Instance.gameStatus)
            {
                // If the game is win
                // Activate the game finish panel and activate the next button
                // And play the finish sound
                case GameStatus.Complete:
                    gameOverPanel.SetActive(true);
                    nextBtn.SetActive(true);
                    SoundManager.Instance.PlayFx(FxTypes.GameCompleteFx);
                    break;
                // If the game is failed
                // Activate the game finish panel and activate the retry button
                // And play the failed sound
                case GameStatus.Failed:
                    gameOverPanel.SetActive(true);
                    retryBtn.SetActive(true);
                    SoundManager.Instance.PlayFx(FxTypes.GameOverFx);
                    break;
            }
        }

        // Method to go to main menu
        public void HomeBtn()
        {
            GameManager.Instance.gameStatus = GameStatus.None;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Method to reload scene
        public void NextRetryBtn()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


    }
}
