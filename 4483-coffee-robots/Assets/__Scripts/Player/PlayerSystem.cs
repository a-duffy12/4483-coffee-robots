using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSystem : MonoBehaviour
{
    [Header("Stats")]
    public float maxHp;

    //[Header("GameObjects")]
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

        maxHp = Config.playerMaxHp;
        currentHp = maxHp;

        // set up healthbar
    }

    // Update is called once per frame
    void Update()
    {
        if (abilityActive && Time.time >= abilityEndTime)
        {
            abilityActive = false;
            Debug.Log("ability ended");
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
        
        // update hp bar

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
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
