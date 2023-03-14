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
    public static float fieldOfView = 90f;

    [Header("Game Progress")]
    public static int gameStage = 0;
    public static float stage1Time;
    public static float stage2Time;
    public static float stage3Time;
    public static float stage4Time;
    public static int stage1Duration = 30;
    public static int stage2Duration = 60;
    public static int stage3Duration = 90;
    public static int stage4Duration = 120;
    public static float stage1SpawnDelay = 7.5f;
    public static float stage2SpawnDelay = 5f;
    public static float stage3SpawnDelay = 3f;
    public static float stage4SpawnDelay = 1.5f;
    public static bool coffeeMachineBuilt = false;

    [Header("Unlocks")]
    public static bool shotgunUnlocked = false;
    public static bool alternateARUnlocked = false;
    public static bool alternateMacheteUnlocked = false;
    public static bool alternateShotgunUnlocked = false;
    public static bool ar1a = false;
    public static bool ar1b = false;
    public static bool ar2a = false;
    public static bool ar2b = false;
    public static bool ar2c = false;
    public static bool ar3a = false;
    public static bool ar3b = false;
    public static bool ar3c = false;
    public static bool ar3d = false;
    public static bool ar4a = false;
    public static bool ar4b = false;
    public static bool ar4c = false;
    public static bool ar4d = false;
    public static bool mc1a = false;
    public static bool mc2a = false;
    public static bool mc2b = false;
    public static bool mc3a = false;
    public static bool mc3b = false;
    public static bool mc3c = false;
    public static bool mc4a = false;
    public static bool mc4b = false;
    public static bool mc4c = false;

    [Header("Player Movement")]
    public static float movementSpeed = 10f;
    public static float dashSpeed = 200f;
    public static float dashCooldown = 1f;

    [Header("Player Systems")]
    public static float playerMaxHp = 100;
    public static float abilityDuration = 5f;
    public static float abilityCooldown = 15f;
    public static float abilityDamageModifier = 0.5f;

    [Header("Weapons")]
    public static int stage1WeaponUpgradeTechCost = 25;
    public static int stage2WeaponUpgradeTechCost = 100;
    public static int stage3WeaponUpgradeTechCost = 300;
    public static int stage4WeaponUpgradeTechCost = 750;

    [Header("Assault Rifle")]
    public static float damageAR = 10;
    public static float fireRateAR = 10f;
    public static float rangeAR = 25;
    public static int maxAmmoAR = 150;
    public static float altDamageAR = 60;
    public static float altFireRateAR = 0.5f;
    public static float altRadiusAR = 4f;
    public static float altLaunchForceAR = 400f;
    public static int refillTechCostAR = 10;
    public static int unlockAltTechCostAR = 400;

    [Header("Machete")]
    public static float damageMachete = 80;
    public static float rateMachete = 1.5f;
    public static float rangeMachete = 4f;
    public static float altRateMachete = 0.6f;
    public static int unlockAltTechCostMachete = 200;

    [Header("Shotgun")]
    public static float damageShot = 16; // shoots 9 pellets
    public static float fireRateShot = 1f;
    public static float rangeShot = 10;
    public static int maxAmmoShot = 25;
    public static float altDamageShot = 4; // shoots 9 pellets in a 40 degree arc
    public static float altKnockDistanceShot = 1f;
    public static int refillTechCostShotgun = 15;
    public static int unlockTechCostShotgun = 250;
    public static int unlockAltTechCostShotgun = 1000;

    [Header("Buildings")]
    public static float buildingInteractDistance = 3f;
    public static float buildingCanvasDistance = 15f;
    public static int buildingFixScrapCost = 100;
    public static float buildingRetargetDelay = 1f;

    [Header("Coffee Plant")]
    public static float coffeePlantMaxHp = 10000;
    
    [Header("Machine Shop")]
    public static int machineShopScrapCost = 10;
    public static int repairElectronicsCost = 50;

    [Header("Armory")]
    public static int armoryScrapCost = 25;

    [Header("Fabricator")]
    public static int fabricatorScrapCost = 50;

    [Header("Coffee Machine")]
    public static int coffeeMachineScrapCost = 1000;

    [Header("Turret")]
    public static int turretMaxHp = 1000;
    public static int unlockTurretScrapCost = 50;
    public static int unlockTurretTechCost = 10;
    public static int countTurret = 0;
    public static int countTurret2 = 2;
    public static int countTurret3 = 3; // 4
    public static int countTurret4 = 4; // 6
    public static float damageTurret = 10;
    public static float attackRateTurret = 2f;
    public static float rangeTurret = 25f;

    [Header("Spikes")]
    public static int unlockSpikesScrapCost = 25;
    public static int countSpikes = 0;
    public static int countSpikes2 = 2; // 4
    public static int countSpikes3 = 4; // 7
    public static int countSpikes4 = 6; // 10
    public static float damageSpikes = 10;
    public static float attackRateSpikes = 1f;

    [Header("Enemy")]
    public static float loseInterestDistance = 15f;
    public static float activeKillMod = 2f;
    public static LayerMask standardSeeMask = LayerMask.GetMask("Player", "Building", "Ground");

    [Header("Sentinel")]
    public static float sentinelMaxHp = 100;
    public static float sentinelAttackRate = 1f;
    public static float sentinelDamage = 15;
    public static float sentinelRange = 1.2f;
    public static float sentinelMovementSpeed = 4.5f;
    public static int sentinelScrapReward = 25;
    public static int sentinelElectronicsReward = 10;
    public static int sentinelTechReward = 10;

    [Header("Payload")]
    public static float payloadMaxHp = 1000;
    public static float payloadDamage = 500;
    public static float payloadRange = 1f;
    public static float payloadMovementSpeed = 1f;
    public static int payloadScrapReward = 100;
    public static int payloadElectronicsReward = 15;
    public static int payloadTechReward = 50;

    [Header("Assassin")]
    public static float assassinMaxHp = 150;
    public static float assassinAttackRate = 0.25f;
    public static float assassinDamage = 25;
    public static float assassinMinRange = 5f;
    public static float assassinMaxRange = 25f;
    public static float assassinMovementSpeed = 6f;
    public static float assassinProjectileSpeed = 500;
    public static int assassinScrapReward = 75;
    public static int assassinElectronicsReward = 25;
    public static int assassinTechReward = 25;

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

    public static void ResetConfigValues()
    {
        Config.gameStage = 0;
        Config.coffeeMachineBuilt = false;
        Config.shotgunUnlocked = false;
        Config.alternateARUnlocked = false;
        Config.alternateMacheteUnlocked = false;
        Config.alternateShotgunUnlocked = false;
        Config.ar1a = false;
        Config.ar1b = false;
        Config.ar2a = false;
        Config.ar2b = false;
        Config.ar2c = false;
        Config.ar3a = false;
        Config.ar3b = false;
        Config.ar3c = false;
        Config.ar3d = false;
        Config.ar4a = false;
        Config.ar4b = false;
        Config.ar4c = false;
        Config.ar4d = false;
        Config.mc1a = false;
        Config.mc2a = false;
        Config.mc2b = false;
        Config.mc3a = false;
        Config.mc3b = false;
        Config.mc3c = false;
        Config.mc4a = false;
        Config.mc4b = false;
        Config.mc4c = false;

        Config.movementSpeed = 10f;
        Config.dashSpeed = 200f;
        Config.dashCooldown = 1f;
        Config.playerMaxHp = 100;
        Config.abilityDuration = 5f;
        Config.abilityCooldown = 15f;
        Config.abilityDamageModifier = 0.5f;
        Config.damageAR = 10;
        Config.fireRateAR = 10f;
        Config.rangeAR = 25;
        Config.maxAmmoAR = 150;
        Config.altDamageAR = 60;
        Config.altFireRateAR = 0.5f;
        Config.altRadiusAR = 4f;
        Config.altLaunchForceAR = 400f;
        Config.refillTechCostAR = 10;
        Config.damageMachete = 80;
        Config.rateMachete = 1.5f;
        Config.rangeMachete = 4f;
        Config.altRateMachete = 0.6f;
        Config.damageShot = 16;
        Config.fireRateShot = 1f;
        Config.rangeShot = 10;
        Config.maxAmmoShot = 25;
        Config.altDamageShot = 4;
        Config.altKnockDistanceShot = 1f;
        Config.refillTechCostShotgun = 15;
        Config.turretMaxHp = 1000;
        Config.countTurret = 0;
        Config.damageTurret = 10;
        Config.attackRateTurret = 2f;
        Config.rangeTurret = 25f;
        Config.countSpikes = 0;
        Config.damageSpikes = 10;
        Config.attackRateSpikes = 1f;
        Config.activeKillMod = 2f;
    }
}

public interface IEnemy {
    void DamageEnemy(float damage, string source = "");
}

public interface IBuilding {
    void DamageBuilding(float damage, string source = "");
}

public interface IDefense {
    void FixBuilding();
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
