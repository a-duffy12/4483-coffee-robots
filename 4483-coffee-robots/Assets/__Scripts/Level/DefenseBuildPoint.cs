using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DefenseBuildPoint : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private TMP_Text promptText;

    GameObject player;
    PlayerInventory inventory;
    private bool quickInteract;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<PlayerInventory>();
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= Config.buildingInteractDistance && Config.gameStage > 0 && Config.gameStage < 5 && inventory.defenses.Any())
        {
            promptText.text = $"Place {inventory.defenses.First().name} [E]";

            if (quickInteract)
            {
                PlaceDefense();
            }
        }
        else if (promptText.text.StartsWith("Place "))
        {
            promptText.text = ""; // only sets to empty if player just left text radius
        }

        quickInteract = false;
    }

    void PlaceDefense()
    {
        Instantiate(inventory.defenses.First(), transform.position, Quaternion.identity);
        Debug.Log($"{inventory.defenses.First().name} placed at ({transform.position.x}, {transform.position.y}, {transform.position.z})");
        inventory.defenses.RemoveAt(0);
        Destroy(gameObject);
    }

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
