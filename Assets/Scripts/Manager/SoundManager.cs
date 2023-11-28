using UnityEngine;

namespace Manager
{
    // Script to handle sound of the game
    public class SoundManager : MonoBehaviour
    {
        // Private
    
        // Public
        // Reference to the audio source which is used we will use for our FX
        public AudioSource fxSource;
        // Different fx audio clips
        public AudioClip gameOverFx, gameCompleteFx, shotFx;    //fx audio clips
    
        public static SoundManager Instance;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }
    
        // Method which plays the required sound fx
        public void PlayFx(FxTypes fxTypes)
        {
            switch (fxTypes)
            {
                // If its game over then play game over fx
                case FxTypes.GameOverFx:
                    fxSource.PlayOneShot(gameOverFx);
                    break;
                // If its game complete then play game complete fx
                case FxTypes.GameCompleteFx:
                    fxSource.PlayOneShot(gameCompleteFx);
                    break;
                // if its shot then play shot fx
                case FxTypes.ShotFx:
                    fxSource.PlayOneShot(shotFx);
                    break;
            }
        }
    }

// Enum to differ fx types
    public enum FxTypes
    {
        GameOverFx, 
        GameCompleteFx, 
        ShotFx
    }
}