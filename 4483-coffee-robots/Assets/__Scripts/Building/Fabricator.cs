using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

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
        }
    }

    public void CloseMenu()
    {
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
