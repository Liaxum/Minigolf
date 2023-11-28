using UnityEngine;

/// Class which store level data
[System.Serializable]
public class LevelData
{
    // Private
    
    // Public
    // Maximum shot the player can take 
    public int shotCount;
    // Reference to the level prefab
    public GameObject levelPrefab;  //reference to level prefab
}
