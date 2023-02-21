using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Config
{
    [Header("Settings")]
    public static Difficulty difficultyLevel = Difficulty.Normal;
    public static float difficultyDamageMod = 1f;
    public static float difficultyMovementMod = 1f;
    public static float fieldOfView = 60f;

    [Header("Game Progress")]
    public static int gameStage = 0;
    public static float stage1Time;
    public static float stage2Time;
    public static float stage3Time;
    public static float stage4Time;
    public static int stage1Duration = 10;
    public static int stage2Duration = 10;
    public static int stage3Duration = 10;
    public static int stage4Duration = 10;
    public static float stage1SpawnDelay = 10f;
    public static float stage2SpawnDelay = 3f;
    public static float stage3SpawnDelay = 0.75f;
    public static float stage4SpawnDelay = 0.25f;

    [Header("Unlocks")]
    public static bool shotgunUnlocked = false;
    public static bool alternateARUnlocked = false;
    public static bool alternateMacheteUnlocked = false;
    public static bool alternateShotgunUnlocked = false;

    [Header("Player Movement")]
    public static float movementSpeed = 10f;
    public static float dashSpeed = 100f;
    public static float dashCooldown = 1f;

    [Header("Player Systems")]
    public static float playerMaxHp = 100;
    public static float abilityDuration = 5f;
    public static float abilityCooldown = 20f;
    public static float abilityDamageModifier = 0.5f;

    [Header("Assault Rifle")]
    public static float damageAR = 10;
    public static float fireRateAR = 10f;
    public static float rangeAR = 20;
    public static int maxAmmoAR = 150;
    public static float altDamageAR = 15;
    public static float altFireRateAR = 0.5f;
    public static int refillTechCostAR = 10;

    [Header("Machete")]
    public static float damageMachete = 80;
    public static float rateMachete = 1.5f;
    public static float rangeMachete = 1.5f;
    public static float altRateMachete = 0.6f;

    [Header("Shotgun")]
    public static float damageShot = 16; // shoots 9 pellets
    public static float fireRateShot = 1f;
    public static float rangeShot = 5;
    public static int maxAmmoShot = 25;
    public static float altDamageShot = 30;
    public static float altKnockForceShot = 100;
    public static float altFireRateShot = 0.8f;
    public static int refillTechCostShotgun = 15;
    public static int unlockTechCostShotgun = 250;

    [Header("Buildings")]
    public static float buildingInteractDistance = 3f;
    public static float buildingCanvasDistance = 15f;

    [Header("Coffee Plant")]
    public static float coffeePlantMaxHp = 10000;
    
    [Header("Machine Shop")]
    public static int machineShopScrapCost = 10;
    public static int repairElectronicsCost = 50;

    [Header("Armory")]
    public static int armoryScrapCost = 25;

    [Header("Fabricator")]
    public static int fabricatorScrapCost = 50;

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

interface IEnemy {
    void DamageEnemy(float damage, string source = "");
}

interface IBuilding {
    void DamageBuilding(float damage, string source = "");
}

public enum BuildStatus {
    Locked = 0,
    Unlocked = 1,
    Built = 2,
    Damaged = 3
}

public enum Difficulty {
    Easy = 0,
    Normal = 1,
    Hard = 2
}
