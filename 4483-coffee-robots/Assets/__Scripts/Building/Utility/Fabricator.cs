using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Fabricator : MonoBehaviour, IBuilding
{
    [Header("GameObjects")]
    [SerializeField] private GameObject unbuiltPrefab;
    [SerializeField] private GameObject builtPrefab;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private GameObject menu;
    [SerializeField] private Button turretPanelButton;
    [SerializeField] private Button spikesPanelButton;
    [SerializeField] private Button ggPanelButton;
    [SerializeField] private Button rocketPanelButton;
    [SerializeField] private GameObject turretPanel;
    [SerializeField] private GameObject spikesPanel;
    [SerializeField] private GameObject ggPanel;
    [SerializeField] private GameObject rocketPanel;

    [Header("Turret")]
    [SerializeField] private Button turretButton;
    [SerializeField] private TMP_Text turretCount;
    [SerializeField] private TMP_Text turretText;
    [SerializeField] private Button tu2aButton;
    [SerializeField] private Button tu2bButton;
    [SerializeField] private Button tu2cButton;
    [SerializeField] private Button tu3aButton;
    [SerializeField] private Button tu3bButton;
    [SerializeField] private Button tu3cButton;
    [SerializeField] private Button tu4aButton;
    [SerializeField] private Button tu4bButton;
    [SerializeField] private Button tu4cButton;

    [Header("Spikes")]
    [SerializeField] private Button spikesButton;
    [SerializeField] private TMP_Text spikesCount;
    [SerializeField] private TMP_Text spikesText;
    [SerializeField] private Button sp2aButton;
    [SerializeField] private Button sp3aButton;
    [SerializeField] private Button sp4aButton;

    [Header("Gamma Generator")]
    [SerializeField] private Button ggButton;
    [SerializeField] private TMP_Text ggCount;
    [SerializeField] private TMP_Text ggText;
    [SerializeField] private Button gg3aButton;
    [SerializeField] private Button gg3bButton;
    [SerializeField] private Button gg4aButton;
    [SerializeField] private Button gg4bButton;

    [Header("Rocket Tower")]
    [SerializeField] private Button rocketButton;
    [SerializeField] private TMP_Text rocketCount;
    [SerializeField] private TMP_Text rocketText;
    [SerializeField] private Button rt3aButton;
    [SerializeField] private Button rt3bButton;
    [SerializeField] private Button rt4aButton;
    [SerializeField] private Button rt4bButton;

    [Header("Defenses")]
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private GameObject spikesPrefab;
    [SerializeField] private GameObject gammaGeneratorPrefab;
    [SerializeField] private GameObject rocketTowerPrefab;

    [Header("Audio")]
    public AudioClip succeedAudio;
    public AudioClip failAudio;
    AudioSource source;

    GameObject player;
    PlayerInventory inventory;
    PlayerInput input;
    private BuildStatus buildStatus = BuildStatus.Locked;
    private bool interact;
    private bool quickInteract;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<PlayerInventory>();
        input = player.GetComponent<PlayerInput>();
        source = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }
    
    void Start()
    {
        unbuiltPrefab.SetActive(false);
        builtPrefab.SetActive(false);
        menu.SetActive(false);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= Config.buildingInteractDistance)
        {
            if (buildStatus == BuildStatus.Unlocked && Config.gameStage > 0 && Config.gameStage < 5)
            {
                promptText.text = $"Build Fabricator {Config.fabricatorScrapCost} <sprite=2> [E]";

                if (quickInteract)
                {
                    Build();
                }
            }
            else if (buildStatus == BuildStatus.Built)
            {
                promptText.text = "Open Fabricator [F]";

                if (interact)
                {
                    OpenMenu();
                }
            }
            else if (promptText.text.StartsWith("Build Fabricator") || promptText.text.StartsWith("Open Fabricator"))
            {
                promptText.text = ""; // only sets to empty if player just left text radius
            }
        }
        else if (promptText.text.StartsWith("Build Fabricator") || promptText.text.StartsWith("Open Fabricator"))
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
        if (PlayerInventory.scrap >= Config.fabricatorScrapCost)
        {
            unbuiltPrefab.SetActive(false);
            builtPrefab.SetActive(true);
            buildStatus = BuildStatus.Built;
            PlayerInventory.scrap -= Config.fabricatorScrapCost;
            PlayAudio(succeedAudio);
        }
        else
        {
            PlayAudio(failAudio);
        }
    }

    void OpenMenu()
    {
        if (Config.aChasisUnlocked)
        {
            PlayerSystem.sleepProtected = true;
        }
        
        input.SwitchCurrentActionMap("Menu");
        menu.SetActive(true);

        if (Config.gameStage <= 1)
        {
            turretPanel.SetActive(false);
            turretPanelButton.gameObject.SetActive(false);
            spikesPanel.SetActive(false);
            spikesPanelButton.gameObject.SetActive(false);
            turretButton.gameObject.SetActive(false);
            spikesButton.gameObject.SetActive(false);
            ggPanel.SetActive(false);
            ggPanelButton.gameObject.SetActive(false);
            ggButton.gameObject.SetActive(false);
            rocketPanel.SetActive(false);
            rocketPanelButton.gameObject.SetActive(false);
            rocketButton.gameObject.SetActive(false);
            tu2aButton.gameObject.SetActive(false);
            tu2bButton.gameObject.SetActive(false);
            tu2cButton.gameObject.SetActive(false);
            tu3aButton.gameObject.SetActive(false);
            tu3bButton.gameObject.SetActive(false);
            tu3cButton.gameObject.SetActive(false);
            tu4aButton.gameObject.SetActive(false);
            tu4bButton.gameObject.SetActive(false);
            tu4cButton.gameObject.SetActive(false);
            sp2aButton.gameObject.SetActive(false);
            sp3aButton.gameObject.SetActive(false);
            sp4aButton.gameObject.SetActive(false);
            gg3aButton.gameObject.SetActive(false);
            gg3bButton.gameObject.SetActive(false);
            gg4aButton.gameObject.SetActive(false);
            gg4bButton.gameObject.SetActive(false);
            rt3aButton.gameObject.SetActive(false);
            rt3bButton.gameObject.SetActive(false);
            rt4aButton.gameObject.SetActive(false);
            rt4bButton.gameObject.SetActive(false);
        }
        else if (Config.gameStage == 2)
        {
            turretPanel.SetActive(true);
            turretPanelButton.gameObject.SetActive(true);
            spikesPanel.SetActive(false);
            spikesPanelButton.gameObject.SetActive(true);
            turretButton.gameObject.SetActive(true);
            spikesButton.gameObject.SetActive(true);
            ggPanel.SetActive(false);
            ggPanelButton.gameObject.SetActive(false);
            ggButton.gameObject.SetActive(false);
            rocketPanel.SetActive(false);
            rocketPanelButton.gameObject.SetActive(false);
            rocketButton.gameObject.SetActive(false);
            tu2aButton.gameObject.SetActive(true);
            tu2bButton.gameObject.SetActive(true);
            tu2cButton.gameObject.SetActive(true);
            tu3aButton.gameObject.SetActive(false);
            tu3bButton.gameObject.SetActive(false);
            tu3cButton.gameObject.SetActive(false);
            tu4aButton.gameObject.SetActive(false);
            tu4bButton.gameObject.SetActive(false);
            tu4cButton.gameObject.SetActive(false);
            sp2aButton.gameObject.SetActive(true);
            sp3aButton.gameObject.SetActive(false);
            sp4aButton.gameObject.SetActive(false);
            gg3aButton.gameObject.SetActive(false);
            gg3bButton.gameObject.SetActive(false);
            gg4aButton.gameObject.SetActive(false);
            gg4bButton.gameObject.SetActive(false);
            rt3aButton.gameObject.SetActive(false);
            rt3bButton.gameObject.SetActive(false);
            rt4aButton.gameObject.SetActive(false);
            rt4bButton.gameObject.SetActive(false);
        }
        else if (Config.gameStage == 3)
        {
            turretPanel.SetActive(true);
            turretPanelButton.gameObject.SetActive(true);
            spikesPanel.SetActive(false);
            spikesPanelButton.gameObject.SetActive(true);
            turretButton.gameObject.SetActive(true);
            spikesButton.gameObject.SetActive(true);
            ggPanel.SetActive(false);
            ggPanelButton.gameObject.SetActive(true);
            ggButton.gameObject.SetActive(true);
            rocketPanel.SetActive(false);
            rocketPanelButton.gameObject.SetActive(true);
            rocketButton.gameObject.SetActive(true);
            tu2aButton.gameObject.SetActive(true);
            tu2bButton.gameObject.SetActive(true);
            tu2cButton.gameObject.SetActive(true);
            tu3aButton.gameObject.SetActive(true);
            tu3bButton.gameObject.SetActive(true);
            tu3cButton.gameObject.SetActive(true);
            tu4aButton.gameObject.SetActive(false);
            tu4bButton.gameObject.SetActive(false);
            tu4cButton.gameObject.SetActive(false);
            sp2aButton.gameObject.SetActive(true);
            sp3aButton.gameObject.SetActive(true);
            sp4aButton.gameObject.SetActive(false);
            gg3aButton.gameObject.SetActive(true);
            gg3bButton.gameObject.SetActive(true);
            gg4aButton.gameObject.SetActive(false);
            gg4bButton.gameObject.SetActive(false);
            rt3aButton.gameObject.SetActive(true);
            rt3bButton.gameObject.SetActive(true);
            rt4aButton.gameObject.SetActive(false);
            rt4bButton.gameObject.SetActive(false);
        }
        else if (Config.gameStage >= 4)
        {
            turretPanel.SetActive(true);
            turretPanelButton.gameObject.SetActive(true);
            spikesPanel.SetActive(false);
            spikesPanelButton.gameObject.SetActive(true);
            turretButton.gameObject.SetActive(true);
            spikesButton.gameObject.SetActive(true);
            ggPanel.SetActive(false);
            ggPanelButton.gameObject.SetActive(true);
            ggButton.gameObject.SetActive(true);
            rocketPanel.SetActive(false);
            rocketPanelButton.gameObject.SetActive(true);
            rocketButton.gameObject.SetActive(true);
            tu2aButton.gameObject.SetActive(true);
            tu2bButton.gameObject.SetActive(true);
            tu2cButton.gameObject.SetActive(true);
            tu3aButton.gameObject.SetActive(true);
            tu3bButton.gameObject.SetActive(true);
            tu3cButton.gameObject.SetActive(true);
            tu4aButton.gameObject.SetActive(true);
            tu4bButton.gameObject.SetActive(true);
            tu4cButton.gameObject.SetActive(true);
            sp2aButton.gameObject.SetActive(true);
            sp3aButton.gameObject.SetActive(true);
            sp4aButton.gameObject.SetActive(true); 
            gg3aButton.gameObject.SetActive(true);
            gg3bButton.gameObject.SetActive(true);
            gg4aButton.gameObject.SetActive(true);
            gg4bButton.gameObject.SetActive(true); 
            rt3aButton.gameObject.SetActive(true);
            rt3bButton.gameObject.SetActive(true);
            rt4aButton.gameObject.SetActive(true);
            rt4bButton.gameObject.SetActive(true);         
        }

        UpdateStoreText();
        SetButtonTextValues();
        PlayAudio(succeedAudio);
    }

    public void CloseMenu()
    {
        PlayerSystem.sleepProtected = false;
        input.SwitchCurrentActionMap("Player");
        menu.SetActive(false);
        PlayAudio(failAudio);
    }

    public void TurretPanel()
    {
        if (menu.activeSelf)
        {
            turretPanel.SetActive(true);
            spikesPanel.SetActive(false);
            ggPanel.SetActive(false);
            rocketPanel.SetActive(false);
        }
    }

    public void SpikesPanel()
    {
        if (menu.activeSelf)
        {
            turretPanel.SetActive(false);
            spikesPanel.SetActive(true);
            ggPanel.SetActive(false);
            rocketPanel.SetActive(false);
        }
    }

    public void GammaGeneratorPanel()
    {
        if (menu.activeSelf)
        {
            turretPanel.SetActive(false);
            spikesPanel.SetActive(false);
            ggPanel.SetActive(true);
            rocketPanel.SetActive(false);
        }
    }

    public void RocketPanel()
    {
        if (menu.activeSelf)
        {
            turretPanel.SetActive(false);
            spikesPanel.SetActive(false);
            ggPanel.SetActive(false);
            rocketPanel.SetActive(true);
        }
    }

    #region purchases

    public void PurchaseTurret()
    {
        if (PlayerInventory.scrap >= Config.unlockTurretScrapCost && PlayerInventory.tech >= Config.unlockTurretTechCost)
        {
            if (Config.gameStage == 2 && Config.countTurret < Config.countTurret2)
            {
                inventory.defenses.Add(turretPrefab);
                PlayerInventory.scrap -= Config.unlockTurretScrapCost;
                PlayerInventory.tech -= Config.unlockTurretTechCost;
                Config.countTurret++;
                UpdateStoreText();
            }
            else if (Config.gameStage == 3 && Config.countTurret < Config.countTurret3)
            {
                inventory.defenses.Add(turretPrefab);
                PlayerInventory.scrap -= Config.unlockTurretScrapCost;
                PlayerInventory.tech -= Config.unlockTurretTechCost;
                Config.countTurret++;
                UpdateStoreText();
            }
            else if (Config.gameStage == 4 && Config.countTurret < Config.countTurret4)
            {
                inventory.defenses.Add(turretPrefab);
                PlayerInventory.scrap -= Config.unlockTurretScrapCost;
                PlayerInventory.tech -= Config.unlockTurretTechCost;
                Config.countTurret++;
                UpdateStoreText();
            }

            PlayAudio(succeedAudio);
        }
        else
        {
            PlayAudio(failAudio);
        }
    }

    public void PurchaseSpikes()
    {
        if (PlayerInventory.scrap >= Config.unlockSpikesScrapCost)
        {
            if (Config.gameStage == 2 && Config.countSpikes < Config.countSpikes2)
            {
                inventory.defenses.Add(spikesPrefab);
                PlayerInventory.scrap -= Config.unlockSpikesScrapCost;
                Config.countSpikes++;
                UpdateStoreText();
            }
            else if (Config.gameStage == 3 && Config.countSpikes < Config.countSpikes3)
            {
                inventory.defenses.Add(spikesPrefab);
                PlayerInventory.scrap -= Config.unlockSpikesScrapCost;
                Config.countSpikes++;
                UpdateStoreText();
            }
            else if (Config.gameStage == 4 && Config.countSpikes < Config.countSpikes4)
            {
                inventory.defenses.Add(spikesPrefab);
                PlayerInventory.scrap -= Config.unlockSpikesScrapCost;
                Config.countSpikes++;
                UpdateStoreText();
            }

            PlayAudio(succeedAudio);
        }
        else
        {
            PlayAudio(failAudio);
        }
    }

    public void PurchaseGammaGenerator()
    {
        if (PlayerInventory.scrap >= Config.unlockGGScrapCost && PlayerInventory.electronics >= Config.unlockGGElectronicsCost)
        {
            if (Config.gameStage == 3 && Config.countGG < Config.countGG3)
            {
                inventory.defenses.Add(gammaGeneratorPrefab);
                PlayerInventory.scrap -= Config.unlockGGScrapCost;
                PlayerInventory.electronics -= Config.unlockGGElectronicsCost;
                Config.countGG++;
                UpdateStoreText();
            }
            else if (Config.gameStage == 4 && Config.countGG < Config.countGG4)
            {
                inventory.defenses.Add(gammaGeneratorPrefab);
                PlayerInventory.scrap -= Config.unlockGGScrapCost;
                PlayerInventory.electronics -= Config.unlockGGElectronicsCost;
                Config.countGG++;
                UpdateStoreText();
            }

            PlayAudio(succeedAudio);
        }
        else
        {
            PlayAudio(failAudio);
        }
    }

    public void PurchaseRocketTower()
    {
        if (PlayerInventory.scrap >= Config.unlockRocketScrapCost && PlayerInventory.tech >= Config.unlockRocketTechCost)
        {
            if (Config.gameStage == 3 && Config.countRocket < Config.countRocket3)
            {
                inventory.defenses.Add(rocketTowerPrefab);
                PlayerInventory.scrap -= Config.unlockRocketScrapCost;
                PlayerInventory.tech -= Config.unlockRocketTechCost;
                Config.countRocket++;
                UpdateStoreText();
            }
            else if (Config.gameStage == 4 && Config.countRocket < Config.countRocket4)
            {
                inventory.defenses.Add(rocketTowerPrefab);
                PlayerInventory.scrap -= Config.unlockRocketScrapCost;
                PlayerInventory.tech -= Config.unlockRocketTechCost;
                Config.countRocket++;
                UpdateStoreText();
            }

            PlayAudio(succeedAudio);
        }
        else
        {
            PlayAudio(failAudio);
        }
    }

    #endregion purchases

    #region TU upgrades

    public void TU2A()
    {
        if (!Config.tu2a && PlayerInventory.scrap >= Config.stage2DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage2DefenseUpgradeScrapCost;
            Config.tu2a = true;
            Config.damageTurret = 11;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.tu2a)
        {
            PlayAudio(failAudio);
        }
    }

    public void TU2B()
    {
        if (!Config.tu2b && PlayerInventory.scrap >= Config.stage2DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage2DefenseUpgradeScrapCost;
            Config.tu2b = true;
            Config.attackRateTurret = 2.3f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.tu2b)
        {
            PlayAudio(failAudio);
        }
    }

    public void TU2C()
    {
        if (!Config.tu2c && PlayerInventory.scrap >= Config.stage2DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage2DefenseUpgradeScrapCost;
            Config.tu2c = true;
            Config.rangeTurret = 26;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.tu2c)
        {
            PlayAudio(failAudio);
        }
    }

    public void TU3A()
    {
        if (!Config.tu3a && PlayerInventory.scrap >= Config.stage3DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage3DefenseUpgradeScrapCost;
            Config.tu3a = true;
            Config.damageTurret = 13;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.tu3a)
        {
            PlayAudio(failAudio);
        }
    }

    public void TU3B()
    {
        if (!Config.tu3b && PlayerInventory.scrap >= Config.stage3DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage3DefenseUpgradeScrapCost;
            Config.tu3b = true;
            Config.attackRateTurret = 2.6f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.tu3b)
        {
            PlayAudio(failAudio);
        }
    }

    public void TU3C()
    {
        if (!Config.tu3c && PlayerInventory.scrap >= Config.stage3DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage3DefenseUpgradeScrapCost;
            Config.tu3c = true;
            Config.rangeTurret = 28;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.tu3c)
        {
            PlayAudio(failAudio);
        }
    }

    public void TU4A()
    {
        if (!Config.tu4a && PlayerInventory.scrap >= Config.stage4DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage4DefenseUpgradeScrapCost;
            Config.tu4a = true;
            Config.damageTurret = 15;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.tu4a)
        {
            PlayAudio(failAudio);
        }
    }

    public void TU4B()
    {
        if (!Config.tu4b && PlayerInventory.scrap >= Config.stage4DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage4DefenseUpgradeScrapCost;
            Config.tu4b = true;
            Config.attackRateTurret = 3f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.tu4b)
        {
            PlayAudio(failAudio);
        }
    }

    public void TU4C()
    {
        if (!Config.tu4c && PlayerInventory.scrap >= Config.stage4DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage4DefenseUpgradeScrapCost;
            Config.tu4c = true;
            Config.rangeTurret = 30;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.tu4c)
        {
            PlayAudio(failAudio);
        }
    }

    #endregion TU upgrades

    #region SP upgrades

    public void SP2A()
    {
        if (!Config.sp2a && PlayerInventory.scrap >= Config.stage2DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage2DefenseUpgradeScrapCost;
            Config.sp2a = true;
            Config.damageSpikes = 12;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.sp2a)
        {
            PlayAudio(failAudio);
        }
    }

    public void SP3A()
    {
        if (!Config.sp3a && PlayerInventory.scrap >= Config.stage3DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage3DefenseUpgradeScrapCost;
            Config.sp3a = true;
            Config.damageSpikes = 15;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.sp3a)
        {
            PlayAudio(failAudio);
        }
    }

    public void SP4A()
    {
        if (!Config.sp4a && PlayerInventory.scrap >= Config.stage4DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage4DefenseUpgradeScrapCost;
            Config.sp4a = true;
            Config.damageSpikes = 20;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.sp4a)
        {
            PlayAudio(failAudio);
        }
    }

    #endregion SP upgrades

    #region GG upgrades

    public void GG3A()
    {
        if (!Config.gg3a && PlayerInventory.scrap >= Config.stage3DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage3DefenseUpgradeScrapCost;
            Config.gg3a = true;
            Config.damageGG = 9;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.gg3a)
        {
            PlayAudio(failAudio);
        }
    }

    public void GG3B()
    {
        if (!Config.gg3b && PlayerInventory.scrap >= Config.stage3DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage3DefenseUpgradeScrapCost;
            Config.gg3b = true;
            Config.targetCountGG = 9;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.gg3b)
        {
            PlayAudio(failAudio);
        }
    }

    public void GG4A()
    {
        if (!Config.gg4a && PlayerInventory.scrap >= Config.stage4DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage4DefenseUpgradeScrapCost;
            Config.gg4a = true;
            Config.damageGG = 11;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.gg4a)
        {
            PlayAudio(failAudio);
        }
    }

    public void GG4B()
    {
        if (!Config.gg4b && PlayerInventory.scrap >= Config.stage4DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage4DefenseUpgradeScrapCost;
            Config.gg4b = true;
            Config.targetCountGG = 11;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.gg4b)
        {
            PlayAudio(failAudio);
        }
    }

    #endregion GG upgrades

    #region RT upgrades

    public void RT3A()
    {
        if (!Config.rt3a && PlayerInventory.scrap >= Config.stage3DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage3DefenseUpgradeScrapCost;
            Config.rt3a = true;
            Config.damageRocket = 198;
            Config.splashDamageRocket = 22;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.rt3a)
        {
            PlayAudio(failAudio);
        }
    }

    public void RT3B()
    {
        if (!Config.rt3b && PlayerInventory.scrap >= Config.stage3DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage3DefenseUpgradeScrapCost;
            Config.rt3b = true;
            Config.attackRateRocket = 0.454545f;
            Config.retargetDelayRocket = 2.2f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.rt3b)
        {
            PlayAudio(failAudio);
        }
    }

    public void RT4A()
    {
        if (!Config.rt4a && PlayerInventory.scrap >= Config.stage4DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage4DefenseUpgradeScrapCost;
            Config.rt4a = true;
            Config.damageRocket = 225;
            Config.splashDamageRocket = 25;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.rt4a)
        {
            PlayAudio(failAudio);
        }
    }

    public void RT4B()
    {
        if (!Config.rt4b && PlayerInventory.scrap >= Config.stage4DefenseUpgradeScrapCost)
        {
            PlayerInventory.scrap -= Config.stage4DefenseUpgradeScrapCost;
            Config.rt4b = true;
            Config.attackRateRocket = 0.5f;
            Config.retargetDelayRocket = 2f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.rt4b)
        {
            PlayAudio(failAudio);
        }
    }

    #endregion RT upgrades

    void UpdateStoreText()
    {
        if (Config.gameStage == 2)
        {
            if (Config.countTurret < Config.countTurret2)
            {
                turretText.text = $"{Config.unlockTurretScrapCost} <sprite=2> {Config.unlockTurretTechCost} <sprite=0>";
            }
            else
            {
                turretText.text = $"Max Number Bought";
            }

            if (Config.countSpikes < Config.countSpikes2)
            {
                spikesText.text = $"{Config.unlockSpikesScrapCost} <sprite=2>";
            }
            else
            {
                spikesText.text = $"Max Number Bought";
            }

            turretCount.text = $"{Config.countTurret}/{Config.countTurret2} Turrets Built";
            spikesCount.text = $"{Config.countSpikes}/{Config.countSpikes2} Spikes Built";
        }
        else if (Config.gameStage == 3)
        {
            if (Config.countTurret < Config.countTurret3)
            {
                turretText.text = $"{Config.unlockTurretScrapCost} <sprite=2> {Config.unlockTurretTechCost} <sprite=0>";
            }
            else
            {
                turretText.text = $"Max Number Bought";
            }

            if (Config.countSpikes < Config.countSpikes3)
            {
                spikesText.text = $"{Config.unlockSpikesScrapCost} <sprite=2>";
            }
            else
            {
                spikesText.text = $"Max Number Bought";
            }

            if (Config.countGG < Config.countGG3)
            {
                ggText.text = $"{Config.unlockGGScrapCost} <sprite=2> {Config.unlockGGElectronicsCost} <sprite=1>";
            }
            else
            {
                ggText.text = $"Max Number Bought";
            }

            if (Config.countRocket < Config.countRocket3)
            {
                rocketText.text = $"{Config.unlockRocketScrapCost} <sprite=2> {Config.unlockRocketTechCost} <sprite=0>";
            }
            else
            {
                rocketText.text = $"Max Number Bought";
            }

            turretCount.text = $"{Config.countTurret}/{Config.countTurret3} Turrets Built";
            spikesCount.text = $"{Config.countSpikes}/{Config.countSpikes3} Spikes Built";
            ggCount.text = $"{Config.countGG}/{Config.countGG3} Gamma Generators Built";
            rocketCount.text = $"{Config.countRocket}/{Config.countRocket3} Rocket Towers Built";
        }
        else if (Config.gameStage == 4)
        {
            if (Config.countTurret < Config.countTurret4)
            {
                turretText.text = $"{Config.unlockTurretScrapCost} <sprite=2> {Config.unlockTurretTechCost} <sprite=0>";
            }
            else
            {
                turretText.text = $"Max Number Bought";
            }

            if (Config.countSpikes < Config.countSpikes4)
            {
                spikesText.text = $"{Config.unlockSpikesScrapCost} <sprite=2>";
            }
            else
            {
                spikesText.text = $"Max Number Bought";
            }

            if (Config.countGG < Config.countGG4)
            {
                ggText.text = $"{Config.unlockGGScrapCost} <sprite=2> {Config.unlockGGElectronicsCost} <sprite=1>";
            }
            else
            {
                ggText.text = $"Max Number Bought";
            }

            if (Config.countRocket < Config.countRocket4)
            {
                rocketText.text = $"{Config.unlockRocketScrapCost} <sprite=2> {Config.unlockRocketTechCost} <sprite=0>";
            }
            else
            {
                rocketText.text = $"Max Number Bought";
            }

            turretCount.text = $"{Config.countTurret}/{Config.countTurret4} Turrets Built";
            spikesCount.text = $"{Config.countSpikes}/{Config.countSpikes4} Spikes Built";
            ggCount.text = $"{Config.countGG}/{Config.countGG4} Gamma Generators Built";
            rocketCount.text = $"{Config.countRocket}/{Config.countRocket4} Rocket Towers Built";
        }
    }

    void SetButtonTextValues()
    {
        // turret
        if (Config.tu2a)
        {
            TMP_Text text = tu2aButton.GetComponentInChildren<TMP_Text>();
            text.text = "Damage I\nUnlocked";
        }
        else
        {
            TMP_Text text = tu2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage I\nUnlock {Config.stage2DefenseUpgradeScrapCost} <sprite=2>";
        }

        if (Config.tu2b)
        {
            TMP_Text text = tu2bButton.GetComponentInChildren<TMP_Text>();
            text.text = "Fire Rate I\nUnlocked";
        }
        else
        {
            TMP_Text text = tu2bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Fire Rate I\nUnlock {Config.stage2DefenseUpgradeScrapCost} <sprite=2>";
        }

        if (Config.tu2c)
        {
            TMP_Text text = tu2cButton.GetComponentInChildren<TMP_Text>();
            text.text = "Range I\nUnlocked";
        }
        else
        {
            TMP_Text text = tu2cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Range I\nUnlock {Config.stage2DefenseUpgradeScrapCost} <sprite=2>";
        }

        if (Config.tu3a)
        {
            tu3aButton.interactable = true;
            TMP_Text text = tu3aButton.GetComponentInChildren<TMP_Text>();
            text.text = "Damage II\nUnlocked";
        }
        else if (!Config.tu3a && Config.tu2a)
        {
            tu3aButton.interactable = true;
            TMP_Text text = tu3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage II\nUnlock {Config.stage3DefenseUpgradeScrapCost} <sprite=2>";
        }
        else
        {
            tu3aButton.interactable = false;
            TMP_Text text = tu3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.tu3b)
        {
            tu3bButton.interactable = true;
            TMP_Text text = tu3bButton.GetComponentInChildren<TMP_Text>();
            text.text = "Fire Rate II\nUnlocked";
        }
        else if (!Config.tu3b && Config.tu2b)
        {
            tu3bButton.interactable = true;
            TMP_Text text = tu3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Fire Rate II\nUnlock {Config.stage3DefenseUpgradeScrapCost} <sprite=2>";
        }
        else
        {
            tu3bButton.interactable = false;
            TMP_Text text = tu3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.tu3c)
        {
            tu3cButton.interactable = true;
            TMP_Text text = tu3cButton.GetComponentInChildren<TMP_Text>();
            text.text = "Range II\nUnlocked";
        }
        else if (!Config.tu3c && Config.tu2c)
        {
            tu3cButton.interactable = true;
            TMP_Text text = tu3cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Range II\nUnlock {Config.stage3DefenseUpgradeScrapCost} <sprite=2>";
        }
        else
        {
            tu3cButton.interactable = false;
            TMP_Text text = tu3cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.tu4a)
        {
            tu4aButton.interactable = true;
            TMP_Text text = tu4aButton.GetComponentInChildren<TMP_Text>();
            text.text = "Damage III\nUnlocked";
        }
        else if (!Config.tu4a && Config.tu3a)
        {
            tu4aButton.interactable = true;
            TMP_Text text = tu4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage III\nUnlock {Config.stage4DefenseUpgradeScrapCost} <sprite=2>";
        }
        else
        {
            tu4aButton.interactable = false;
            TMP_Text text = tu4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.tu4b)
        {
            tu4bButton.interactable = true;
            TMP_Text text = tu4bButton.GetComponentInChildren<TMP_Text>();
            text.text = "Fire Rate III\nUnlocked";
        }
        else if (!Config.tu4b && Config.tu3b)
        {
            tu4bButton.interactable = true;
            TMP_Text text = tu4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Fire Rate III\nUnlock {Config.stage4DefenseUpgradeScrapCost} <sprite=2>";
        }
        else
        {
            tu4bButton.interactable = false;
            TMP_Text text = tu4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.tu4c)
        {
            tu4cButton.interactable = true;
            TMP_Text text = tu4cButton.GetComponentInChildren<TMP_Text>();
            text.text = "Range III\nUnlocked";
        }
        else if (!Config.tu4c && Config.tu3c)
        {
            tu4cButton.interactable = true;
            TMP_Text text = tu4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Range III\nUnlock {Config.stage4DefenseUpgradeScrapCost} <sprite=2>";
        }
        else
        {
            tu4cButton.interactable = false;
            TMP_Text text = tu4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        // spikes
        if (Config.sp2a)
        {
            TMP_Text text = sp2aButton.GetComponentInChildren<TMP_Text>();
            text.text = "Damage I\nUnlocked";
        }
        else
        {
            TMP_Text text = sp2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage I\nUnlock {Config.stage2DefenseUpgradeScrapCost} <sprite=2>";
        }

        if (Config.sp3a)
        {
            sp3aButton.interactable = true;
            TMP_Text text = sp3aButton.GetComponentInChildren<TMP_Text>();
            text.text = "Damage II\nUnlocked";
        }
        else if (!Config.sp3a && Config.sp2a)
        {
            sp3aButton.interactable = true;
            TMP_Text text = sp3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage II\nUnlock {Config.stage3DefenseUpgradeScrapCost} <sprite=2>";
        }
        else
        {
            sp3aButton.interactable = false;
            TMP_Text text = sp3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.sp4a)
        {
            sp4aButton.interactable = true;
            TMP_Text text = sp4aButton.GetComponentInChildren<TMP_Text>();
            text.text = "Damage III\nUnlocked";
        }
        else if (!Config.sp4a && Config.sp3a)
        {
            sp4aButton.interactable = true;
            TMP_Text text = sp4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage III\nUnlock {Config.stage4DefenseUpgradeScrapCost} <sprite=2>";
        }
        else
        {
            sp4aButton.interactable = false;
            TMP_Text text = sp4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        // gamma generator
        if (Config.gg3a)
        {
            TMP_Text text = gg3aButton.GetComponentInChildren<TMP_Text>();
            text.text = "Damage I\nUnlocked";
        }
        else
        {
            TMP_Text text = gg3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage I\nUnlock {Config.stage3DefenseUpgradeScrapCost} <sprite=2>";
        }

        if (Config.gg3b)
        {
            TMP_Text text = gg3bButton.GetComponentInChildren<TMP_Text>();
            text.text = "Max Targets I\nUnlocked";
        }
        else
        {
            TMP_Text text = gg3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Targets I\nUnlock {Config.stage3DefenseUpgradeScrapCost} <sprite=2>";
        }

        if (Config.gg4a)
        {
            gg4aButton.interactable = true;
            TMP_Text text = gg4aButton.GetComponentInChildren<TMP_Text>();
            text.text = "Damage II\nUnlocked";
        }
        else if (!Config.gg4a && Config.gg3a)
        {
            gg4aButton.interactable = true;
            TMP_Text text = gg4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage II\nUnlock {Config.stage4DefenseUpgradeScrapCost} <sprite=2>";
        }
        else
        {
            gg4aButton.interactable = false;
            TMP_Text text = gg4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.gg4b)
        {
            gg4bButton.interactable = true;
            TMP_Text text = gg4bButton.GetComponentInChildren<TMP_Text>();
            text.text = "Max Targets II\nUnlocked";
        }
        else if (!Config.gg4b && Config.gg3b)
        {
            gg4bButton.interactable = true;
            TMP_Text text = gg4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Max Targets II\nUnlock {Config.stage4DefenseUpgradeScrapCost} <sprite=2>";
        }
        else
        {
            gg4bButton.interactable = false;
            TMP_Text text = gg4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        // rocket tower
        if (Config.rt3a)
        {
            TMP_Text text = rt3aButton.GetComponentInChildren<TMP_Text>();
            text.text = "Damage I\nUnlocked";
        }
        else
        {
            TMP_Text text = rt3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage I\nUnlock {Config.stage3DefenseUpgradeScrapCost} <sprite=2>";
        }

        if (Config.rt3b)
        {
            TMP_Text text = rt3bButton.GetComponentInChildren<TMP_Text>();
            text.text = "Fire Rate I\nUnlocked";
        }
        else
        {
            TMP_Text text = rt3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Fire Rate I\nUnlock {Config.stage3DefenseUpgradeScrapCost} <sprite=2>";
        }

        if (Config.rt4a)
        {
            rt4aButton.interactable = true;
            TMP_Text text = rt4aButton.GetComponentInChildren<TMP_Text>();
            text.text = "Damage II\nUnlocked";
        }
        else if (!Config.rt4a && Config.rt3a)
        {
            rt4aButton.interactable = true;
            TMP_Text text = rt4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Damage II\nUnlock {Config.stage4DefenseUpgradeScrapCost} <sprite=2>";
        }
        else
        {
            rt4aButton.interactable = false;
            TMP_Text text = rt4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.rt4b)
        {
            rt4bButton.interactable = true;
            TMP_Text text = rt4bButton.GetComponentInChildren<TMP_Text>();
            text.text = "Fire Rate II\nUnlocked";
        }
        else if (!Config.rt4b && Config.rt3b)
        {
            rt4bButton.interactable = true;
            TMP_Text text = rt4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Fire Rate II\nUnlock {Config.stage4DefenseUpgradeScrapCost} <sprite=2>";
        }
        else
        {
            rt4bButton.interactable = false;
            TMP_Text text = rt4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }
    }

    public void DamageBuilding(float damage, string source = "") {}

    void PlayAudio(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

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
