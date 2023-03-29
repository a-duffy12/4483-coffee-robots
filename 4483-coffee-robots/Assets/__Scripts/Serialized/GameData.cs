using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [Header("Settings")]
    public Difficulty difficultyLevel;
    public float difficultyDamageMod;
    public float difficultyMovementMod;
    public float fieldOfView;
    public bool showFps;
    public bool showEnemyCount;
    public bool showTutorials;
    public int highScore;
    public int winCount;

    public GameData()
    {
        difficultyLevel = Config.difficultyLevel;
        difficultyDamageMod = Config.difficultyDamageMod;
        difficultyMovementMod = Config.difficultyMovementMod;
        fieldOfView = Config.fieldOfView;
        showFps = Config.showFps;
        showEnemyCount = Config.showEnemyCount;
        showTutorials = Config.showTutorials;
        highScore = Config.highScore;
        winCount = Config.winCount;
    }
}
