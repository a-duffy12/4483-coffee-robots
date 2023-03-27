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

    [Header("Audio")]
    public AudioClip turretDeployAudio;
    public AudioClip spikesDeployAudio;
    public AudioClip gammaGeneratorDeployAudio;
    public AudioClip rocketTowerDeployAudio;
    AudioSource source;

    GameObject player;
    PlayerInventory inventory;
    private bool quickInteract;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<PlayerInventory>();
        source = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.volume = 1f;
        source.priority = 160;
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
        else if (promptText.text.StartsWith("Place ") && !inventory.defenses.Any())
        {
            promptText.text = ""; // only sets to empty if player just left text radius
        }

        quickInteract = false;
    }

    void PlaceDefense()
    {
        Instantiate(inventory.defenses.First(), transform.position, Quaternion.identity);
        Debug.Log($"{inventory.defenses.First().name} placed at ({transform.position.x}, {transform.position.y}, {transform.position.z})");
        
        if (inventory.defenses.First().name == "Turret")
        {
            source.clip = turretDeployAudio;
        }
        else if (inventory.defenses.First().name == "Spikes")
        {
            source.clip = spikesDeployAudio;
        }
        else if (inventory.defenses.First().name == "Gamma Generator")
        {
            source.clip = gammaGeneratorDeployAudio;
        }
        else if (inventory.defenses.First().name == "Rocket Tower")
        {
            source.clip = rocketTowerDeployAudio;
        }

        source.Play();
        inventory.defenses.RemoveAt(0);

        Destroy(gameObject, source.clip.length);
        //Destroy(gameObject);
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
