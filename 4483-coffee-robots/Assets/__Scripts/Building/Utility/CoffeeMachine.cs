using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class CoffeeMachine : MonoBehaviour, IBuilding
{
    [Header("GameObjects")]
    [SerializeField] private GameObject unbuiltPrefab;
    [SerializeField] private GameObject builtPrefab;
    [SerializeField] private TMP_Text promptText;

    GameObject player;
    private BuildStatus buildStatus = BuildStatus.Locked;
    private bool quickInteract;
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        unbuiltPrefab.SetActive(false);
        builtPrefab.SetActive(false);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= Config.buildingInteractDistance)
        {
            if (buildStatus == BuildStatus.Unlocked && Config.gameStage > 2 && Config.gameStage < 5)
            {
                promptText.text = $"Build Coffee Machine {Config.coffeeMachineScrapCost} <sprite=2> [E]";

                if (quickInteract)
                {
                    Build();
                }
            }
            else if (promptText.text.StartsWith("Build Coffee Machine"))
            {
                promptText.text = ""; // only sets to empty if player just left text radius
            }
        }
        else if (promptText.text.StartsWith("Build Coffee Machine"))
        {
            promptText.text = ""; // only sets to empty if player just left text radius
        }

        quickInteract = false;
    }

    public void Unlock()
    {
        unbuiltPrefab.SetActive(true);
        buildStatus = BuildStatus.Unlocked;
    }

    void Build()
    {
        if (PlayerInventory.scrap >= Config.coffeeMachineScrapCost)
        {
            unbuiltPrefab.SetActive(false);
            builtPrefab.SetActive(true);
            buildStatus = BuildStatus.Built;
            PlayerInventory.scrap -= Config.coffeeMachineScrapCost;
            Config.coffeeMachineBuilt = true;
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

    #endregion input functions
}
