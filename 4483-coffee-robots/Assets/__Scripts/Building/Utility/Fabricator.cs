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
    [SerializeField] private GameObject defensePanel;
    [SerializeField] private GameObject trapPanel;
    [SerializeField] private Button defenseButton;
    [SerializeField] private Button trapButton;
    [SerializeField] private TMP_Text turretText;
    [SerializeField] private TMP_Text spikesText;

    [Header("Defenses")]
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private GameObject spikesPrefab;

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

        if (Config.gameStage < 2)
        {
            defensePanel.SetActive(false);
            trapPanel.SetActive(false);
            defenseButton.gameObject.SetActive(false);
            trapButton.gameObject.SetActive(false);
        }
        else
        {
            defensePanel.SetActive(true);
            trapPanel.SetActive(false);
            defenseButton.gameObject.SetActive(true);
            trapButton.gameObject.SetActive(true);

            UpdateStoreText();
        }
    }

    public void CloseMenu()
    {
        PlayerSystem.sleepProtected = false;
        input.SwitchCurrentActionMap("Player");
        menu.SetActive(false);
    }

    public void DefensePanel()
    {
        if (menu.activeSelf)
        {
            defensePanel.SetActive(true);
            trapPanel.SetActive(false);
        }
    }

    public void TrapPanel()
    {
        if (menu.activeSelf)
        {
            defensePanel.SetActive(false);
            trapPanel.SetActive(true);
        }
    }

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
        }
    }

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
        }
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
