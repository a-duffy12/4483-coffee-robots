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
        unbuiltPrefab.SetActive(false);
        builtPrefab.SetActive(true);
        buildStatus = BuildStatus.Built;
        PlayerInventory.scrap -= Config.armoryScrapCost;
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

        if (Config.gameStage < 2)
        {
            shotgunButton.gameObject.SetActive(false);
        }
        else
        {
            shotgunButton.gameObject.SetActive(true);
        }
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
