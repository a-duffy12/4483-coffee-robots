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
    //[Header("Audio")]
    
    [HideInInspector] public float hp { get { return currentHp; } }
    private float currentHp;
    private float abilityEndTime;
    private float nextAbilityTime;
    private bool abilityActive;

    AudioSource playerSource;

    void Awake()
    {
        playerSource = GetComponent<AudioSource>();
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

    // Update is called once per frame
    void Update()
    {
        if (abilityActive && Time.time >= abilityEndTime)
        {
            abilityActive = false;
            Debug.Log("ability ended");
        }

        abilityBar.fillAmount = 1 - Mathf.Clamp((nextAbilityTime - Time.time)/Config.abilityCooldown, 0, Config.abilityCooldown);
        if (abilityBar.fillAmount < 1)
        {
            abilityText.text = (nextAbilityTime - Time.time).ToString("F1");
        }
        else
        {
            abilityText.text = "";
        }
    }

    public void DamagePlayer(float damage, string source = "")
    {
        if (!abilityActive)
        {
            currentHp -= damage;
        }
        else 
        {
            currentHp -= damage * Config.abilityDamageModifier;
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
        healthText.text = currentHp.ToString("F0");
        
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

    IEnumerator DamagePlayerOverlay(float duration)
    {
        //damageOverlay.SetActive(true);
        
        yield return new WaitForSeconds(duration);

        //damageOverlay.SetActive(false);
    }

    IEnumerator KillPlayer()
    {
        yield return new WaitForSeconds(0.0005f);

        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.name);
    }

    #region input functions

    public void Ability(InputAction.CallbackContext con)
	{
		if (con.performed && Time.time >= nextAbilityTime)
		{
            abilityActive = true;
            abilityEndTime = Time.time + Config.abilityDuration;
            nextAbilityTime = Time.time + Config.abilityCooldown;
            Debug.Log("ability started");
		}
	}

    #endregion input functions
}
