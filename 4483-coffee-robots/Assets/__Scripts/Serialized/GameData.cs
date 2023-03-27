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
    public int highScore;
    public int winCount;

    public GameData()
    {
        difficultyLevel = Config.difficultyLevel;
        difficultyDamageMod = Config.difficultyDamageMod;
        difficultyMovementMod = Config.difficultyMovementMod;
        fieldOfView = Config.fieldOfView;
        highScore = Config.highScore;
        winCount = Config.winCount;
    }
}
