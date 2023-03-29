using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class MachineShop : MonoBehaviour, IBuilding
{
    [Header("GameObjects")]
    [SerializeField] private GameObject unbuiltPrefab;
    [SerializeField] private GameObject builtPrefab;
    [SerializeField] private TMP_Text promptText;
     [SerializeField] private GameObject menu;
    [SerializeField] private GameObject chasisPanel;
    [SerializeField] private GameObject abilityPanel;
    [SerializeField] private GameObject chasisAltDesc;
    [SerializeField] private Button chasisAltButton;
    [SerializeField] private GameObject abilityAltDesc;
    [SerializeField] private Button abilityAltButton;

    [Header("Chasis Upgrades")]
    [SerializeField] private Button ch1aButton;
    [SerializeField] private Button ch1bButton;
    [SerializeField] private Button ch2aButton;
    [SerializeField] private Button ch2bButton;
    [SerializeField] private Button ch3aButton;
    [SerializeField] private Button ch3bButton;
    [SerializeField] private Button ch3cButton;
    [SerializeField] private Button ch4aButton;
    [SerializeField] private Button ch4bButton;
    [SerializeField] private Button ch4cButton;

    [Header("Ability Upgrades")]
    [SerializeField] private Button ab1aButton;
    [SerializeField] private Button ab2aButton;
    [SerializeField] private Button ab2bButton;
    [SerializeField] private Button ab2cButton;
    [SerializeField] private Button ab3aButton;
    [SerializeField] private Button ab3bButton;
    [SerializeField] private Button ab3dButton;
    [SerializeField] private Button ab4aButton;
    [SerializeField] private Button ab4bButton;
    [SerializeField] private Button ab4cButton;
    [SerializeField] private Button ab4dButton;

    [Header("Audio")]
    public AudioClip succeedAudio;
    public AudioClip failAudio;
    AudioSource source;

    GameObject player;
    PlayerSystem system;
    PlayerInput input;
    private BuildStatus buildStatus = BuildStatus.Locked;
    private bool interact;
    private bool quickInteract;
    private int repairCost;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        system = player.GetComponent<PlayerSystem>();
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
                promptText.text = $"Build Machine Shop {Config.machineShopScrapCost} <sprite=2> [E]";

                if (quickInteract)
                {
                    Build();
                }
            }
            else if (buildStatus == BuildStatus.Built)
            {
                if (system.hp < Config.playerMaxHp)
                {
                    repairCost = Config.repairElectronicsCost * Config.gameStage > 200 ? 200 : Config.repairElectronicsCost * Config.gameStage; 
                    promptText.text = $"Repair {repairCost} <sprite=1> [E]\n\nOpen Machine Shop [F]";

                    if (quickInteract && PlayerInventory.electronics >= repairCost)
                    {
                        RepairPlayer(repairCost);
                        PlayAudio(succeedAudio);
                    }
                    else if (quickInteract)
                    {
                        PlayAudio(failAudio);
                    }
                }
                else
                {
                    promptText.text = "Open Machine Shop [F]";
                }

                if (interact)
                {
                    OpenMenu();
                }
            }
            else if (promptText.text.StartsWith("Build Machine Shop") || promptText.text.StartsWith("Repair") || promptText.text.StartsWith("Open Machine Shop"))
            {
                promptText.text = ""; // only sets to empty if player just left text radius
            }
        }
        else if (promptText.text.StartsWith("Build Machine Shop") || promptText.text.StartsWith("Repair") || promptText.text.StartsWith("Open Machine Shop"))
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
        if (PlayerInventory.scrap >= Config.machineShopScrapCost)
        {
            unbuiltPrefab.SetActive(false);
            builtPrefab.SetActive(true);
            buildStatus = BuildStatus.Built;
            PlayerInventory.scrap -= Config.machineShopScrapCost;
            PlayAudio(succeedAudio);
        }
        else
        {
            PlayAudio(failAudio);
        }
    }

    void RepairPlayer(int cost)
    {
        system.DamagePlayer(Config.playerMaxHp * -1, "machine_shop");
        PlayerInventory.electronics -= cost;
    }

    void OpenMenu()
    {
        if (Config.aChasisUnlocked)
        {
            PlayerSystem.sleepProtected = true;
        }
        
        input.SwitchCurrentActionMap("Menu");
        menu.SetActive(true);
        chasisPanel.SetActive(true);
        abilityPanel.SetActive(false);

        if (Config.gameStage <= 1)
        {
            chasisAltDesc.SetActive(false);
            chasisAltButton.gameObject.SetActive(false);
            abilityAltDesc.SetActive(false);
            abilityAltButton.gameObject.SetActive(false);
            ch1aButton.gameObject.SetActive(true);
            ch1bButton.gameObject.SetActive(true);
            ch2aButton.gameObject.SetActive(false);
            ch2bButton.gameObject.SetActive(false);
            ch3aButton.gameObject.SetActive(false);
            ch3bButton.gameObject.SetActive(false);
            ch3cButton.gameObject.SetActive(false);
            ch4aButton.gameObject.SetActive(false);
            ch4bButton.gameObject.SetActive(false);
            ch4cButton.gameObject.SetActive(false);
            ab1aButton.gameObject.SetActive(true);
            ab2aButton.gameObject.SetActive(false);
            ab2bButton.gameObject.SetActive(false);
            ab2cButton.gameObject.SetActive(false);
            ab3aButton.gameObject.SetActive(false);
            ab3bButton.gameObject.SetActive(false);
            ab3dButton.gameObject.SetActive(false);
            ab4aButton.gameObject.SetActive(false);
            ab4bButton.gameObject.SetActive(false);
            ab4cButton.gameObject.SetActive(false);
            ab4dButton.gameObject.SetActive(false);
        }
        else if (Config.gameStage == 2)
        {
            chasisAltDesc.SetActive(true);
            chasisAltButton.gameObject.SetActive(true);
            abilityAltDesc.SetActive(false);
            abilityAltButton.gameObject.SetActive(false);
            ch1aButton.gameObject.SetActive(true);
            ch1bButton.gameObject.SetActive(true);
            ch2aButton.gameObject.SetActive(true);
            ch2bButton.gameObject.SetActive(true);
            ch3aButton.gameObject.SetActive(false);
            ch3bButton.gameObject.SetActive(false);
            ch3cButton.gameObject.SetActive(false);
            ch4aButton.gameObject.SetActive(false);
            ch4bButton.gameObject.SetActive(false);
            ch4cButton.gameObject.SetActive(false);
            ab1aButton.gameObject.SetActive(true);
            ab2aButton.gameObject.SetActive(true);
            ab2bButton.gameObject.SetActive(true);
            ab2cButton.gameObject.SetActive(true);
            ab3aButton.gameObject.SetActive(false);
            ab3bButton.gameObject.SetActive(false);
            ab3dButton.gameObject.SetActive(false);
            ab4aButton.gameObject.SetActive(false);
            ab4bButton.gameObject.SetActive(false);
            ab4cButton.gameObject.SetActive(false);
            ab4dButton.gameObject.SetActive(false);  
        }
        else if (Config.gameStage == 3)
        {
            chasisAltDesc.SetActive(true);
            chasisAltButton.gameObject.SetActive(true);
            abilityAltDesc.SetActive(true);
            abilityAltButton.gameObject.SetActive(true);
            ch1aButton.gameObject.SetActive(true);
            ch1bButton.gameObject.SetActive(true);
            ch2aButton.gameObject.SetActive(true);
            ch2bButton.gameObject.SetActive(true);
            ch3aButton.gameObject.SetActive(true);
            ch3bButton.gameObject.SetActive(true);
            ch3cButton.gameObject.SetActive(true);
            ch4aButton.gameObject.SetActive(false);
            ch4bButton.gameObject.SetActive(false);
            ch4cButton.gameObject.SetActive(false);
            ab1aButton.gameObject.SetActive(true);
            ab2aButton.gameObject.SetActive(true);
            ab2bButton.gameObject.SetActive(true);
            ab2cButton.gameObject.SetActive(true);
            ab3aButton.gameObject.SetActive(true);
            ab3bButton.gameObject.SetActive(true);
            ab3dButton.gameObject.SetActive(true);
            ab4aButton.gameObject.SetActive(false);
            ab4bButton.gameObject.SetActive(false);
            ab4cButton.gameObject.SetActive(false);
            ab4dButton.gameObject.SetActive(false);
        }
        else if (Config.gameStage >= 4)
        {
            chasisAltDesc.SetActive(true);
            chasisAltButton.gameObject.SetActive(true);
            abilityAltDesc.SetActive(true);
            abilityAltButton.gameObject.SetActive(true);
            ch1aButton.gameObject.SetActive(true);
            ch1bButton.gameObject.SetActive(true);
            ch2aButton.gameObject.SetActive(true);
            ch2bButton.gameObject.SetActive(true);
            ch3aButton.gameObject.SetActive(true);
            ch3bButton.gameObject.SetActive(true);
            ch3cButton.gameObject.SetActive(true);
            ch4aButton.gameObject.SetActive(true);
            ch4bButton.gameObject.SetActive(true);
            ch4cButton.gameObject.SetActive(true);
            ab1aButton.gameObject.SetActive(true);
            ab2aButton.gameObject.SetActive(true);
            ab2bButton.gameObject.SetActive(true);
            ab2cButton.gameObject.SetActive(true);
            ab3aButton.gameObject.SetActive(true);
            ab3bButton.gameObject.SetActive(true);
            ab3dButton.gameObject.SetActive(true);
            ab4aButton.gameObject.SetActive(true);
            ab4bButton.gameObject.SetActive(true);
            ab4cButton.gameObject.SetActive(true);
            ab4dButton.gameObject.SetActive(true);
        }

        SetButtonTextValues();
    }

    public void CloseMenu()
    {
        PlayerSystem.sleepProtected = false;
        input.SwitchCurrentActionMap("Player");
        menu.SetActive(false);
    }

    public void ChasisPanel()
    {
        if (menu.activeSelf)
        {
            chasisPanel.SetActive(true);
            abilityPanel.SetActive(false);
        }
    }

    public void AbilityPanel()
    {
        if (menu.activeSelf)
        {
            chasisPanel.SetActive(false);
            abilityPanel.SetActive(true);
        }
    }

    public void AChasis()
    {
        if (!Config.aChasisUnlocked && PlayerInventory.electronics >= Config.unlockAChasisElectronicsCost)
        {
            PlayerInventory.electronics -= Config.unlockAChasisElectronicsCost;
            Config.aChasisUnlocked = true;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.aChasisUnlocked)
        {
            PlayAudio(failAudio);
        }
    }

    public void AAbility()
    {
        if (!Config.aAbilityUnlocked && PlayerInventory.electronics >= Config.unlockAAbilityElectronicsCost)
        {
            PlayerInventory.electronics -= Config.unlockAAbilityElectronicsCost;
            Config.aAbilityUnlocked = true;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.aAbilityUnlocked)
        {
            PlayAudio(failAudio);
        }
    }

    #region CH upgrades

    public void CH1A()
    {
        if (!Config.ch1a && PlayerInventory.electronics >= Config.stage1UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage1UpgradeElectronicsCost;
            Config.ch1a = true;
            Config.playerMaxHp = 120;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
            system.DamagePlayer(Config.playerMaxHp * -1, "machine_shop");
        }
        else if (!Config.ch1a)
        {
            PlayAudio(failAudio);
        }
    }

    public void CH1B()
    {
        if (!Config.ch1b && PlayerInventory.electronics >= Config.stage1UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage1UpgradeElectronicsCost;
            Config.ch1b = true;
            Config.dashSpeed = 220;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ch1b)
        {
            PlayAudio(failAudio);
        }
    }

    public void CH2A()
    {
        if (!Config.ch2a && PlayerInventory.electronics >= Config.stage2UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage2UpgradeElectronicsCost;
            Config.ch2a = true;
            Config.playerMaxHp = 145;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
            system.DamagePlayer(Config.playerMaxHp * -1, "machine_shop");
        }
        else if (!Config.ch2a)
        {
            PlayAudio(failAudio);
        }
    }

    public void CH2B()
    {
        if (!Config.ch2b && PlayerInventory.electronics >= Config.stage2UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage2UpgradeElectronicsCost;
            Config.ch2b = true;
            Config.dashSpeed = 245;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ch2b)
        {
            PlayAudio(failAudio);
        }
    }

    public void CH3A()
    {
        if (!Config.ch3a && PlayerInventory.electronics >= Config.stage3UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage3UpgradeElectronicsCost;
            Config.ch3a = true;
            Config.playerMaxHp = 170;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
            system.DamagePlayer(Config.playerMaxHp * -1, "machine_shop");
        }
        else if (!Config.ch3a)
        {
            PlayAudio(failAudio);
        }
    }

    public void CH3B()
    {
        if (!Config.ch3b && PlayerInventory.electronics >= Config.stage3UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage3UpgradeElectronicsCost;
            Config.ch3b = true;
            Config.dashSpeed = 270;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ch3b)
        {
            PlayAudio(failAudio);
        }
    }

    public void CH3C()
    {
        if (!Config.ch3c && PlayerInventory.electronics >= Config.stage3UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage3UpgradeElectronicsCost;
            Config.ch3c = true;
            Config.dashCooldown = 0.75f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ch3c)
        {
            PlayAudio(failAudio);
        }
    }

    public void CH4A()
    {
        if (!Config.ch4a && PlayerInventory.electronics >= Config.stage4UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage4UpgradeElectronicsCost;
            Config.ch4a = true;
            Config.playerMaxHp = 200;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
            system.DamagePlayer(Config.playerMaxHp * -1, "machine_shop");
        }
        else if (!Config.ch4a)
        {
            PlayAudio(failAudio);
        }
    }

    public void CH4B()
    {
        if (!Config.ch4b && PlayerInventory.electronics >= Config.stage4UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage4UpgradeElectronicsCost;
            Config.ch4b = true;
            Config.dashSpeed = 300;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ch4b)
        {
            PlayAudio(failAudio);
        }
    }

    public void CH4C()
    {
        if (!Config.ch4c && PlayerInventory.electronics >= Config.stage4UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage4UpgradeElectronicsCost;
            Config.ch4c = true;
            Config.dashCooldown = 0.5f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ch4c)
        {
            PlayAudio(failAudio);
        }
    }

    #endregion CH upgrades

    #region AB upgrades

    public void AB1A()
    {
        if (!Config.ab1a && PlayerInventory.electronics >= Config.stage1UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage1UpgradeElectronicsCost;
            Config.ab1a = true;
            Config.abilityDamageModifier = 0.45f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ab1a)
        {
            PlayAudio(failAudio);
        }
    }

    public void AB2A()
    {
        if (!Config.ab2a && PlayerInventory.electronics >= Config.stage2UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage2UpgradeElectronicsCost;
            Config.ab2a = true;
            Config.abilityDamageModifier = 0.4f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ab2a)
        {
            PlayAudio(failAudio);
        }
    }

    public void AB2B()
    {
        if (!Config.ab2b && PlayerInventory.electronics >= Config.stage2UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage2UpgradeElectronicsCost;
            Config.ab2b = true;
            Config.abilityDuration = 5.5f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ab2b)
        {
            PlayAudio(failAudio);
        }
    }

    public void AB2C()
    {
        if (!Config.ab2c && PlayerInventory.electronics >= Config.stage2UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage2UpgradeElectronicsCost;
            Config.ab2c = true;
            Config.abilityCooldown = 14;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ab2c)
        {
            PlayAudio(failAudio);
        }
    }

    public void AB3A()
    {
        if (!Config.ab3a && PlayerInventory.electronics >= Config.stage3UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage3UpgradeElectronicsCost;
            Config.ab3a = true;
            Config.abilityDamageModifier = 0.3f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ab3a)
        {
            PlayAudio(failAudio);
        }
    }

    public void AB3B()
    {
        if (!Config.ab3b && PlayerInventory.electronics >= Config.stage3UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage3UpgradeElectronicsCost;
            Config.ab3b = true;
            Config.abilityDuration = 6;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ab3b)
        {
            PlayAudio(failAudio);
        }
    }

    public void AB3D()
    {
        if (!Config.ab3d && PlayerInventory.electronics >= Config.stage3UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage3UpgradeElectronicsCost;
            Config.ab3d = true;
            Config.diversionModifier = 0.75f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ab3d)
        {
            PlayAudio(failAudio);
        }
    }

    public void AB4A()
    {
        if (!Config.ab4a && PlayerInventory.electronics >= Config.stage4UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage4UpgradeElectronicsCost;
            Config.ab4a = true;
            Config.abilityDamageModifier = 0.2f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ab4a)
        {
            PlayAudio(failAudio);
        }
    }

    public void AB4B()
    {
        if (!Config.ab4b && PlayerInventory.electronics >= Config.stage4UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage4UpgradeElectronicsCost;
            Config.ab4b = true;
            Config.abilityDuration = 6.5f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ab4b)
        {
            PlayAudio(failAudio);
        }
    }

    public void AB4C()
    {
        if (!Config.ab4c && PlayerInventory.electronics >= Config.stage4UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage4UpgradeElectronicsCost;
            Config.ab4c = true;
            Config.abilityCooldown = 13;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ab4c)
        {
            PlayAudio(failAudio);
        }
    }

    public void AB4D()
    {
        if (!Config.ab4d && PlayerInventory.electronics >= Config.stage4UpgradeElectronicsCost)
        {
            PlayerInventory.electronics -= Config.stage4UpgradeElectronicsCost;
            Config.ab4d = true;
            Config.diversionModifier = 1f;
            SetButtonTextValues();
            PlayAudio(succeedAudio);
        }
        else if (!Config.ab4d)
        {
            PlayAudio(failAudio);
        }
    }

    #endregion AB upgrades

    void SetButtonTextValues()
    {
        if (Config.aChasisUnlocked)
        {
            TMP_Text text = chasisAltButton.GetComponentInChildren<TMP_Text>();
            text.text = "Unlocked";
        }
        else
        {
            TMP_Text text = chasisAltButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Unlock {Config.unlockAChasisElectronicsCost} <sprite=1>";
        }

        if (Config.aAbilityUnlocked)
        {
            TMP_Text text = abilityAltButton.GetComponentInChildren<TMP_Text>();
            text.text = "Unlocked";
        }
        else
        {
            TMP_Text text = abilityAltButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Unlock {Config.unlockAAbilityElectronicsCost} <sprite=1>";
        }

        // Chasis

        if (Config.ch1a)
        {
            TMP_Text text = ch1aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Vitality I\nUnlocked";
        }
        else
        {
            TMP_Text text = ch1aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Vitality I\nUnlock {Config.stage1UpgradeElectronicsCost} <sprite=1>";
        }

        if (Config.ch1b)
        {
            TMP_Text text = ch1bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Dash Speed I\nUnlocked";
        }
        else
        {
            TMP_Text text = ch1bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Dash Speed I\nUnlock {Config.stage1UpgradeElectronicsCost} <sprite=1>";
        }

        if (Config.ch2a)
        {
            ch2aButton.interactable = true;
            TMP_Text text = ch2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Vitality II\nUnlocked";
        }
        else if (!Config.ch2a && Config.ch1a)
        {
            ch2aButton.interactable = true;
            TMP_Text text = ch2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Vitality II\nUnlock {Config.stage2UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ch2aButton.interactable = false;
            TMP_Text text = ch2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ch2b)
        {
            ch2bButton.interactable = true;
            TMP_Text text = ch2bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Dash Speed II\nUnlocked";
        }
        else if (!Config.ch2b && Config.ch1b)
        {
            ch2bButton.interactable = true;
            TMP_Text text = ch2bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Dash Speed II\nUnlock {Config.stage2UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ch2bButton.interactable = false;
            TMP_Text text = ch2bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ch3a)
        {
            ch3aButton.interactable = true;
            TMP_Text text = ch3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Vitality III\nUnlocked";
        }
        else if (!Config.ch3a && Config.ch2a)
        {
            ch3aButton.interactable = true;
            TMP_Text text = ch3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Vitality III\nUnlock {Config.stage3UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ch3aButton.interactable = false;
            TMP_Text text = ch3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ch3b)
        {
            ch3bButton.interactable = true;
            TMP_Text text = ch3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Dash Speed III\nUnlocked";
        }
        else if (!Config.ch3b && Config.ch2b)
        {
            ch3bButton.interactable = true;
            TMP_Text text = ch3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Dash Speed III\nUnlock {Config.stage3UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ch3bButton.interactable = false;
            TMP_Text text = ch3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ch3c)
        {
            TMP_Text text = ch3cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Recharge I\nUnlocked";
        }
        else
        {
            TMP_Text text = ch3cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Recharge I\nUnlock {Config.stage3UpgradeElectronicsCost} <sprite=1>";
        }

        if (Config.ch4a)
        {
            ch4aButton.interactable = true;
            TMP_Text text = ch4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Vitality IV\nUnlocked";
        }
        else if (!Config.ch4a && Config.ch3a)
        {
            ch4aButton.interactable = true;
            TMP_Text text = ch4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Vitality IV\nUnlock {Config.stage4UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ch4aButton.interactable = false;
            TMP_Text text = ch4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ch4b)
        {
            ch4bButton.interactable = true;
            TMP_Text text = ch4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Dash Speed IV\nUnlocked";
        }
        else if (!Config.ch4b && Config.ch3b)
        {
            ch4bButton.interactable = true;
            TMP_Text text = ch4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Dash Speed IV\nUnlock {Config.stage4UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ch4bButton.interactable = false;
            TMP_Text text = ch4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ch4c)
        {
            ch4cButton.interactable = true;
            TMP_Text text = ch4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Recharge II\nUnlocked";
        }
        else if (!Config.ch4c && Config.ch3c)
        {
            ch4cButton.interactable = true;
            TMP_Text text = ch4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Recharge II\nUnlock {Config.stage4UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ch4cButton.interactable = false;
            TMP_Text text = ch4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        // Ability

        if (Config.ab1a)
        {
            TMP_Text text = ab1aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Resistance I\nUnlocked";
        }
        else
        {
            TMP_Text text = ab1aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Resistance I\nUnlock {Config.stage1UpgradeElectronicsCost} <sprite=1>";
        }

        if (Config.ab2a)
        {
            ab2aButton.interactable = true;
            TMP_Text text = ab2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Resistance II\nUnlocked";
        }
        else if (!Config.ab2a && Config.ab1a)
        {
            ab2aButton.interactable = true;
            TMP_Text text = ab2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Resistance II\nUnlock {Config.stage2UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ab2aButton.interactable = false;
            TMP_Text text = ab2aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ab2b)
        {
            TMP_Text text = ab2bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Duration I\nUnlocked";
        }
        else
        {
            TMP_Text text = ab2bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Duration I\nUnlock {Config.stage2UpgradeElectronicsCost} <sprite=1>";
        }

        if (Config.ab2c)
        {
            TMP_Text text = ab2cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Recharge I\nUnlocked";
        }
        else
        {
            TMP_Text text = ab2cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Recharge I\nUnlock {Config.stage2UpgradeElectronicsCost} <sprite=1>";
        }

        if (Config.ab3a)
        {
            ab3aButton.interactable = true;
            TMP_Text text = ab3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Resistance III\nUnlocked";
        }
        else if (!Config.ab3a && Config.ab2a)
        {
            ab3aButton.interactable = true;
            TMP_Text text = ab3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Resistance III\nUnlock {Config.stage3UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ab3aButton.interactable = false;
            TMP_Text text = ab3aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ab3b)
        {
            ab3bButton.interactable = true;
            TMP_Text text = ab3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Duration II\nUnlocked";
        }
        else if (!Config.ab3b && Config.ab2b)
        {
            ab3bButton.interactable = true;
            TMP_Text text = ab3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Duration II\nUnlock {Config.stage3UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ab3bButton.interactable = false;
            TMP_Text text = ab3bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ab3d && Config.aAbilityUnlocked)
        {
            ab3dButton.interactable = true;
            TMP_Text text = ab3dButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Diversion I\nUnlocked";
        }
        else if (Config.aAbilityUnlocked)
        {
            ab3dButton.interactable = true;
            TMP_Text text = ab3dButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Diversion I\nUnlock {Config.stage3UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ab3dButton.interactable = false;
            TMP_Text text = ab3dButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ab4a)
        {
            ab4aButton.interactable = true;
            TMP_Text text = ab4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Resistance IV\nUnlocked";
        }
        else if (!Config.ab4a && Config.ab3a)
        {
            ab4aButton.interactable = true;
            TMP_Text text = ab4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Resistance IV\nUnlock {Config.stage4UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ab4aButton.interactable = false;
            TMP_Text text = ab4aButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ab4b)
        {
            ab4bButton.interactable = true;
            TMP_Text text = ab4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Duration III\nUnlocked";
        }
        else if (!Config.ab4b && Config.ab3b)
        {
            ab4bButton.interactable = true;
            TMP_Text text = ab4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Duration III\nUnlock {Config.stage4UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ab4bButton.interactable = false;
            TMP_Text text = ab4bButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ab4c)
        {
            ab4cButton.interactable = true;
            TMP_Text text = ab4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Recharge II\nUnlocked";
        }
        else if (!Config.ab4c && Config.ab2c)
        {
            ab4cButton.interactable = true;
            TMP_Text text = ab4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Recharge II\nUnlock {Config.stage4UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ab4cButton.interactable = false;
            TMP_Text text = ab4cButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Locked";
        }

        if (Config.ab4d && Config.aAbilityUnlocked)
        {
            ab4dButton.interactable = true;
            TMP_Text text = ab4dButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Diversion II\nUnlocked";
        }
        else if (!Config.ab4d && Config.ab3d && Config.aAbilityUnlocked)
        {
            ab4dButton.interactable = true;
            TMP_Text text = ab4dButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Diversion II\nUnlock {Config.stage4UpgradeElectronicsCost} <sprite=1>";
        }
        else
        {
            ab4dButton.interactable = false;
            TMP_Text text = ab4dButton.GetComponentInChildren<TMP_Text>();
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
