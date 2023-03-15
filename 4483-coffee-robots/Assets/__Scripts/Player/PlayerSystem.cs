using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerSystem : MonoBehaviour
{
    //[Header("Stats")]
    [Header("GameObjects")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image abilityBar;
    [SerializeField] private TMP_Text abilityText;
    [SerializeField] private GameObject damageOverlay;

    //[Header("Audio")]
    
    public static bool sleepProtected = false;
    [HideInInspector] public float hp { get { return currentHp; } }
    private float currentHp;
    private float abilityEndTime;
    private float nextAbilityTime;

    AudioSource playerSource;
    CoffeePlant plant;

    void Awake()
    {
        playerSource = GetComponent<AudioSource>();
        plant = GameObject.FindGameObjectWithTag("CoffeePlant").GetComponent<CoffeePlant>();
    }
    
    void Start()
    {
        playerSource.playOnAwake = false;
        playerSource.spatialBlend = 1f;
        playerSource.volume = 1f;
        playerSource.priority = 100;

        currentHp = Config.playerMaxHp;

        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.playerMaxHp, 0, Config.playerMaxHp);
        healthText.text = currentHp.ToString("F0");
    }

    void Update()
    {
        if (Time.time < abilityEndTime)
        {
            abilityBar.fillAmount = Mathf.Clamp((abilityEndTime - Time.time)/Config.abilityDuration, 0, Config.abilityDuration);
            abilityText.text = "";
        }
        else if (Time.time >= abilityEndTime && Time.time < nextAbilityTime)
        {
            abilityBar.fillAmount = 1 - Mathf.Clamp((nextAbilityTime - Time.time)/Config.abilityCooldown, 0, Config.abilityCooldown);
            abilityText.text = (nextAbilityTime - Time.time).ToString("F1");
        }
        else
        {
            abilityText.text = "";
        }

        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.playerMaxHp, 0, Config.playerMaxHp);
        healthText.text = currentHp.ToString("F0");
    }

    public void DamagePlayer(float damage, string source = "")
    {
        if (!PlayerSystem.sleepProtected) // cannot take damage in menus
        {
            damage = (float)Mathf.FloorToInt(damage);
        
            if (Time.time < abilityEndTime) // ability is active
            {
                currentHp -= damage * Config.abilityDamageModifier;

                if (Config.aAbilityUnlocked) // return health to plant
                {
                    plant.DamageBuilding(-1 * damage * (1 - Config.abilityDamageModifier) * Config.diversionModifier, "player");
                }
            }
            else 
            {
                currentHp -= damage;
            }

            if (damage > 0)
            {
                StartCoroutine(DamagePlayerOverlay(0.1f));
            }

            if (currentHp > Config.playerMaxHp)
            {
                currentHp = Config.playerMaxHp;
            }

            if (currentHp > 0 && currentHp < 1)
            {
                currentHp = 1;
            }

            healthBar.fillAmount = Mathf.Clamp(currentHp/Config.playerMaxHp, 0, Config.playerMaxHp);
            if (currentHp > 0)
            {
                healthText.text = currentHp.ToString("F0");
            }
            else
            {
                healthText.text = "DEAD";
            }
            
            if (damage > 0)
            {
                StartCoroutine(DamagePlayerOverlay(0.1f)); // flash screen
            }

            if (currentHp <= 0)
            {
                Time.timeScale = 0.0001f;
                StartCoroutine(KillPlayer());
            }
        }
    }

    IEnumerator DamagePlayerOverlay(float duration)
    {
        damageOverlay.SetActive(true);
        
        yield return new WaitForSeconds(duration);

        damageOverlay.SetActive(false);
    }

    IEnumerator KillPlayer()
    {
        // death audio

        Time.timeScale = 0.0001f;

        yield return new WaitForSeconds(0.0005f);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    #region input functions

    public void Ability(InputAction.CallbackContext con)
	{
		if (con.performed && Time.time >= nextAbilityTime)
		{
            abilityEndTime = Time.time + Config.abilityDuration;
            nextAbilityTime = abilityEndTime + Config.abilityCooldown;
		}
	}

    #endregion input functions
}
