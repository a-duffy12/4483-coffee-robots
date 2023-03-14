using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Armory : MonoBehaviour, IBuilding
{
    [Header("GameObjects")]
    [SerializeField] private GameObject unbuiltPrefab;
    [SerializeField] private GameObject builtPrefab;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject arPanel;
    [SerializeField] private GameObject machetePanel;
    [SerializeField] private GameObject shotgunPanel;
    [SerializeField] private Button shotgunButton;
    [SerializeField] private GameObject arAltDesc;
    [SerializeField] private Button arAltButton;
    [SerializeField] private GameObject macheteAltDesc;
    [SerializeField] private Button macheteAltButton;
    [SerializeField] private GameObject shotgunAltDesc;
    [SerializeField] private Button shotgunAltButton;

    [Header("AR Upgrades")]
    [SerializeField] private Button ar1aButton;
    [SerializeField] private Button ar1bButton;
    [SerializeField] private Button ar2aButton;
    [SerializeField] private Button ar2bButton;
    [SerializeField] private Button ar2cButton;
    [SerializeField] private Button ar3aButton;
    [SerializeField] private Button ar3bButton;
    [SerializeField] private Button ar3cButton;
    [SerializeField] private Button ar3dButton;
    [SerializeField] private Button ar4aButton;
    [SerializeField] private Button ar4bButton;
    [SerializeField] private Button ar4cButton;
    [SerializeField] private Button ar4dButton;

    [Header("Machete Upgrades")]
    [SerializeField] private Button mc1aButton;
    [SerializeField] private Button mc2aButton;
    [SerializeField] private Button mc2bButton;
    [SerializeField] private Button mc3aButton;
    [SerializeField] private Button mc3bButton;
    [SerializeField] private Button mc3cButton;
    [SerializeField] private Button mc4aButton;
    [SerializeField] private Button mc4bButton;
    [SerializeField] private Button mc4cButton;

    [Header("Shotgun Upgrades")]
    [SerializeField] private Button sh2aButton;
    [SerializeField] private Button sh2bButton;
    [SerializeField] private Button sh3aButton;
    [SerializeField] private Button sh3bButton;
    [SerializeField] private Button sh3cButton;
    [SerializeField] private Button sh4aButton;
    [SerializeField] private Button sh4bButton;
    [SerializeField] private Button sh4cButton;
    [SerializeField] private Button sh4dButton;
    [SerializeField] private Button sh4eButton;

    GameObject player;
    PlayerInventory inventory;
    PlayerInput input;
    private BuildStatus buildStatus = BuildStatus.Locked;
    private bool interact;
    private bool quickInteract;
    private int refillCost;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<PlayerInventory>();
        input = player.GetComponent<PlayerInput>();
    }
    
    void Start()
    {
        unbuiltPrefab.SetActive(false);
        builtPrefab.SetActive(false);
        menu.SetActive(false);

        TMP_Text text = shotgunButton.GetComponentInChildren<TMP_Text>();
        text.text = $"Unlock {Config.unlockTechCostShotgun} <sprite=0>";
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= Config.buildingInteractDistance)
        {
            if (buildStatus == BuildStatus.Unlocked && Config.gameStage > 0 && Config.gameStage < 5)
            {
                promptText.text = $"Build Armory {Config.armoryScrapCost} <sprite=2> [E]";

                if (quickInteract)
                {
                    Build();
                }
            }
            else if (buildStatus == BuildStatus.Built)
            {
                refillCost = CalculateRefillCost(inventory);
                if (refillCost > 0)
                {
                    promptText.text = $"Refill {refillCost} <sprite=0> [E]\n\nOpen Armory [F]";

                    if (quickInteract && PlayerInventory.tech >= refillCost)
                    {
                        RefillAmmo(refillCost);
                    }
                }
                else
                {
                    promptText.text = "Open Armory [F]";
                }

                if (interact)
                {
                    OpenMenu();
                }
            }
            else if (promptText.text.StartsWith("Build Armory") || promptText.text.StartsWith("Refill") || promptText.text.StartsWith("Open Armory"))
            {
                promptText.text = ""; // only sets to empty if player just left text radius
            }
        }
        else if (promptText.text.StartsWith("Build Armory") || promptText.text.StartsWith("Refill") || promptText.text.StartsWith("Open Armory"))
        {
            promptText.text = ""; // only sets to empty if player just left text radius
        }

        quickInteract = false;
        interact = false;
    }

    public void Unlock()
    {
        unbuiltPrefab.SetActive(true);
        buildStatus = BuildStatus.Unlocked;
    }

    void Build()
    {
        if (PlayerInventory.scrap >= Config.armoryScrapCost)
        {
            unbuiltPrefab.SetActive(false);
            builtPrefab.SetActive(true);
            buildStatus = BuildStatus.Built;
            PlayerInventory.scrap -= Config.armoryScrapCost;
        }
    }

    void RefillAmmo(int cost)
    {
        inventory.ar.currentAmmo = Config.maxAmmoAR;
        if (Config.shotgunUnlocked)
        {
            inventory.shotgun.currentAmmo = Config.maxAmmoShot;
        }
        PlayerInventory.tech -= cost;
    }

    void OpenMenu()
    {
        input.SwitchCurrentActionMap("Menu");
        menu.SetActive(true);
        arPanel.SetActive(true);
        machetePanel.SetActive(false);
        shotgunPanel.SetActive(false);

        if (Config.gameStage <= 1)
        {
            shotgunButton.gameObject.SetActive(false);
            arAltDesc.SetActive(false);
            arAltButton.gameObject.SetActive(false);
            macheteAltDesc.SetActive(false);
            macheteAltButton.gameObject.SetActive(false);
            shotgunAltDesc.SetActive(false);
            shotgunAltButton.gameObject.SetActive(false);
            ar1aButton.gameObject.SetActive(true);
            ar1bButton.gameObject.SetActive(true);
            ar2aButton.gameObject.SetActive(false);
            ar2bButton.gameObject.SetActive(false);
            ar2cButton.gameObject.SetActive(false);
            ar3aButton.gameObject.SetActive(false);
            ar3bButton.gameObject.SetActive(false);
            ar3cButton.gameObject.SetActive(false);
            ar3dButton.gameObject.SetActive(false);
            ar4aButton.gameObject.SetActive(false);
            ar4bButton.gameObject.SetActive(false);
            ar4cButton.gameObject.SetActive(false);
            ar4dButton.gameObject.SetActive(false);
            mc1aButton.gameObject.SetActive(true);
            mc2aButton.gameObject.SetActive(false);
            mc2bButton.gameObject.SetActive(false);
            mc3aButton.gameObject.SetActive(false);
            mc3bButton.gameObject.SetActive(false);
            mc3cButton.gameObject.SetActive(false);
            mc4aButton.gameObject.SetActive(false);
            mc4bButton.gameObject.SetActive(false);
            mc4cButton.gameObject.SetActive(false);
            sh2aButton.gameObject.SetActive(false);
            sh2bButton.gameObject.SetActive(false);
            sh3aButton.gameObject.SetActive(false);
            sh3bButton.gameObject.SetActive(false);
            sh3cButton.gameObject.SetActive(false);
            sh4aButton.gameObject.SetActive(false);
            sh4bButton.gameObject.SetActive(false);
            sh4cButton.gameObject.SetActive(false);
            sh4dButton.gameObject.SetActive(false);
            sh4eButton.gameObject.SetActive(false);
        }
        else if (Config.gameStage == 2)
        {
            shotgunButton.gameObject.SetActive(true);
            arAltDesc.SetActive(true);
            arAltButton.gameObject.SetActive(true);
            macheteAltDesc.SetActive(true);
            macheteAltButton.gameObject.SetActive(true);
            shotgunAltDesc.SetActive(false);
            shotgunAltButton.gameObject.SetActive(false);
            ar1aButton.gameObject.SetActive(true);
            ar1bButton.gameObject.SetActive(true);
            ar2aButton.gameObject.SetActive(true);
            ar2bButton.gameObject.SetActive(true);
            ar2cButton.gameObject.SetActive(true);
            ar3aButton.gameObject.SetActive(false);
            ar3bButton.gameObject.SetActive(false);
            ar3cButton.gameObject.SetActive(false);
            ar3dButton.gameObject.SetActive(false);
            ar4aButton.gameObject.SetActive(false);
            ar4bButton.gameObject.SetActive(false);
            ar4cButton.gameObject.SetActive(false);
            ar4dButton.gameObject.SetActive(false);
            mc1aButton.gameObject.SetActive(true);
            mc2aButton.gameObject.SetActive(true);
            mc2bButton.gameObject.SetActive(true);
            mc3aButton.gameObject.SetActive(false);
            mc3bButton.gameObject.SetActive(false);
            mc3cButton.gameObject.SetActive(false);
            mc4aButton.gameObject.SetActive(false);
            mc4bButton.gameObject.SetActive(false);
            mc4cButton.gameObject.SetActive(false);
            sh2aButton.gameObject.SetActive(true);
            sh2bButton.gameObject.SetActive(true);
            sh3aButton.gameObject.SetActive(false);
            sh3bButton.gameObject.SetActive(false);
            sh3cButton.gameObject.SetActive(false);
            sh4aButton.gameObject.SetActive(false);
            sh4bButton.gameObject.SetActive(false);
            sh4cButton.gameObject.SetActive(false);
            sh4dButton.gameObject.SetActive(false);
            sh4eButton.gameObject.SetActive(false);
        }
        else if (Config.gameStage == 3)
        {
            shotgunButton.gameObject.SetActive(true);
            arAltDesc.SetActive(true);
            arAltButton.gameObject.SetActive(true);
            macheteAltDesc.SetActive(true);
            macheteAltButton.gameObject.SetActive(true);
            shotgunAltDesc.SetActive(true);
            shotgunAltButton.gameObject.SetActive(true);
            ar1aButton.gameObject.SetActive(true);
            ar1bButton.gameObject.SetActive(true);
            ar2aButton.gameObject.SetActive(true);
            ar2bButton.gameObject.SetActive(true);
            ar2cButton.gameObject.SetActive(true);
            ar3aButton.gameObject.SetActive(true);
            ar3bButton.gameObject.SetActive(true);
            ar3cButton.gameObject.SetActive(true);
            ar3dButton.gameObject.SetActive(true);
            ar4aButton.gameObject.SetActive(false);
            ar4bButton.gameObject.SetActive(false);
            ar4cButton.gameObject.SetActive(false);
            ar4dButton.gameObject.SetActive(false);
            mc1aButton.gameObject.SetActive(true);
            mc2aButton.gameObject.SetActive(true);
            mc2bButton.gameObject.SetActive(true);
            mc3aButton.gameObject.SetActive(true);
            mc3bButton.gameObject.SetActive(true);
            mc3cButton.gameObject.SetActive(true);
            mc4aButton.gameObject.SetActive(false);
            mc4bButton.gameObject.SetActive(false);
            mc4cButton.gameObject.SetActive(false);
            sh2aButton.gameObject.SetActive(true);
            sh2bButton.gameObject.SetActive(true);
            sh3aButton.gameObject.SetActive(true);
            sh3bButton.gameObject.SetActive(true);
            sh3cButton.gameObject.SetActive(true);
            sh4aButton.gameObject.SetActive(false);
            sh4bButton.gameObject.SetActive(false);
            sh4cButton.gameObject.SetActive(false);
            sh4dButton.gameObject.SetActive(false);
            sh4eButton.gameObject.SetActive(false);
        }
        else if (Config.gameStage >= 4)
        {
            shotgunButton.gameObject.SetActive(true);
            arAltDesc.SetActive(true);
            arAltButton.gameObject.SetActive(true);
            macheteAltDesc.SetActive(true);
            macheteAltButton.gameObject.SetActive(true);
            shotgunAltDesc.SetActive(true);
            shotgunAltButton.gameObject.SetActive(true);
            ar1aButton.gameObject.SetActive(true);
            ar1bButton.gameObject.SetActive(true);
            ar2aButton.gameObject.SetActive(true);
            ar2bButton.gameObject.SetActive(true);
            ar2cButton.gameObject.SetActive(true);
            ar3aButton.gameObject.SetActive(true);
            ar3bButton.gameObject.SetActive(true);
            ar3cButton.gameObject.SetActive(true);
            ar3dButton.gameObject.SetActive(true);
            ar4aButton.gameObject.SetActive(true);
            ar4bButton.gameObject.SetActive(true);
            ar4cButton.gameObject.SetActive(true);
            ar4dButton.gameObject.SetActive(true);
            mc1aButton.gameObject.SetActive(true);
            mc2aButton.gameObject.SetActive(true);
            mc2bButton.gameObject.SetActive(true);
            mc3aButton.gameObject.SetActive(true);
            mc3bButton.gameObject.SetActive(true);
            mc3cButton.gameObject.SetActive(true);
            mc4aButton.gameObject.SetActive(true);
            mc4bButton.gameObject.SetActive(true);
            mc4cButton.gameObject.SetActive(true);
            sh2aButton.gameObject.SetActive(true);
            sh2bButton.gameObject.SetActive(true);
            sh3aButton.gameObject.SetActive(true);
            sh3bButton.gameObject.SetActive(true);
            sh3cButton.gameObject.SetActive(true);
            sh4aButton.gameObject.SetActive(true);
            sh4bButton.gameObject.SetActive(true);
            sh4cButton.gameObject.SetActive(true);
            sh4dButton.gameObject.SetActive(true);
            sh4eButton.gameObject.SetActive(true);
        }

        SetButtonTextValues();
    }

    public void CloseMenu()
    {
        input.SwitchCurrentActionMap("Player");
        menu.SetActive(false);
    }

    public void ARPanel()
    {
        if (menu.activeSelf)
        {
            arPanel.SetActive(true);
            machetePanel.SetActive(false);
            shotgunPanel.SetActive(false);
        }
    }

    public void MachetePanel()
    {
        if (menu.activeSelf)
        {
            arPanel.SetActive(false);
            machetePanel.SetActive(true);
            shotgunPanel.SetActive(false);
        }
    }

    public void ShotgunPanel()
    {
        if (menu.activeSelf && Config.shotgunUnlocked)
        {
            arPanel.SetActive(false);
            machetePanel.SetActive(false);
            shotgunPanel.SetActive(true);
        }
        else if (menu.activeSelf && !Config.shotgunUnlocked && PlayerInventory.tech >= Config.unlockTechCostShotgun)
        {
            PlayerInventory.tech -= Config.unlockTechCostShotgun;
            Config.shotgunUnlocked = true;
            arPanel.SetActive(false);
            machetePanel.SetActive(false);
            shotgunPanel.SetActive(true);

            TMP_Text text = shotgunButton.GetComponentInChildren<TMP_Text>();
            text.text = "Shotgun";
        }
    }

    public void ARAlternate()
    {
        if (!Config.alternateARUnlocked && PlayerInventory.tech >= Config.unlockAltTechCostAR)
        {
            PlayerInventory.tech -= Config.unlockAltTechCostAR;
            Config.alternateARUnlocked = true;
            SetButtonTextValues();
        }
    }

    public void MacheteAlternate()
    {
        if (!Config.alternateMacheteUnlocked && PlayerInventory.tech >= Config.unlockAltTechCostMachete)
        {
            PlayerInventory.tech -= Config.unlockAltTechCostMachete;
            Config.alternateMacheteUnlocked = true;
            SetButtonTextValues();
        }
    }

    public void ShotgunAlternate()
    {
        if (!Config.alternateShotgunUnlocked && PlayerInventory.tech >= Config.unlockAltTechCostShotgun)
        {
            PlayerInventory.tech -= Config.unlockAltTechCostShotgun;
            Config.alternateShotgunUnlocked = true;
            SetButtonTextValues();
        }
    }

    #region AR upgrades

    public void AR1A()
    {
        if (!Config.ar1a && PlayerInventory.tech >= Config.stage1WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage1WeaponUpgradeTechCost;
            Config.ar1a = true;
            Config.maxAmmoAR = 165;
            SetButtonTextValues();
        }
    }

    public void AR1B()
    {
        if (!Config.ar1b && PlayerInventory.tech >= Config.stage1WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage1WeaponUpgradeTechCost;
            Config.ar1b = true;
            Config.damageAR = 11;
            SetButtonTextValues();
        }
    }

    public void AR2A()
    {
        if (!Config.ar2a && PlayerInventory.tech >= Config.stage2WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage2WeaponUpgradeTechCost;
            Config.ar2a = true;
            Config.maxAmmoAR = 185;
            SetButtonTextValues();
        }
    }

    public void AR2B()
    {
        if (!Config.ar2b && PlayerInventory.tech >= Config.stage2WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage2WeaponUpgradeTechCost;
            Config.ar2b = true;
            Config.damageAR = 12;
            SetButtonTextValues();
        }
    }

    public void AR2C()
    {
        if (!Config.ar2c && PlayerInventory.tech >= Config.stage2WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage2WeaponUpgradeTechCost;
            Config.ar2c = true;
            Config.altDamageAR = 80;
            SetButtonTextValues();
        }
    }

    public void AR3A()
    {
        if (!Config.ar3a && PlayerInventory.tech >= Config.stage3WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage3WeaponUpgradeTechCost;
            Config.ar3a = true;
            Config.maxAmmoAR = 210;
            SetButtonTextValues();
        }
    }

    public void AR3B()
    {
        if (!Config.ar3b && PlayerInventory.tech >= Config.stage3WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage3WeaponUpgradeTechCost;
            Config.ar3b = true;
            Config.damageAR = 13;
            SetButtonTextValues();
        }
    }

    public void AR3C()
    {
        if (!Config.ar3c && PlayerInventory.tech >= Config.stage3WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage3WeaponUpgradeTechCost;
            Config.ar3c = true;
            Config.altDamageAR = 100;
            SetButtonTextValues();
        }
    }

    public void AR3D()
    {
        if (!Config.ar3d && PlayerInventory.tech >= Config.stage3WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage3WeaponUpgradeTechCost;
            Config.ar3d = true;
            Config.altRadiusAR = 5;
            SetButtonTextValues();
        }
    }

    public void AR4A()
    {
        if (!Config.ar4a && PlayerInventory.tech >= Config.stage4WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage4WeaponUpgradeTechCost;
            Config.ar4a = true;
            Config.maxAmmoAR = 240;
            SetButtonTextValues();
        }
    }

    public void AR4B()
    {
        if (!Config.ar4b && PlayerInventory.tech >= Config.stage4WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage4WeaponUpgradeTechCost;
            Config.ar4b = true;
            Config.damageAR = 15;
            SetButtonTextValues();
        }
    }

    public void AR4C()
    {
        if (!Config.ar4c && PlayerInventory.tech >= Config.stage4WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage4WeaponUpgradeTechCost;
            Config.ar4c = true;
            Config.altDamageAR = 125;
            SetButtonTextValues();
        }
    }

    public void AR4D()
    {
        if (!Config.ar4d && PlayerInventory.tech >= Config.stage4WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage4WeaponUpgradeTechCost;
            Config.ar4d = true;
            Config.altRadiusAR = 6;
            SetButtonTextValues();
        }
    }

    #endregion AR upgrades

    #region MC upgrades

    public void MC1A()
    {
        if (!Config.mc1a && PlayerInventory.tech >= Config.stage1WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage1WeaponUpgradeTechCost;
            Config.mc1a = true;
            Config.damageMachete = 90;
            SetButtonTextValues();
        }
    }

    public void MC2A()
    {
        if (!Config.mc2a && PlayerInventory.tech >= Config.stage2WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage2WeaponUpgradeTechCost;
            Config.mc2a = true;
            Config.damageMachete = 100;
            SetButtonTextValues();
        }
    }

    public void MC2B()
    {
        if (!Config.mc2b && PlayerInventory.tech >= Config.stage2WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage2WeaponUpgradeTechCost;
            Config.mc2b = true;
            Config.rateMachete = 1.7f;
            SetButtonTextValues();
        }
    }

    public void MC3A()
    {
        if (!Config.mc3a && PlayerInventory.tech >= Config.stage3WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage3WeaponUpgradeTechCost;
            Config.mc3a = true;
            Config.damageMachete = 110;
            SetButtonTextValues();
        }
    }

    public void MC3B()
    {
        if (!Config.mc3b && PlayerInventory.tech >= Config.stage3WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage3WeaponUpgradeTechCost;
            Config.mc3b = true;
            Config.rateMachete = 1.9f;
            SetButtonTextValues();
        }
    }

    public void MC3C()
    {
        if (!Config.mc3c && PlayerInventory.tech >= Config.stage3WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage3WeaponUpgradeTechCost;
            Config.mc3c = true;
            Config.altRateMachete = 0.7f;
            SetButtonTextValues();
        }
    }

    public void MC4A()
    {
        if (!Config.mc4a && PlayerInventory.tech >= Config.stage4WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage4WeaponUpgradeTechCost;
            Config.mc4a = true;
            Config.damageMachete = 125;
            SetButtonTextValues();
        }
    }

    public void MC4B()
    {
        if (!Config.mc4b && PlayerInventory.tech >= Config.stage4WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage4WeaponUpgradeTechCost;
            Config.mc4b = true;
            Config.rateMachete = 2.1f;
            SetButtonTextValues();
        }
    }

    public void MC4C()
    {
        if (!Config.mc4c && PlayerInventory.tech >= Config.stage4WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage4WeaponUpgradeTechCost;
            Config.mc4c = true;
            Config.altRateMachete = 0.8f;
            SetButtonTextValues();
        }
    }

    #endregion MC upgrades
    
    #region SH upgrades
    
    public void SH2A()
    {
        if (Config.shotgunUnlocked && !Config.sh2a && PlayerInventory.tech >= Config.stage2WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage2WeaponUpgradeTechCost;
            Config.sh2a = true;
            Config.maxAmmoShot = 28;
            SetButtonTextValues();
        }
    }

    public void SH2B()
    {
        if (Config.shotgunUnlocked && !Config.sh2b && PlayerInventory.tech >= Config.stage2WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage2WeaponUpgradeTechCost;
            Config.sh2b = true;
            Config.damageShot = 17;
            SetButtonTextValues();
        }
    }

    public void SH3A()
    {
        if (Config.shotgunUnlocked && !Config.sh3a && PlayerInventory.tech >= Config.stage3WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage3WeaponUpgradeTechCost;
            Config.sh3a = true;
            Config.maxAmmoShot = 32;
            SetButtonTextValues();
        }
    }

    public void SH3B()
    {
        if (Config.shotgunUnlocked && !Config.sh3b && PlayerInventory.tech >= Config.stage3WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage3WeaponUpgradeTechCost;
            Config.sh3b = true;
            Config.damageShot = 18;
            SetButtonTextValues();
        }
    }

    public void SH3C()
    {
        if (Config.shotgunUnlocked && !Config.sh3c && PlayerInventory.tech >= Config.stage3WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage3WeaponUpgradeTechCost;
            Config.sh3c = true;
            Config.fireRateShot = 1.1f;
            SetButtonTextValues();
        }
    }

    public void SH4A()
    {
        if (Config.shotgunUnlocked && !Config.sh4a && PlayerInventory.tech >= Config.stage4WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage4WeaponUpgradeTechCost;
            Config.sh4a = true;
            Config.maxAmmoShot = 36;
            SetButtonTextValues();
        }
    }

    public void SH4B()
    {
        if (Config.shotgunUnlocked && !Config.sh4b && PlayerInventory.tech >= Config.stage4WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage4WeaponUpgradeTechCost;
            Config.sh4b = true;
            Config.damageShot = 20;
            SetButtonTextValues();
        }
    }

    public void SH4C()
    {
        if (Config.shotgunUnlocked && !Config.sh4c && PlayerInventory.tech >= Config.stage4WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage4WeaponUpgradeTechCost;
            Config.sh4c = true;
            Config.fireRateShot = 1.2f;
            SetButtonTextValues();
        }
    }

    public void SH4D()
    {
        if (Config.shotgunUnlocked && !Config.sh4d && PlayerInventory.tech >= Config.stage4WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage4WeaponUpgradeTechCost;
            Config.sh4d = true;
            Config.rangeShot = 15;
            SetButtonTextValues();
        }
    }

    public void SH4E()
    {
        if (Config.shotgunUnlocked && !Config.sh4e && PlayerInventory.tech >= Config.stage4WeaponUpgradeTechCost)
        {
            PlayerInventory.tech -= Config.stage4WeaponUpgradeTechCost;
            Config.sh4e = true;
            Config.altKnockDistanceShot = 1.1f;
            SetButtonTextValues();
        }
    }
    
    #endregion SH upgrades
    
    void SetButtonTextValues()
    {
        if (Config.alternateARUnlocked)
        {
            TMP_Text text = arAltButton.GetComponentInChildren<TMP_Text>();
            text.text = "Unlocked";
        }
        else
        {
            TMP_Text text = arAltButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Unlock {Config.unlockAltTechCostAR} <sprite=0>";
        }

        if (Config.alternateMacheteUnlocked)
        {
            TMP_Text text = macheteAltButton.GetComponentInChildren<TMP_Text>();
            text.text = "Unlocked";
        }
        else
        {
            TMP_Text text = macheteAltButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Unlock {Config.unlockAltTechCostMachete} <sprite=0>";
        }

        if (Config.alternateShotgunUnlocked)
        {
            TMP_Text text = shotgunAltButton.GetComponentInChildren<TMP_Text>();
            text.text = "Unlocked";
        }
        else
        {
            TMP_Text text = shotgunAltButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Unlock {Config.unlockAltTechCostShotgun} <sprite=0>";
        }

        // Assault Rifle

        if (Config.ar1a)
        {
            TMP_Text text = ar1aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo I\nUnlocked";
        }
        else
        {
            TMP_Text text = ar1aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo I\nUnlock {Config.stage1WeaponUpgradeTechCost} <sprite=0>";
        }

        if (Config.ar1b)
        {
            TMP_Text text = ar1bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage I\nUnlocked";
        }
        else
        {
            TMP_Text text = ar1bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage I\nUnlock {Config.stage1WeaponUpgradeTechCost} <sprite=0>";
        }

        if (Config.ar2a)
        {
            ar2aButton.interactable = true;
            TMP_Text text = ar2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo II\nUnlocked";
        }
        else if (!Config.ar2a && Config.ar1a)
        {
            ar2aButton.interactable = true;
            TMP_Text text = ar2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo II\nUnlock {Config.stage2WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            ar2aButton.interactable = false;
            TMP_Text text = ar2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ar2b)
        {
            ar2bButton.interactable = true;
            TMP_Text text = ar2bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage II\nUnlocked";
        }
        else if (!Config.ar2b && Config.ar1b)
        {
            ar2bButton.interactable = true;
            TMP_Text text = ar2bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage II\nUnlock {Config.stage2WeaponUpgradeTechCost} <sprite=0>";
        }
        else 
        {
            ar2bButton.interactable = false;
            TMP_Text text = ar2bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ar2c && Config.alternateARUnlocked)
        {
            ar2cButton.interactable = true;
            TMP_Text text = ar2cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Damage I\nUnlocked";
        }
        else if (Config.alternateARUnlocked)
        {
            ar2cButton.interactable = true;
            TMP_Text text = ar2cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Damage I\nUnlock {Config.stage2WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {   
            ar2cButton.interactable = false;
            TMP_Text text = ar2cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ar3a)
        {
            ar3aButton.interactable = true;
            TMP_Text text = ar3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo III\nUnlocked";
        }
        else if (!Config.ar3a && Config.ar2a)
        {
            ar3aButton.interactable = true;
            TMP_Text text = ar3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo III\nUnlock {Config.stage3WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {   
            ar3aButton.interactable = false;
            TMP_Text text = ar3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ar3b)
        {
            ar3bButton.interactable = true;
            TMP_Text text = ar3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage III\nUnlocked";
        }
        else if (!Config.ar3b && Config.ar2b)
        {
            ar3bButton.interactable = true;
            TMP_Text text = ar3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage III\nUnlock {Config.stage3WeaponUpgradeTechCost} <sprite=0>";
        }
        else 
        {
            ar3bButton.interactable = false;
            TMP_Text text = ar3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ar3c && Config.alternateARUnlocked)
        {
            ar3cButton.interactable = true;
            TMP_Text text = ar3cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Damage II\nUnlocked";
        }
        else if (!Config.ar3c && Config.ar2c && Config.alternateARUnlocked)
        {
            ar3cButton.interactable = true;
            TMP_Text text = ar3cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Damage II\nUnlock {Config.stage3WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            ar3cButton.interactable = false;
            TMP_Text text = ar3cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ar3d && Config.alternateARUnlocked)
        {
            ar3dButton.interactable = true;
            TMP_Text text = ar3dButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Radius I\nUnlocked";
        }
        else if (Config.alternateARUnlocked)
        {
            ar3dButton.interactable = true;
            TMP_Text text = ar3dButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Radius I\nUnlock {Config.stage3WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            ar3dButton.interactable = false;
            TMP_Text text = ar3dButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ar4a)
        {
            ar4aButton.interactable = true;
            TMP_Text text = ar4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo IV\nUnlocked";
        }
        else if (!Config.ar4a && Config.ar3a)
        {
            ar4aButton.interactable = true;
            TMP_Text text = ar4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo IV\nUnlock {Config.stage4WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            ar4aButton.interactable = false;
            TMP_Text text = ar4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ar4b)
        {
            ar4bButton.interactable = true;
            TMP_Text text = ar4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage IV\nUnlocked";
        }
        else if (!Config.ar4b && Config.ar3b)
        {
            ar4bButton.interactable = true;
            TMP_Text text = ar4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage IV\nUnlock {Config.stage4WeaponUpgradeTechCost} <sprite=0>";
        }
        else 
        {
            ar4bButton.interactable = false;
            TMP_Text text = ar4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ar4c && Config.alternateARUnlocked)
        {
            ar4cButton.interactable = true;
            TMP_Text text = ar4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Damage III\nUnlocked";
        }
        else if (!Config.ar4c && Config.ar3c && Config.alternateARUnlocked)
        {
            ar4cButton.interactable = true;
            TMP_Text text = ar4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Damage III\nUnlock {Config.stage4WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            ar4cButton.interactable = false;
            TMP_Text text = ar4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ar4d && Config.alternateARUnlocked)
        {
            ar4dButton.interactable = true;
            TMP_Text text = ar4dButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Radius II\nUnlocked";
        }
        else if (!Config.ar4d && Config.ar3d && Config.alternateARUnlocked)
        {
            ar4dButton.interactable = true;
            TMP_Text text = ar4dButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Radius II\nUnlock {Config.stage4WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            ar4dButton.interactable = false;
            TMP_Text text = ar4dButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        // Machete

        if (Config.mc1a)
        {
            TMP_Text text = mc1aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage I\nUnlocked";
        }
        else
        {
            TMP_Text text = mc1aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage I\nUnlock {Config.stage1WeaponUpgradeTechCost} <sprite=0>";
        }

        if (Config.mc2a)
        {
            mc2aButton.interactable = true;
            TMP_Text text = mc2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage II\nUnlocked";
        }
        else if (!Config.mc2a && Config.mc1a)
        {
            mc2aButton.interactable = true;
            TMP_Text text = mc2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage II\nUnlock {Config.stage2WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            mc2aButton.interactable = false;
            TMP_Text text = mc2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.mc2b)
        {
            TMP_Text text = mc2bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Attack Rate I\nUnlocked";
        }
        else
        {
            TMP_Text text = mc2bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Attack Rate I\nUnlock {Config.stage2WeaponUpgradeTechCost} <sprite=0>";
        }

        if (Config.mc3a)
        {
            mc3aButton.interactable = true;
            TMP_Text text = mc3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage III\nUnlocked";
        }
        else if (!Config.mc3a && Config.mc2a)
        {
            mc3aButton.interactable = true;
            TMP_Text text = mc3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage III\nUnlock {Config.stage3WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            mc3aButton.interactable = false;
            TMP_Text text = mc3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.mc3b)
        {
            mc3bButton.interactable = true;
            TMP_Text text = mc3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Attack Rate II\nUnlocked";
        }
        else if (!Config.mc3b && Config.mc2b)
        {
            mc3bButton.interactable = true;
            TMP_Text text = mc3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Attack Rate II\nUnlock {Config.stage3WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            mc3bButton.interactable = false;
            TMP_Text text = mc3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.mc3c && Config.alternateMacheteUnlocked)
        {
            mc3cButton.interactable = true;
            TMP_Text text = mc3cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Rate I\nUnlocked";
        }
        else if (Config.alternateMacheteUnlocked)
        {
            mc3cButton.interactable = true;
            TMP_Text text = mc3cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Rate I\nUnlock {Config.stage3WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            mc3cButton.interactable = false;
            TMP_Text text = mc3cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.mc4a)
        {
            mc4aButton.interactable = true;
            TMP_Text text = mc4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage IV\nUnlocked";
        }
        else if (!Config.mc4a && Config.mc3a)
        {
            mc4aButton.interactable = true;
            TMP_Text text = mc4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage IV\nUnlock {Config.stage4WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            mc4aButton.interactable = false;
            TMP_Text text = mc4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.mc4b)
        {
            mc4bButton.interactable = true;
            TMP_Text text = mc4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Attack Rate III\nUnlocked";
        }
        else if (!Config.mc4b && Config.mc3b)
        {
            mc4bButton.interactable = true;
            TMP_Text text = mc4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Attack Rate III\nUnlock {Config.stage4WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            mc4bButton.interactable = false;
            TMP_Text text = mc4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.mc4c && Config.alternateMacheteUnlocked)
        {
            mc4cButton.interactable = true;
            TMP_Text text = mc4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Rate II\nUnlocked";
        }
        else if (!Config.mc4c && Config.mc3c && Config.alternateMacheteUnlocked)
        {
            mc4cButton.interactable = true;
            TMP_Text text = mc4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Alt Rate II\nUnlock {Config.stage4WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            mc4cButton.interactable = false;
            TMP_Text text = mc4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        // Shotgun

        if (Config.sh2a)
        {
            sh2aButton.interactable = true;
            TMP_Text text = sh2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo I\nUnlocked";
        }
        else
        {
            sh2aButton.interactable = true;
            TMP_Text text = sh2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo I\nUnlock {Config.stage2WeaponUpgradeTechCost} <sprite=0>";
        }

        if (Config.sh2b)
        {
            sh2bButton.interactable = true;
            TMP_Text text = sh2bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage I\nUnlocked";
        }
        else
        {
            sh2bButton.interactable = true;
            TMP_Text text = sh2bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage I\nUnlock {Config.stage2WeaponUpgradeTechCost} <sprite=0>";
        }

        if (Config.sh3a)
        {
            sh3aButton.interactable = true;
            TMP_Text text = sh3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo II\nUnlocked";
        }
        else if (!Config.sh3a && Config.sh2a)
        {
            sh3aButton.interactable = true;
            TMP_Text text = sh3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo II\nUnlock {Config.stage3WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            sh3aButton.interactable = false;
            TMP_Text text = sh3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.sh3b)
        {
            sh3bButton.interactable = true;
            TMP_Text text = sh3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage II\nUnlocked";
        }
        else if (!Config.sh3b && Config.sh2b)
        {
            sh3bButton.interactable = true;
            TMP_Text text = sh3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage II\nUnlock {Config.stage3WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            sh3bButton.interactable = false;
            TMP_Text text = sh3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.sh3c)
        {
            sh3cButton.interactable = true;
            TMP_Text text = sh3cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Fire Rate I\nUnlocked";
        }
        else
        {
            sh3cButton.interactable = true;
            TMP_Text text = sh3cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Fire Rate I\nUnlock {Config.stage3WeaponUpgradeTechCost} <sprite=0>";
        }

        if (Config.sh4a)
        {
            sh4aButton.interactable = true;
            TMP_Text text = sh4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo III\nUnlocked";
        }
        else if (!Config.sh4a && Config.sh3a)
        {
            sh4aButton.interactable = true;
            TMP_Text text = sh4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Ammo III\nUnlock {Config.stage4WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            sh4aButton.interactable = false;
            TMP_Text text = sh4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.sh4b)
        {
            sh4bButton.interactable = true;
            TMP_Text text = sh4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage III\nUnlocked";
        }
        else if (!Config.sh4b && Config.sh3b)
        {
            sh4bButton.interactable = true;
            TMP_Text text = sh4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage III\nUnlock {Config.stage4WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            sh4bButton.interactable = false;
            TMP_Text text = sh4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.sh4c)
        {
            sh4cButton.interactable = true;
            TMP_Text text = sh4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Fire Rate II\nUnlocked";
        }
        else if (!Config.sh4c && Config.sh3c)
        {
            sh4cButton.interactable = true;
            TMP_Text text = sh4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Fire Rate II\nUnlock {Config.stage4WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            sh4cButton.interactable = false;
            TMP_Text text = sh4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.sh4d)
        {
            sh4dButton.interactable = true;
            TMP_Text text = sh4dButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Range I\nUnlocked";
        }
        else
        {
            sh4dButton.interactable = true;
            TMP_Text text = sh4dButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Range I\nUnlock {Config.stage4WeaponUpgradeTechCost} <sprite=0>";
        }

        if (Config.sh4e && Config.alternateShotgunUnlocked)
        {
            sh4eButton.interactable = true;
            TMP_Text text = sh4eButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Knockback I\nUnlocked";
        }
        else if (Config.alternateShotgunUnlocked)
        {
            sh4eButton.interactable = true;
            TMP_Text text = sh4eButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Knockback I\nUnlock {Config.stage4WeaponUpgradeTechCost} <sprite=0>";
        }
        else
        {
            sh4eButton.interactable = false;
            TMP_Text text = sh4eButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";   
        }
    }

    int CalculateRefillCost(PlayerInventory inventory)
    {
        int cost = 0;

        if (inventory.ar.currentAmmo < Config.maxAmmoAR)
        {
            cost += Config.refillTechCostAR;
        }

        if (Config.shotgunUnlocked && inventory.shotgun.currentAmmo < Config.maxAmmoShot)
        {
            cost += Config.refillTechCostShotgun;
        }

        return cost;
    }

    public void DamageBuilding(float damage, string source = "") {}

    #region input functions

    public void QuickInteract(InputAction.CallbackContext con)
    {
        if (con.performed)
        {
            quickInteract = true;
        }
    }

    public void Interact(InputAction.CallbackContext con)
    {
        if (con.performed)
        {
            interact = true;
        }
    }

    #endregion input functions
}
