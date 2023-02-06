using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Config
{
    [Header("Settings")]
    public static int difficultyLevel = 1;
    public static float difficultyDamageMod = 1f;
    public static float difficultyMovementMod = 1f;
    public static float fieldOfView = 60f;


    [Header("Player Movement")]
    public static float movementSpeed = 10f;
    public static float dashSpeed = 100f;
    public static float dashCooldown = 1f;


    [Header("Player Systems")]
    public static float playerMaxHp = 100;
    public static float abilityDuration = 5f;
    public static float abilityCooldown = 20f;
    public static float abilityDamageModifier = 0.5f;

    public static void GetSaveData()
    {
        GameData savedData = SaveLoad.LoadData(); // load save file

        if (savedData != null)
        {
            Config.difficultyLevel = savedData.difficultyLevel;
            Config.difficultyDamageMod = savedData.difficultyDamageMod;
            Config.difficultyMovementMod = savedData.difficultyMovementMod;
            Config.fieldOfView = savedData.fieldOfView;
        }
    }
}
