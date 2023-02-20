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
    }
    
    void Start()
    {
        unbuiltPrefab.SetActive(false);
        builtPrefab.SetActive(false);
        menu.SetActive(false);
        PlayerInventory.scrap = 5000; // TODO
        PlayerInventory.electronics = 5000;
        PlayerInventory.tech = 5000;
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
        unbuiltPrefab.SetActive(false);
        builtPrefab.SetActive(true);
        buildStatus = BuildStatus.Built;
        PlayerInventory.scrap -= Config.machineShopScrapCost;
    }

    void RepairPlayer(int cost)
    {
        system.DamagePlayer(Config.playerMaxHp * -1, "machine_shop");
        PlayerInventory.electronics -= cost;
    }

    void OpenMenu()
    {
        input.SwitchCurrentActionMap("Menu");
        menu.SetActive(true);
        chasisPanel.SetActive(true);
        abilityPanel.SetActive(false);
    }

    public void CloseMenu()
    {
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
